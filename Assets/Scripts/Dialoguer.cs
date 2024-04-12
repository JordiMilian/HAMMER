using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Generic_OnTriggerEnterEvents;

public class Dialoguer : MonoBehaviour
{
    public List<string> TextLines = new List<string>();
    
    [SerializeField] TextMeshProUGUI MeshPro;
    [SerializeField] GameObject DialogueBubblePosition;
    [SerializeField] Generic_OnTriggerEnterEvents PlayerCloseTrigger;

    int CurrentLineToRead;
    bool playerIsInside;
    bool currentlyReading;

    Coroutine currentRead;
    Coroutine currentLinesReseter;

    private void OnEnable()
    {
        PlayerCloseTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        PlayerCloseTrigger.OnTriggerEntered += PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited += PlayerExitedDialogue;
    }
    private void OnDisable()
    {
        PlayerCloseTrigger.OnTriggerEntered -= PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited -= PlayerExitedDialogue;


        HideDialogueBubble();
        RemoveDialoguerFromTargetGroup();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) 
        {
            if (currentlyReading) { CompleteCurrentRead(); }
            else if (playerIsInside) NextLine();
        }
    }
    void ShowNextLine()
    {
        // stop reading and start de reading corroutine
        if(currentRead != null) { StopCoroutine(currentRead); }
        currentRead = StartCoroutine( SlowlyReadLine(TextLines[CurrentLineToRead]));

        //I dont know what this does but it mostly works. It updates the size of the bubble
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)MeshPro.transform);
    }
    IEnumerator SlowlyReadLine(string finalText)
    {
        currentlyReading = true;
        MeshPro.text = "";
        bool skipping = false;;
        foreach (char c in finalText) 
        { 
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
        DialogueBubblePosition.SetActive(false);
    }
    void ShowDialogueBubble()
    {
        DialogueBubblePosition.SetActive(true);
    }
    void NextLine()
    {
        if(CurrentLineToRead == TextLines.Count -1) //Si es lultima linea de text
        {
            HideDialogueBubble();
            CurrentLineToRead++;
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
    void PlayerEnterDialogue(object sender, EventArgsCollisionInfo args)
    {
        playerIsInside = true;
        CheckIfOutOfRange();
        ShowNextLine();
        // Lets try without reseting the lines ok??
        // if(currentLinesReseter != null) { StopCoroutine(currentLinesReseter); }
        ShowDialogueBubble();

        AddDialoguerToTargetGroup();
  
    }
    void PlayerExitedDialogue(object sender, EventArgsCollisionInfo args)
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
