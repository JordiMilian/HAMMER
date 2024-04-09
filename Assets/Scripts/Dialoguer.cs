using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class Dialoguer : MonoBehaviour
{
    public List<string> TextLines = new List<string>();
    public int CurrentLineToRead;
    [SerializeField] TextMeshProUGUI MeshPro01, MeshPro02;
    [SerializeField] GameObject Position01, Position02;
    [SerializeField] Generic_OnTriggerEnterEvents PlayerCloseTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents Position01_Collider, Position02_Collider;
    [SerializeField] CinemachineTargetGroup targetGroup;

    [SerializeField] float DelayToRestartLines;
    bool playerIsInside;
    bool isDelaying;
    public float timer;
    private void Awake()
    {
        targetGroup = GameObject.Find(TagsCollection.TargetGroup).GetComponent<CinemachineTargetGroup>();
    }
    private void OnEnable()
    {
        PlayerCloseTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        PlayerCloseTrigger.OnTriggerEntered += PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited += PlayerExitedDialogue;

        Position01_Collider.AddActivatorTag("Player");
        Position02_Collider.AddActivatorTag("Player");
        Position01_Collider.OnTriggerEntered += SwitchPosition01;
        Position02_Collider.OnTriggerEntered += SwitchPosition02;
    }
    private void OnDisable()
    {
        PlayerCloseTrigger.OnTriggerEntered -= PlayerEnterDialogue;
        PlayerCloseTrigger.OnTriggerExited -= PlayerExitedDialogue;

        Position01_Collider.OnTriggerEntered -= SwitchPosition01;
        Position02_Collider.OnTriggerEntered -= SwitchPosition02;

        Position02.SetActive(false);
        Position01.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (playerIsInside) NextLine();
        }
        if (isDelaying)
        {
            timer += Time.deltaTime;

            if (timer >= DelayToRestartLines)
            {
                isDelaying = false;
                RestartLinesCount();
            }
        }
    }
    void ShowLine()
    {
        MeshPro01.text = TextLines[CurrentLineToRead];
        MeshPro02.text = TextLines[CurrentLineToRead];
    }
    void SwitchPosition01(object sender, EventArgsCollisionInfo args)
    {
        Position01.SetActive(false);
        Position02.SetActive(true);
    }
    void SwitchPosition02(object sender, EventArgsCollisionInfo args)
    {
        Position01.SetActive(true);
        Position02.SetActive(false);
    }
    void HideAllPositions()
    {
        Position01.SetActive(false);
        Position02.SetActive(false);
    }
    void NextLine()
    {
        if(CurrentLineToRead == TextLines.Count -1)
        {
            HideAllPositions();
            CurrentLineToRead++;
            return;
        }
        if(CurrentLineToRead >= TextLines.Count)
        {
            CurrentLineToRead = 0;
            Position01.SetActive(true);
            ShowLine();
            return;
        }
        CurrentLineToRead++;
        ShowLine();
    }
    void RestartLinesCount()
    {
        CurrentLineToRead = 0;
    }
    void PlayerEnterDialogue(object sender, EventArgsCollisionInfo args)
    {
        playerIsInside = true;
        CheckIfOutOfRange();
        ShowLine();
        Position01.SetActive(true);
        isDelaying = false;
        AddDialoguerToTargetGroup();
  
    }
    void PlayerExitedDialogue(object sender, EventArgsCollisionInfo args)
    {
        playerIsInside = false;
        ResetDelay();
        HideAllPositions();
        RemoveDialoguerFromTargetGroup();
    }

    void CheckIfOutOfRange()
    {
        if(CurrentLineToRead >= TextLines.Count -1)
        {
            CurrentLineToRead = 0;
        }
    }
    private void ResetDelay()
    {
        isDelaying = true;
        timer = 0;
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
