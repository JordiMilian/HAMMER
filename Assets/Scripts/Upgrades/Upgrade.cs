using UnityEngine;


public abstract class Upgrade : ScriptableObject
{
    public UpgradesType upgradeType;
    public upgradeRarity rarity;
    public Sprite iconSprite;
    public string Title;
    public abstract void onAdded(GameObject entity);

    public abstract void onRemoved(GameObject entity);

    public abstract string shortDescription();
}
