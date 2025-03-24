using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp_ShowOnPickUpWeapon : MonoBehaviour
{
    [SerializeField] UI_TutorialPopUp_Script popUpScript;
    [SerializeField] GameState gameState;
    private void OnEnable()
    {
        GlobalPlayerReferences.Instance.references.weaponSwitcher.OnPickedNewWeapon += showPopUp;
    }
    private void OnDisable()
    {
        GlobalPlayerReferences.Instance.references.weaponSwitcher.OnPickedNewWeapon -= showPopUp;
    }
    void showPopUp(int indexInGameState)
    {
        if (!gameState.hasPickedFirstUpgrade)
        {
            popUpScript.ShowPopUp();
            gameState.hasPickedFirstUpgrade = true;
        }

        GlobalPlayerReferences.Instance.references.weaponSwitcher.OnPickedNewWeapon -= showPopUp;
    }
}
