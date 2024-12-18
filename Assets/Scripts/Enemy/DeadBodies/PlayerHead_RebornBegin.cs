using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead_RebornBegin : MonoBehaviour
{
    [SerializeField] Rigidbody2D HeadRb;
    [SerializeField] float maxForce;
    [SerializeField] List<SpriteRenderer> spritesList;
    float verticalForce;
    bool isPushing;
    public IEnumerator FlyAwayCoroutine()
    {
        verticalForce = 0f;
        float timer = 0;
        isPushing = true;
        while (maxForce - verticalForce > 0.5f )
        {
            timer += Time.deltaTime;
            verticalForce = Mathf.Lerp(verticalForce, maxForce, 0.05f);
            yield return null;
        }
        StartCoroutine(FadeSprites());
        isPushing = false;
    }
    IEnumerator FadeSprites()
    {
        float timer = 0;
        float maxTime = 0.2f;
        while (timer < maxTime) 
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.InverseLerp(maxTime, 0, timer);
            foreach (SpriteRenderer sprite in spritesList)
            {
                sprite.color = new Color(1, 1, 1, normalizedTime);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (isPushing)
        {
            HeadRb.AddForce(Vector2.up * verticalForce);
            HeadRb.AddTorque(verticalForce / 40);
        }
    }
}
