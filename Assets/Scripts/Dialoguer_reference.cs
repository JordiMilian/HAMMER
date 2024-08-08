using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialoguer_reference : MonoBehaviour
{
    [SerializeField] Dialoguer dialoguerReferenced;
    [SerializeField] List<string> newTextLines = new List<string>();

    private void Awake()
    {
        dialoguerReferenced.TextLines = newTextLines;
    }
}
