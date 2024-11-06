using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FollowMouseWithFocus_V2 : MonoBehaviour
{
    public bool isCurrentlyFocusing { get; private set; }
    public GameObject CurrentlyFocusedEnemy { get; private set; }
    public Vector2 SwordDirection { get; private set; } //This is so it can be accessed from other scripts 
    [SerializeField] Player_References playerRefs;

    [Header("Focus Stats")]
    [SerializeField] float followingSpeed_Mouse = 8f;
    [SerializeField] float followingSpeed_Controller = 8f;
    [SerializeField] float JoystickRegularFocusAttempt_Radius;
    [SerializeField] float OnFocusedEnemyDied_Radius;
    [SerializeField] float JoystickJoystickRefocus_Radius;

    [Header("Scriptable Object Variables")]
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] BoolVariable isDistanceToMouse;
    [SerializeField] TransformVariable ClosestEnemy;

    Vector2 lastValidDirection; //This is for joystick, so the mouse looks at last looked direction
    InputDetector inputDetector;
    List<GameObject> spawnedEnemies = new List<GameObject>();
    bool attemptedJoystickRefocus;
    CameraZoomController zoomController;

    private void Awake()
    {
        zoomController = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>(); //MAL FEO FATAL potser hauria de ser un signleton
        inputDetector = InputDetector.Instance;
    }
    private void OnEnable()
    {
        inputDetector.OnFocusPressed += OnFocusInputPressed;
        playerRefs.events.OnDeath += OnPlayerDied;
        playerRefs.events.OnDealtDamage += OnAttackedEnemy;
    }
    private void OnDisable()
    {
        inputDetector.OnFocusPressed -= OnFocusInputPressed;
        playerRefs.events.OnDeath -= OnPlayerDied;
        playerRefs.events.OnDealtDamage -= OnAttackedEnemy;
    }
    private void Update()
    {
        Vector2 PosToLook = Vector2.zero;

        if (isCurrentlyFocusing) //Look at enemy if focusing enemy
        {
            PosToLook = CurrentlyFocusedEnemy.transform.position;
        }
        else if (distanceToEnemy.Value < 2.5f && !isDistanceToMouse.Value) //Look at closest enemy if everything is alright
        {
            if (ClosestEnemy.Tf != null) { PosToLook = ClosestEnemy.Tf.position; }
        } 
        else //Or look at input direction
        {
            if (inputDetector.LookingDirectionInput.sqrMagnitude < 0.5f) //if not enough input from joystick, look at last valid
            {
                PosToLook = inputDetector.PlayerPos + lastValidDirection;
            }
            else //just look at input direction (mouse always has enought)
            {
                PosToLook = inputDetector.PlayerPos + inputDetector.LookingDirectionInput; ;
                lastValidDirection = inputDetector.LookingDirectionInput;
            }
        }
        LookingAtTarget(PosToLook);

        ChangeFocusWithJoystick();


        //
        void LookingAtTarget(Vector2 targetPos)
        {
            Vector2 playerPos = transform.position;
            SwordDirection = (targetPos - playerPos).normalized;

            //Change rotation speed depending on controller or kboard
            float actualSpeed = followingSpeed_Controller;
            if (!InputDetector.Instance.isControllerDetected) { actualSpeed = followingSpeed_Mouse; }
            transform.up = Vector3.RotateTowards(transform.up, SwordDirection, actualSpeed * Time.deltaTime, 10f);

            playerRefs.spriteFliper.FocusVector = targetPos;
        }
        void ChangeFocusWithJoystick()
        {
            if (inputDetector.isControllerDetected && inputDetector.LookingDirectionInput.sqrMagnitude > .8f && isCurrentlyFocusing  )
            {
                if (!attemptedJoystickRefocus)
                {
                    Vector2 center = (Vector2)CurrentlyFocusedEnemy.transform.position + (inputDetector.LookingDirectionInput.normalized * JoystickJoystickRefocus_Radius);
                    AttemptFocus(center, JoystickJoystickRefocus_Radius, false, false);
                    attemptedJoystickRefocus = true;
                }
            }
            else
            {
                if (attemptedJoystickRefocus) { attemptedJoystickRefocus = false; }
            }
        }
    }
    public void FocusNewEnemy(GameObject enemy)
    {
        UnfocusCurrentEnemy();

        isCurrentlyFocusing = true;
        CurrentlyFocusedEnemy = enemy;
        SubscribeToEnemy(enemy);
        TargetGroupSingleton.Instance.AddTarget(enemy.transform, 3, 2);
        zoomController.onFocusedEnemy();
        playerRefs.events.OnFocusEnemy?.Invoke(enemy); //For tutorial now

        //
        void SubscribeToEnemy(GameObject subscribed)
        {
            Enemy_EventSystem subscribedEvents = subscribed.GetComponent<Enemy_EventSystem>();
            subscribedEvents.OnFocused?.Invoke();
            subscribedEvents.OnDeath += OnFocusedEnemyDied;
        }
    }
    public void UnfocusCurrentEnemy()
    {
        if (!isCurrentlyFocusing) { return; }

        isCurrentlyFocusing = false;
        if(CurrentlyFocusedEnemy != null) 
        { 
            UnsubscribeToEnemy(CurrentlyFocusedEnemy);
            TargetGroupSingleton.Instance.RemoveTarget(CurrentlyFocusedEnemy.transform);
        }
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        zoomController.onUnfocusedEnemy();
        playerRefs.events.OnUnfocusEnemy?.Invoke(); //For tutorial now
        CurrentlyFocusedEnemy = null;

        //
        void UnsubscribeToEnemy(GameObject unsubscribed)
        {
            Enemy_EventSystem unsubscribedEvents = unsubscribed.GetComponent<Enemy_EventSystem>();
            unsubscribedEvents.OnUnfocused?.Invoke();
            unsubscribedEvents.OnDeath -= OnFocusedEnemyDied;
        }
    }
    void AttemptFocus(Vector2 circleCenter, float radius, bool canUnfocus, bool ignoreCurrent)
    {
        spawnedEnemies.Clear();
        spawnedEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        GameObject lastFocusedEnemy = CurrentlyFocusedEnemy; //Unfocus current enemy but keep a reference just in case
        UnfocusCurrentEnemy();

        List<GameObject> InrangeEnemies = new List<GameObject>();
        List<float> InrangeDistances = new List<float>();

        StartCoroutine(DrawAttemptDebug(circleCenter, radius, 2f));

        //Add to a list every enemy within range and its distance
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] == lastFocusedEnemy) 
            { 
                if (ignoreCurrent) { continue; } 
            }
            if (Vector2.Distance(circleCenter, spawnedEnemies[i].transform.position) < radius)
            {
                InrangeEnemies.Add(spawnedEnemies[i]);
                InrangeDistances.Add(Vector2.Distance(circleCenter, spawnedEnemies[i].transform.position));
            }
        }
        //If no enemies in range
        if (InrangeEnemies.Count == 0)
        {
            if (canUnfocus) { return; }
            else { FocusNewEnemy(lastFocusedEnemy); return; }
        }

        //Check which index has the shortest distance and return
        int minIndex = 0;
        for (int o = 0; o < InrangeDistances.Count; o++)
        {
            if (InrangeDistances[o] < InrangeDistances[minIndex])
            {
                minIndex = o;
            }
        }

        FocusNewEnemy(spawnedEnemies[minIndex]);

        //
        IEnumerator DrawAttemptDebug(Vector2 center, float radius, float time)
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                UsefullMethods.DrawPolygon(center, 10, radius);
                yield return null;
            }
        }
    }
    void OnFocusedEnemyDied( object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        const float diedRadius = 5;
        AttemptFocus(
            info.DeadGameObject.transform.position,
            diedRadius,
            true,
            true);
    }
    void OnPlayerDied(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        UnfocusCurrentEnemy();
    }
    void OnFocusInputPressed()
    {
        if(inputDetector.isControllerDetected)
        {
            Vector2 center = inputDetector.PlayerPos + (lastValidDirection.normalized * JoystickRegularFocusAttempt_Radius);
            AttemptFocus(center, JoystickRegularFocusAttempt_Radius, false, false);
        }
        else
        {
            AttemptFocus(MouseCameraTarget.Instance.transform.position, 3, false, false);
        }
    }
    void OnAttackedEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        if(info.AttackedEntity.CompareTag(TagsCollection.Enemy))
        {
            FocusNewEnemy(info.AttackedEntity);
        }
    }
}
