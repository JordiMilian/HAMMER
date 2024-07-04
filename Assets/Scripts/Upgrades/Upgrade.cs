using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Upgrade : ScriptableObject
{
    public Sprite iconSprite;
    public abstract void onAdded(GameObject entity);

    public abstract void onRemoved(GameObject entity);

    public abstract string shortDescription();

}
