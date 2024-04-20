using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Foregrounder : MonoBehaviour
{
    [SerializeField] float ChangeTime;
    [SerializeField] Vector2 MinMaxDistance;
    float distanceToPlayer;
    Transform playerTF;
    Material[] materials;
    private void Awake()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
        playerTF = GameObject.Find(TagsCollection.MainCharacter).transform;
    }
    private void Update()
    {
        //Utilitzem SqrMagnitud, per aixo al fer el normalized ho elebem a 2
        distanceToPlayer = (transform.position - playerTF.position).sqrMagnitude;
        float normalizedDistance = Mathf.InverseLerp(Mathf.Pow(MinMaxDistance.x,2), Mathf.Pow(MinMaxDistance.y, 2), distanceToPlayer);
        setTotalOpacity(normalizedDistance);
    }
    public void CallTurnBlack() { StartCoroutine(TurnBlack()); }
    public void CallTurnColor() { StartCoroutine(TurnColor()); }
    IEnumerator TurnBlack()
    {
        float timer = 0;
        setCrossedBool(true);
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
        setCrossedBool(false);
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
    void setTotalOpacity(float opacity)
    {
        foreach(Material mat in materials)
        {
            mat.SetFloat("_TotalOpacity", opacity);
        }  
    }
    void setCrossedBool(bool crossed)
    {
        int add = 1;
        if (crossed) { add = 0; }
        foreach (Material mat in materials)
        {
            mat.SetFloat("_crossedFloat", add);
        }
    }
}
