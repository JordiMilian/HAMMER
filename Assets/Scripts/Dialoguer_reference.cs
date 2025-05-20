using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialoguer_reference : MonoBehaviour
{
    public Dialoguer Dialoguer;
    [SerializeField] DialoguesContainer container;
    [SerializeField] int indexInContainer;
    [SerializeField] List<string> newTextLines = new List<string>();
    [SerializeField] bool useReferenceLines;

    private void Start()
    {
        if (useReferenceLines)
        {
            Dialoguer.TextLines = newTextLines;
        }
        Dialoguer.TrySetDialoguesFromContainer(container, indexInContainer);
    }
}
