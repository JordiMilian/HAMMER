using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new Dialogues Container")]
public class DialoguesContainer : ScriptableObject
{
    [Serializable]
    public class Dialogue
    {
        //public dialoguesEnum thisDialogueEnum;
        public string shortDescription;
        public List<string> DialogueLines = new List<string>();
    }
    public List<Dialogue> AllDialoguesList = new List<Dialogue>();

    /*
    public Dialogue GetDIalogueList(dialoguesEnum key)
    {
        foreach (Dialogue list in AllDialoguesList)
        {
            if(list.thisDialogueEnum == key)
            {
                return list;
            }
        }
        Debug.LogError("Missing dialgue");
        return null;
    }
    */
}
