using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents EndCollider;
    List<MaskableGraphic> images = new List<MaskableGraphic>();
    [SerializeField] UI_BaseAction UI_Action;
    [SerializeField] GameObject EndScreenRootImage;
    bool isDisplaying;

    private void OnEnable()
    {
        images = GetComponentsInChildren<MaskableGraphic>().ToList<MaskableGraphic>();
        EndCollider.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        isDisplaying = false;
        EndCollider.OnTriggerEntered += playerEnteredEndCollider;
        InputDetector.Instance.OnPausePressed += onPausePressed;
    }
    private void OnDisable()
    {
        EndCollider.OnTriggerEntered -= playerEnteredEndCollider;
        InputDetector.Instance.OnPausePressed -= onPausePressed;
    }
    private void Start()
    {
        FadeOut(0);
    }
   void onPausePressed()
    {
        if(isDisplaying)
        {
            UI_Action.Action(new UI_Button());
        }
    }
    
    void playerEnteredEndCollider(Collider2D collision)
    {
        EndScreenRootImage.SetActive(true);//not used perque si no estan actius no busca be les imatges crec

        FadeIn(1f);

        collision.transform.root.GetComponent<Player_EventSystem>().CallDisable?.Invoke(); //Disable player movement and such
    }
    void FadeOut(float time)
    {
        foreach (MaskableGraphic i in images) 
        {
            StartCoroutine(FadeOutUiElement(i, time));
        }
        isDisplaying = false;
        EndScreenRootImage.SetActive(false);
    }
    void FadeIn(float time)
    {
        foreach (MaskableGraphic i in images)
        {
            StartCoroutine(FadeInUIElement(i, time));
        }
        isDisplaying = true;
        
    }
    IEnumerator FadeOutUiElement(MaskableGraphic im, float duration)
    {
        float timer = 0;
        float inverseNormalizedTime = 0;
        Color baseColor = im.color; //get the original color so we dont mess up
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            inverseNormalizedTime = timer/duration;
            im.color = new Color(baseColor.r, baseColor.g, baseColor.b, inverseNormalizedTime);
            yield return null;
        }
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
    }
    IEnumerator FadeInUIElement(MaskableGraphic im, float duration)
    {
        float timer = 0;
        float NormalizedTime = 0;
        Color baseColor = im.color;
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            NormalizedTime = timer / duration;
            im.color = new Color(baseColor.r, baseColor.g, baseColor.b, NormalizedTime);
            yield return null;
        }
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
    }
}
