using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FollowMouseWithFocus_V2 : MonoBehaviour
{
     public bool isCurrentlyFocusing { get; private set; }
     public GameObject CurrentlyFocusedEnemy { get; private set; }
    public Vector2 SwordDirection { get; private set; }

    [SerializeField] Player_References playerRefs;

    [Header("Focus Stats")]
    [SerializeField] float followingSpeed_Mouse = 8f;
    [SerializeField] float followingSpeed_Controller = 8f;
    [SerializeField] float JoystickRegularFocusAttempt_Radius;
    [SerializeField] float OnFocusedEnemyDied_Radius;
    [SerializeField] float JoystickJoystickRefocus_Radius;
    [SerializeField] float MouseRegularFocus_Radius;

    [Header("Scriptable Object Variables")]
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] BoolVariable isDistanceToMouse;
    [SerializeField] TransformVariable ClosestEnemy;

    Vector2 lastValidDirection; //This is for joystick, so the mouse looks at last looked direction
    InputDetector inputDetector;
    List<GameObject> spawnedEnemies = new List<GameObject>();
    bool attemptedJoystickRefocus;
    CameraZoomController zoomController;

    //The Update is constantly checking if we are focusing or not and looking at near stuff
    //At any moment we can call FocusNewEnemy or UnfocusCurrentEnemy and the Update will act accordingly
    //We have the method AttemptFocus which looks around a circle area and focuses the nearest enemy to the center.
    //We call this method from diferent functions depending on context and adapt the circle position and radius accordingly 
    //For now AttemptFocus is private, but it could perfectly become public if we need to call it from somewhere else, just as we do with FocusEnemy and UnfocusCurrentEnemy

    private void Awake()
    {
        zoomController = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>(); //MAL FEO FATAL potser hauria de ser un signleton
        inputDetector = InputDetector.Instance;
    }

    private void Update()
    {
        Vector2 PosToLook = Vector2.zero;

        if (isCurrentlyFocusing) //Look at enemy if focusing enemy
        {
            if(CurrentlyFocusedEnemy != null)
            {
                PosToLook = CurrentlyFocusedEnemy.transform.position;
            }
            else { Debug.LogError("Focusing null enemy wtf"); }
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
        void ChangeFocusWithJoystick() //NEEDS TESTING
        {
            if (inputDetector.isControllerDetected && inputDetector.LookingDirectionInput.sqrMagnitude > .8f && isCurrentlyFocusing  )
            {
                if (!attemptedJoystickRefocus)
                {
                    GameObject oldEnemy = CurrentlyFocusedEnemy;

                    Vector2 center = (Vector2)CurrentlyFocusedEnemy.transform.position + (inputDetector.LookingDirectionInput.normalized * JoystickJoystickRefocus_Radius);

                    GameObject newEnemy = GetClosestEnemyToCircle(center, JoystickJoystickRefocus_Radius, false);
                    if(newEnemy != null && newEnemy != oldEnemy) 
                    {
                        if (UsefullMethods.IsOutsideCameraView(newEnemy.transform.position, Camera.main)
                        {
                            return;
                        }
                        FocusNewEnemy(newEnemy);
                    }
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
        Debug.Log("Focused " + enemy.name);

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
    GameObject GetClosestEnemyToCircle(Vector2 circleCenter, float radius, bool ignoreCurrent) //work in pr
    {
        spawnedEnemies.Clear();
        spawnedEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        GameObject lastFocusedEnemy = CurrentlyFocusedEnemy; //Unfocus current enemy but keep a reference
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
        if (InrangeEnemies.Count == 0)
        {
            return null;
        }

        int minIndex = 0;
        for (int o = 0; o < InrangeDistances.Count; o++)
        {
            if (InrangeDistances[o] < InrangeDistances[minIndex])
            {
                minIndex = o;
            }
        }

        return InrangeEnemies[minIndex];

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
        GameObject newEnemy = GetClosestEnemyToCircle(info.DeadGameObject.transform.position,
            diedRadius,
            true);
        if (newEnemy != null) { FocusNewEnemy(newEnemy); }
    }
    void OnPlayerDied(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        UnfocusCurrentEnemy();
    }
    void OnFocusInputPressed()
    {
        if(inputDetector.isControllerDetected)
        {
            if (isCurrentlyFocusing) { UnfocusCurrentEnemy(); }
            else
            {
                GameObject newEnemy = GetClosestEnemyToCircle(Camera.main.transform.position, JoystickRegularFocusAttempt_Radius, true);
                if(newEnemy != null) { FocusNewEnemy(newEnemy); }
            }
        }
        else
        {
            GameObject newEnemy = GetClosestEnemyToCircle(MouseCameraTarget.Instance.transform.position, MouseRegularFocus_Radius, false);
            if (newEnemy != null) { FocusNewEnemy(newEnemy); }
        }
    }
    void OnAttackedEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        EnemyStats currentEnemyStats = info.AttackedRoot.GetComponent<Enemy_References>().currentEnemyStats;
        if ( currentEnemyStats!= null && currentEnemyStats.CurrentHp <= 0) { return; }

        if(info.AttackedRoot.CompareTag(TagsCollection.Enemy))
        {
            FocusNewEnemy(info.AttackedRoot);
        }
    }
}
