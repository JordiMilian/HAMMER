using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackUI : MonoBehaviour
{
    [SerializeField] Transform Size01;
    [SerializeField] Color FullColor, ChargingColor;
    [SerializeField] Transform TutorialsRoot, StarRoot;
    [SerializeField] SpriteRenderer BarSprite;
    [SerializeField] Animator animator;
    [SerializeField] PlayerStats currentStats;

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        currentStats.OnCurrentBloodFlowChange += onBloodflowUpdated;
        onBloodflowUpdated(currentStats.CurrentBloodFlow);
    }
    private void OnDisable()
    {
        currentStats.OnCurrentBloodFlowChange -= onBloodflowUpdated;
    }
    void onBloodflowUpdated(float newValue)
    {
        animator.SetTrigger("Update");
        float normalizedSpAt = Mathf.InverseLerp(0, currentStats.MaxBloodFlow, currentStats.CurrentBloodFlow);
        Size01.localScale = new Vector3(normalizedSpAt, 1, 1);
        if(currentStats.CurrentBloodFlow >= currentStats.MaxBloodFlow) { OnFullyCharged(); }
        else { OnNotFullyCharged() ; }
    }
    void OnFullyCharged()
    {
        BarSprite.color = FullColor;
        TutorialsRoot.gameObject.SetActive(true);
        StarRoot.gameObject.SetActive(true);
        animator.SetBool("Star", true);
    }
    void OnNotFullyCharged()
    {
        BarSprite.color = ChargingColor;
        TutorialsRoot.gameObject.SetActive(false);
        StarRoot.gameObject.SetActive(false);
        animator.SetBool("Star", false);
    }
}
