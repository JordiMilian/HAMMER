using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dialoguer : MonoBehaviour
{
    public List<string> TextLines = new List<string>();

    public DialoguesContainer dialoguesContainer;
    public int dialoguesIndex;

    public Action<int> onFinishedReading;
    
    [SerializeField] TextMeshProUGUI MeshPro;
    [SerializeField] GameObject DialogueBubblePosition;
    [SerializeField] Generic_OnTriggerEnterEvents PlayerCloseTrigger;
    [SerializeField] Animator bubbleAnimator;

    int CurrentLineToRead;
    bool playerIsInside;
    bool currentlyReading;
    bool isDisplaying;

    Coroutine currentRead;
    Coroutine currentLinesReseter;

    IDamageReceiver damageReceiver;
    [SerializeField]GameObject damageReceiverRoot;
    private void OnValidate()
    {
        if(damageReceiverRoot != null)
        {
            UsefullMethods.CheckIfGameobjectImplementsInterface<IDamageReceiver>(ref damageReceiverRoot, ref damageReceiver);
        }
    }

    private void Awake()
    {
        TrySetDialoguesFromContainer(dialoguesContainer, dialoguesIndex);
    }
    public void UpdateDialogues()
    {
        TrySetDialoguesFromContainer(dialoguesContainer, dialoguesIndex);
    }
    public bool TrySetDialoguesFromContainer(DialoguesContainer container, int index)
    {
        if (container == null) { return false; }
        if (index < 0) { return false; }

        TextLines = container.AllDialoguesList[index].DialogueLines;
        return true;
        
    }
    private void OnEnable()
    {
        PlayerCloseTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        PlayerCloseTrigger.OnTriggerEntered += PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited += PlayerExitedDialogue;
        damageReceiver.OnDamageReceived_event += OnInteracted;

        isDisplaying = true;
        InstaHideDialogueBubble();
    }
    private void OnDisable()
    {
        PlayerCloseTrigger.OnTriggerEntered -= PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited -= PlayerExitedDialogue;


        HideDialogueBubble();
        RemoveDialoguerFromTargetGroup();
        damageReceiver.OnDamageReceived_event -= OnInteracted;
    }
    void OnInteracted(ReceivedAttackInfo info)
    {
        if (currentlyReading) { CompleteCurrentRead(); }
        else if (playerIsInside) NextLine();
    }
    void ShowNextLine()
    {
        // stop reading and start de reading corroutine
        if(currentRead != null) { StopCoroutine(currentRead); }
        currentRead = StartCoroutine( SlowlyReadLine(TextLines[CurrentLineToRead]));

        //I dont know what this does but it mostly works. It updates the size of the bubble
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)MeshPro.transform);

        bubbleAnimator.SetTrigger("NextLine");
    }
    IEnumerator SlowlyReadLine(string finalText)
    {
        currentlyReading = true;
        MeshPro.text = "";
        bool skipping = false;;
        foreach (char c in finalText) 
        { 
            // for coloring text
            if(c == '<') { skipping = true; MeshPro.text += c; continue; }
            if(c == '>') { skipping = false; MeshPro.text += c; continue; }
            if (skipping) { MeshPro.text += c; continue; }

            MeshPro.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        currentlyReading = false;
    }
    void CompleteCurrentRead()
    {
        if(currentRead != null) { StopCoroutine(currentRead);}
        MeshPro.text = TextLines[CurrentLineToRead];
        currentlyReading = false;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)MeshPro.transform);
        
    }
    void HideDialogueBubble()
    {
        if(isDisplaying)
        {
            bubbleAnimator.SetTrigger("Disappear");
            isDisplaying = false;
        }
    }
    void InstaHideDialogueBubble()
    {
        if(isDisplaying)
        {
            bubbleAnimator.SetTrigger("InstaDisappear");
            isDisplaying = false;
        }
    }
    
    void ShowDialogueBubble()
    {
        if(!isDisplaying)
        {
            DialogueBubblePosition.SetActive(true);
            isDisplaying = true;
            bubbleAnimator.SetTrigger("Appear");
        }
        
    }
    void NextLine()
    {
        if(CurrentLineToRead == TextLines.Count -1) //Si es lultima linea de text
        {
            HideDialogueBubble();
            CurrentLineToRead++;
            onFinishedReading?.Invoke(dialoguesIndex);
            return;
        }
        if(CurrentLineToRead >= TextLines.Count) //Si ho has llegit tot i vols tornar a llegir et torna al principi
        {
            CurrentLineToRead = 0;
            ShowDialogueBubble();
            ShowNextLine();
            return;
        }

        //Comportament habitual
        CurrentLineToRead++;
        ShowNextLine();
    }
    IEnumerator TimerToResetLines(float maxTime)
    {
        float timer = 0;
        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        CurrentLineToRead = 0;
    }
    void PlayerEnterDialogue(Collider2D collision)
    {
        playerIsInside = true;
        CheckIfOutOfRange();
        ShowNextLine();
        // Lets try without reseting the lines ok??
        // if(currentLinesReseter != null) { StopCoroutine(currentLinesReseter); }
        ShowDialogueBubble();

        AddDialoguerToTargetGroup();
  
    }
    void PlayerExitedDialogue(Collider2D collision)
    {
        playerIsInside = false;

        // currentLinesReseter = StartCoroutine(TimerToResetLines(10));

        HideDialogueBubble();

        RemoveDialoguerFromTargetGroup();
    }

    void CheckIfOutOfRange() //Quan entres de nou, si ja tho has llegit tot o t'havies passat, tornes al principi
    {
        if(CurrentLineToRead >= TextLines.Count -1)
        {
            CurrentLineToRead = 0;
        }
    }
    void AddDialoguerToTargetGroup()
    {
        TargetGroupSingleton.Instance.AddTarget(transform, 1.5f, 1.5f);
    }
    void RemoveDialoguerFromTargetGroup()
    {
        TargetGroupSingleton.Instance.RemoveTarget(transform);
    }
}
