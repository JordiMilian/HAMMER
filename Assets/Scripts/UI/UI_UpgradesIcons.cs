using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradesIcons : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] List<UnityEngine.UI.Image> iconsImages = new List<UnityEngine.UI.Image>();
    [SerializeField] float distanceBetweenIcons;
    [SerializeField] bool triggerUpdateUI = true;
    Player_EventSystem playerEvents;

    private void OnEnable()
    {
        playerEvents = GlobalPlayerReferences.Instance.references.events;

        playerEvents.OnPickedNewUpgrade += (UpgradeContainer container) => updateIcons();
        playerEvents.OnRemovedUpgrade += updateIcons;

        updateIcons();
    }
    private void OnDisable()
    {
        playerEvents.OnPickedNewUpgrade -= (UpgradeContainer container) => updateIcons();
        playerEvents.OnRemovedUpgrade -= updateIcons;
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
}
