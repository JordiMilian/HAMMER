using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PinkSaw : MonoBehaviour
{
    public SpriteShapeController shapeController;
    Vector2 pos01, pos02;
    [SerializeField] Transform sawTf;
    [SerializeField] float TimeToReach;
    [SerializeField] float posOffset01;
    [SerializeField] float posOffset02;
    [SerializeField] int startingPos = 1;
    [SerializeField] bool isConstantlySawing;
    [SerializeField] float delayedStartingTime;
    [Header("References")]
    [SerializeField] Animator sawAnimator;
    [SerializeField] Generic_AreaTriggerEvents sawAreaTrigger;
    [SerializeField] SpriteMask spriteMask;
    [SerializeField] SpriteRenderer sawSprite;
    [SerializeField] RoomWithEnemiesLogic roomWithEnemiesLogic;
    int currentPos;
    private void OnEnable()
    {
        sawAreaTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        sawAreaTrigger.onAreaActive += startSawing;
        sawAreaTrigger.onAreaUnactive += stopSawing;
        currentPos = startingPos;
        if(roomWithEnemiesLogic != null) { roomWithEnemiesLogic.onRoomCompleted += DeactivateSaw; }
        hideSprites();
    }
    private void OnDisable()
    {
        sawAreaTrigger.onAreaActive -= startSawing;
        sawAreaTrigger.onAreaUnactive -= stopSawing;
        if (roomWithEnemiesLogic != null) { roomWithEnemiesLogic.onRoomCompleted -= DeactivateSaw; }
    }
    private void Start()
    {
        if (isConstantlySawing) { Invoke("startSawing", delayedStartingTime); }
    }
    void DeactivateSaw(BaseRoomWithDoorLogic logic)
    {
        sawAnimator.SetTrigger("Deactivated");
        sawAreaTrigger.onAreaActive -= startSawing;
        sawAreaTrigger.onAreaUnactive -= stopSawing;
        stopSawing();
    }
    void setPositions()
    {
        Vector2 shapePos = shapeController.gameObject.transform.position;

        pos01 =  shapeController.spline.GetPosition(0);
        pos02 =  shapeController.spline.GetPosition(1);

        Vector2 directionToPos02 = (pos02 - pos01).normalized;
        Vector2 directionToPos01 = (pos01 - pos02).normalized;
        Vector2 offsetPos01 = directionToPos02 * posOffset01;
        Vector2 offsetPos02 = directionToPos01 * posOffset02;
        pos01 = pos01 + shapePos + offsetPos01;
        pos02 = pos02 + shapePos + offsetPos02;
    }
    void startSawing()
    {
        
        setPositions();
        positionAtStart();
        sawAnimator.SetBool("isSawing", true);
        showSprites();
        
    }
    void stopSawing()
    {
        if (isConstantlySawing) { return; }
       sawAnimator.SetBool("isSawing", false);
    }
    void positionAtStart()
    {
        if (currentPos == 1)
        {
            sawTf.position = pos01;
        }
        else if (currentPos == 2)
        {
            sawTf.position = pos02;
        }
    }
    void hideSprites()
    {
        spriteMask.enabled = false;
        sawSprite.enabled = false;
    }
    void showSprites()
    {
        spriteMask.enabled = true;
        sawSprite.enabled = true;
    }
    public void EV_StartMoving()
    {
        if (currentPos == 1)
        {
            StartCoroutine(travelToPos(pos01, pos02, TimeToReach));
            currentPos = 2;
        }
        else if(currentPos == 2)
        {
            StartCoroutine(travelToPos(pos02, pos01, TimeToReach));
            currentPos = 1;
        }
    }
    public void EV_SawIsHidden()
    {
        if(!sawAnimator.GetBool("isSawing"))
        {
            hideSprites();
        }
    }
    void reachedEnd()
    {
        sawAnimator.SetTrigger("reachedEnd");
    }
    IEnumerator travelToPos(Vector2 initialPos, Vector2 finalPos, float timeToReach)
    {
        float timer = 0;
        float normalizedTime = 0;
        while(timer < timeToReach)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / timeToReach;

            Vector2 newPos = Vector2.Lerp(initialPos, finalPos, BezierBlend(normalizedTime));

            sawTf.position = newPos;
            yield return null;
        }
        reachedEnd();
    }
    float BezierBlend(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }
}
