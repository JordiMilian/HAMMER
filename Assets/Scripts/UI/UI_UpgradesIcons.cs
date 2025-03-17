using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradesIcons : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] List<UnityEngine.UI.Image> iconsImages = new List<UnityEngine.UI.Image>();
    [SerializeField] float distanceBetweenIcons;
    [SerializeField] bool triggerUpdateUI = true;
    [SerializeField] Player_UpgradesManager upgradesManger;

    private void OnEnable()
    {
        upgradesManger.OnUpdatedUpgrades += updateIcons;

        updateIcons();
    }
    private void OnDisable()
    {
        upgradesManger.OnUpdatedUpgrades -= updateIcons;
    }
    void updateIcons()
    {
        foreach(UnityEngine.UI.Image image in iconsImages)
        {
            image.enabled = false;
        }

        float acumulatedDistance = 0;

        for (int i = 0; i < gameState.playerUpgrades.Count; i++)
        {
            iconsImages[i].enabled = true;

            iconsImages[i].sprite = gameState.playerUpgrades[i].iconSprite;

            iconsImages[i].rectTransform.localPosition = new Vector3(acumulatedDistance, 0, 0);  

            acumulatedDistance += distanceBetweenIcons;
        }

    }
    private void Update()
    {
        if (triggerUpdateUI)
        {
            updateIcons();
            triggerUpdateUI = false;
        }
    }
}
