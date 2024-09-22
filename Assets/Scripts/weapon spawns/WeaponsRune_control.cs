using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsRune_control : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] List<SpriteRenderer> RunesSprites = new List<SpriteRenderer>();
    [SerializeField] Sprite SpriteOn, SpriteOff;
    private void OnEnable()
    {
        SetRunesSprites(new WeaponPrefab_infoHolder());
        GlobalPlayerReferences.Instance.references.events.OnPickedNewWeapon += SetRunesSprites;
    }
    public void SetRunesSprites(WeaponPrefab_infoHolder info)
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
