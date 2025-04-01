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
        StartCoroutine(Flasher(flashTime));
    }
    public void CallCustomFlash(float time)
    {
        StartCoroutine(Flasher(time));
    }
    private IEnumerator Flasher(float time)
    {
        SetFlashColors();
        float CurrentFlashAmount = 0;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime * Time.timeScale;
            CurrentFlashAmount = Mathf.Lerp(1f,0f,elapsedTime/time);
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
