using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp_ShowOnPickUpgrade : MonoBehaviour
{
    [SerializeField] UI_TutorialPopUp_Script popUpScript;
    [SerializeField] GameState gameState;
    private void OnEnable()
    {
        if (gameState.hasPickedFirstUpgrade) { return; }

        GlobalPlayerReferences.Instance.references.events.OnPickedNewUpgrade += showPopUp;
    }
    private void OnDisable()
    {
        GlobalPlayerReferences.Instance.references.events.OnPickedNewUpgrade -= showPopUp;
    }
    void showPopUp(UpgradeContainer upgrade)
    {
        if (!gameState.hasPickedFirstUpgrade) 
        { 
            popUpScript.ShowPopUp();
            gameState.hasPickedFirstWeapon = true;
        }
        
        GlobalPlayerReferences.Instance.references.events.OnPickedNewUpgrade -= showPopUp;
    }
}
