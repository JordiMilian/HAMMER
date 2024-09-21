using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsRune_control : MonoBehaviour
{
    [SerializeField] WeaponsInfos_AltoMandoSpawns altoMandoSpawns;
    [SerializeField] List<SpriteRenderer> RunesGO = new List<SpriteRenderer>();
    [SerializeField] Sprite SpriteOn, SpriteOff;
    private void OnEnable()
    {
        for (int i = 0; i < RunesGO.Count; i++)
        {
            if (altoMandoSpawns.weaponSpawners[i].isUnlocked)
            {
                RunesGO[i].sprite = SpriteOn;
            }
            else { RunesGO[i].sprite = SpriteOff; }
        }
    }
}
