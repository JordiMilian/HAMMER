using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Flash : MonoBehaviour
{
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashTime = 0.25f;
    [SerializeField] GameObject SpritesRoot;

    SpriteRenderer[] spriteRenderers;
    Material[] materials;
    void Awake()
    {
        spriteRenderers = SpritesRoot.GetComponentsInChildren<SpriteRenderer>();

        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }
    public void CallDefaultFlasher()
    {
        StartCoroutine(Flasher(flashTime, flashColor));
    }
    public void CallCustomFlash(float time, Color color)
    {
        StartCoroutine(Flasher(time,color));
    }
    Coroutine controlledFlash;
    public void StartFlashing(float time)
    {
        if (controlledFlash != null) { StopCoroutine(controlledFlash); }

        controlledFlash = StartCoroutine(startFlash());

        IEnumerator startFlash()
        {
            float timer = 0;
            while(timer < time)
            {
                timer += Time.deltaTime;
                SetFlashAmount(timer / time);
                yield return null;
            }
            SetFlashAmount(1);
        }
    }
    public void EndFlashing(float time)
    {
        if (controlledFlash != null) { StopCoroutine(controlledFlash); }

        controlledFlash = StartCoroutine(endFlash());

        IEnumerator endFlash()
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                SetFlashAmount(1 - (timer / time));
                yield return null;
            }
            SetFlashAmount(0);
        }
    }
    private IEnumerator Flasher(float time, Color color)
    {
        SetFlashColors();
        float CurrentFlashAmount = 0;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime * Time.timeScale;
            CurrentFlashAmount = 1- (elapsedTime/time);
            SetFlashAmount(CurrentFlashAmount);
            yield return null;
        }
        
    }
    private void SetFlashColors()
    {
        for(int i = 0;i < materials.Length;i++)
        {
            materials[i].SetColor("_FlashColor", flashColor);
        }
    }

    void SetFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmound", amount);
        }
    }
}
