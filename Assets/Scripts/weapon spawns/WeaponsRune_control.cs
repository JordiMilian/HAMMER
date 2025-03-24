using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsRune_control : MonoBehaviour
{
    //This controls the runes signs below the weapons in the Alto Mando room

    [SerializeField] GameState gameState;
    [SerializeField] List<SpriteRenderer> RunesSprites = new List<SpriteRenderer>();
    [SerializeField] Sprite SpriteOn, SpriteOff;
    private void OnEnable()
    {
        SetRunesSprites();
        GlobalPlayerReferences.Instance.references.weaponSwitcher.OnPickedNewWeapon += (int info) => SetRunesSprites();
    }
    public void SetRunesSprites()
    {
        for (int i = 0; i < gameState.WeaponInfosList.Count; i++)
        {
            if (gameState.WeaponInfosList[i].isUnlocked)
            {
                RunesSprites[i].sprite = SpriteOn;
            }
            else { RunesSprites[i].sprite = SpriteOff; }
        }
    }
}
