using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Foregrounder : MonoBehaviour
{
    [SerializeField] float ChangeTime;
    Material[] materials;
    private void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }
    public void CallTurnBlack() { StartCoroutine(TurnBlack()); }
    public void CallTurnColor() { StartCoroutine(TurnColor()); }
    IEnumerator TurnBlack()
    {
        float timer = 0;
        while (timer < ChangeTime)
        {
            timer += Time.deltaTime;
            float lerpedOpacity = Mathf.InverseLerp(0, ChangeTime, timer);
            setOpacity(lerpedOpacity);
            yield return null;
        }
        setOpacity(1);
    }
    IEnumerator TurnColor()
    {
        float timer = 0;
        while (timer < ChangeTime)
        {
            timer += Time.deltaTime;
            float lerpedOpacity = Mathf.InverseLerp(ChangeTime, 0, timer);
            setOpacity(lerpedOpacity);
            yield return null;
        }
        setOpacity(0);
    }
    void setOpacity(float lerpedOpacity)
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_ColorOpacity", lerpedOpacity);
        }
    }
}
