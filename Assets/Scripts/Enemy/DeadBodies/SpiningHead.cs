using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningHead : MonoBehaviour
{
    [SerializeField] AnimationCurve RandomDirectionCurve;
    [SerializeField] AnimationCurve VerticalCurve;
    [SerializeField] AnimationCurve HorizontalCurve;
    [SerializeField] AnimationCurve AlphaCurve;
    [SerializeField] float Lifetime;
    SpriteRenderer headSprite;
    GameObject SpriteGO;
    void Start()
    {
        headSprite = GetComponentInChildren<SpriteRenderer>();
        SpriteGO = headSprite.gameObject;
        StartCoroutine(HeadSpin());
    }
    IEnumerator HeadSpin()
    {
        Vector2 origin = transform.position; 
        float randomHorizontal = RandomDirectionCurve.Evaluate( Random.Range(0f, 1f));
        
        float timer = 0;
        float weightY = 0;
        float weightX = 0;
        float weightRotate = 0;
        float weightAlpha = 0;

        while (timer < Lifetime)
        {
            timer += Time.deltaTime;
               
            weightY = VerticalCurve.Evaluate(timer / Lifetime);
            weightX = HorizontalCurve.Evaluate(timer/Lifetime) * randomHorizontal;
            transform.position = origin + new Vector2(weightX, weightY);

            weightRotate += 3 * Time.timeScale;
            SpriteGO.transform.rotation = Quaternion.Euler(0f, 0f,weightRotate*-randomHorizontal);

            weightAlpha = AlphaCurve.Evaluate(timer / Lifetime);
            headSprite.color = new Color(1, 1, 1, weightAlpha);
            yield return null;
        }
       Destroy(gameObject);
    }
}
