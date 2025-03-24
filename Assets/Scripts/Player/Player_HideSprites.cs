using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HideSprites : MonoBehaviour
{
    [SerializeField] GameObject[] PlayerSpritesRoots;

    public void HidePlayerSprites()
    {
        foreach (GameObject root in PlayerSpritesRoots)
        {
            root.SetActive(false);
        }
    }
    public void ShowPlayerSprites()
    {
        foreach (GameObject root in PlayerSpritesRoots)
        {
            root.SetActive(true);
        }
    }
}
