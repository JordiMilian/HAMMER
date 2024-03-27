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
    private IEnumerator Start()
    {
        Debug.Log("les go");
        yield return new WaitForSeconds(2);
        verticalForce = 0f;
        float timer = 0;
        isPushing = true;
        Debug.Log("les go?¿");
        while (maxForce - verticalForce > 0.5f )
        {
            Debug.Log("?¿?¿");
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
    }
    private void FixedUpdate()
    {
        Debug.Log("updating fixedly");
        if (isPushing)
        {
            HeadRb.AddForce(Vector2.up * verticalForce);
            HeadRb.AddTorque(verticalForce / 40);
            Debug.Log("pushing with: " + verticalForce);
        }
    }
}
