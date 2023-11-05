using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Flash : MonoBehaviour
{
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashTime = 0.25f;

    SpriteRenderer[] spriteRenderers;
    Material[] materials;
        // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }
    public void CallFlasher()
    {
        StartCoroutine(Flasher());
    }
    private IEnumerator Flasher()
    {
        SetFlashColors();
        float CurrentFlashAmount = 0;
        float elapsedTime = 0;

       

        while (elapsedTime < flashTime)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            CurrentFlashAmount = Mathf.Lerp(1f,0f,elapsedTime/flashTime);
            Debug.Log(CurrentFlashAmount);
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
