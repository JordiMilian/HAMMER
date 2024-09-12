using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp_ShowOnTrigger : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents onTriggerEnter;
    [SerializeField] UI_TutorialPopUp_Script PopUpScript;

    private void OnEnable()
    {
        onTriggerEnter.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        onTriggerEnter.OnTriggerEntered += ShowPopUp;
    }
    private void OnDisable()
    {
        onTriggerEnter.OnTriggerEntered -= ShowPopUp;
    }
    void ShowPopUp(Collider2D collider)
    {
        PopUpScript.ShowPopUp();
        onTriggerEnter.OnTriggerEntered -= ShowPopUp;
    }

}
