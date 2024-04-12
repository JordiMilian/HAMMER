using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextBoxToPreferedSize : MonoBehaviour
{
    
    private void Update()
    {
        AdjustTextBoxSize();
    }
    void AdjustTextBoxSize()
    {
        // Get the RectTransform component of the TextMeshPro object
        Transform yoquese = transform;

        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        

        Vector2 textSize = GetComponent<TextMeshPro>().GetPreferredValues(textMeshPro.text);
        Debug.Log(textSize);
        /*
// Adjust the size of the RectTransform to match the preferred text size
rectTransform.sizeDelta = new Vector2(textSize.x, textSize.y);
;
*/
    }
}
