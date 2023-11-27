using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningHead : MonoBehaviour
{
    [SerializeField] AnimationCurve SpinningCurve;
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
        float randomHorizontal = Random.Range(0f, 1f);
        
        float timer = 0;
        float weightY = 0;
        float weightX = 0;
        float weightRotate = 0;
        float weightAlpha = 0;

        while (timer < Lifetime)
        {
            timer += Time.deltaTime;
            weightAlpha = AlphaCurve.Evaluate(timer / Lifetime);    
            weightY = VerticalCurve.Evaluate(timer / Lifetime);
            weightX = HorizontalCurve.Evaluate(randomHorizontal);
            weightRotate += SpinningCurve.Evaluate(timer / Lifetime);
            transform.Translate(new Vector2(weightX, weightY));
            SpriteGO.transform.rotation = Quaternion.Euler(0f, 0f,weightRotate*-weightX*50);
            headSprite.color = new Color(1, 1, 1, weightAlpha);
            yield return null;
        }
       Destroy(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) { StartCoroutine(HeadSpin()); }
    }
}
