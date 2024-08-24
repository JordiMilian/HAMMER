using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialoguer_reference : MonoBehaviour
{
    [SerializeField] Dialoguer dialoguerReferenced;
    [SerializeField] DialoguesContainer container;
    [SerializeField] int indexInContainer;
    [SerializeField] List<string> newTextLines = new List<string>();

    private void Awake()
    { 
        if(!dialoguerReferenced.TrySetDialoguesFromContainer(container, indexInContainer))
        {
            dialoguerReferenced.TextLines = newTextLines;
        }
    }
}
