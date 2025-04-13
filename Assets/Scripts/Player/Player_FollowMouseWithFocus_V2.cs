using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_FollowMouseWithFocus_V2 : MonoBehaviour
{
    public bool isCurrentlyFocusing{ get; private set; }
     public Focuseable CurrentFocuseable { get; private set; }
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
    List<Focuseable> spawnedFocuseables = new List<Focuseable>();
    bool attemptedJoystickRefocus;
    CameraZoomController zoomController;

    //The Update is constantly checking if we are focusing or not and looking at near stuff
    //At any moment we can call FocusNewEnemy or UnfocusCurrentEnemy and the Update will act accordingly
    //We have the method AttemptFocus which looks around a circle area and focuses the nearest enemy to the center.
    //We call this method from diferent functions depending on context and adapt the circle position and radius accordingly 
    //For now AttemptFocus is private, but it could perfectly become public if we need to call it from somewhere else, just as we do with FocusEnemy and UnfocusCurrentEnemy

    private void Awake()
    {
        zoomController = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>(); //MAL FEO FATAL potser hauria de ser un signleton
        inputDetector = InputDetector.Instance;
    }

    private void Update()
    {
        Vector2 PosToLook = Vector2.zero;

        if (isCurrentlyFocusing) //Look at enemy if focusing enemy
        {
            if(CurrentFocuseable != null)
            {
                PosToLook = CurrentFocuseable.transform.position;
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
                    Focuseable oldEnemy = CurrentFocuseable;

                    Vector2 center = (Vector2)CurrentFocuseable.transform.position + (inputDetector.LookingDirectionInput.normalized * JoystickJoystickRefocus_Radius);

                    Focuseable newEnemy = GetClosestFocuseableToCircle(center, JoystickJoystickRefocus_Radius, false);
                    if(newEnemy != null && newEnemy != oldEnemy) 
                    {
                        if (UsefullMethods.IsOutsideCameraView(newEnemy.transform.position, Camera.main))
                        {
                            return;
                        }
                        FocusNewEnemy(newEnemy);
                    }
                    else
                    {
                        FocusNewEnemy(oldEnemy);
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
    Generic_StateMachine focusedEnemy_StateMachine;
    public void FocusNewEnemy(Focuseable newFocuseable)
    {
        UnfocusCurrentEnemy();

        isCurrentlyFocusing = true;
        CurrentFocuseable = newFocuseable;
        SubscribeToEnemy(newFocuseable);
        TargetGroupSingleton.Instance.AddTarget(newFocuseable.transform, 3, 2);
        zoomController.onFocusedEnemy();

        //
        void SubscribeToEnemy(Focuseable newICon)
        {
            newICon.ShowFocusIcon();

            //IF they have a statemachine, subcribe to unfocus when killed. Should it be a IKillabble? Maybe, but I dont want to implement it into the TiedEnemy
            focusedEnemy_StateMachine = newICon.RootGameObject.GetComponent<Generic_StateMachine>();
            if(focusedEnemy_StateMachine != null)
            {
                focusedEnemy_StateMachine.OnStateChanged += OnFocusedEnemyChangeState;
            }
        }
    }
   
    public void UnfocusCurrentEnemy()
    {
        if (!isCurrentlyFocusing) { return; }

        isCurrentlyFocusing = false;
        if(CurrentFocuseable != null) 
        { 
            UnsubscribeToEnemy(CurrentFocuseable);
            TargetGroupSingleton.Instance.RemoveTarget(CurrentFocuseable.transform);
        }
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        zoomController.onUnfocusedEnemy();
        CurrentFocuseable = null;

        //
        void UnsubscribeToEnemy(Focuseable oldIcon)
        {
            oldIcon.HideFocusIcon();
            if (focusedEnemy_StateMachine != null) { focusedEnemy_StateMachine.OnStateChanged -= OnFocusedEnemyChangeState; }
        }
    }
    private void OnEnable()
    {
        inputDetector.OnFocusPressed += OnFocusInputPressed;
    }
    private void OnDisable()
    {
        inputDetector.OnFocusPressed -= OnFocusInputPressed;
    }
    Focuseable GetClosestFocuseableToCircle(Vector2 circleCenter, float radius, bool ignoreCurrent)
    {
        spawnedFocuseables.Clear();
        GameObject[] allFocuseables = GameObject.FindGameObjectsWithTag(Tags.FocuseableObject);
        foreach(GameObject focuseable in allFocuseables)
        {
            Focuseable thisFocus = focuseable.GetComponent<Focuseable>();
            if (thisFocus != null) { spawnedFocuseables.Add(thisFocus); }
        }

        //Unfocus current enemy but keep a reference
        Focuseable lastFocusedEnemy = null;
        if (CurrentFocuseable != null) { lastFocusedEnemy = CurrentFocuseable; }
        UnfocusCurrentEnemy();

        List<Focuseable> InrangeEnemies = new List<Focuseable>();
        List<float> InrangeDistances = new List<float>();

        StartCoroutine(DrawAttemptDebug(circleCenter, radius, 2f));

        //Add to a list every enemy within range and its distance
        for (int i = 0; i < spawnedFocuseables.Count; i++)
        {
            if (spawnedFocuseables[i] == lastFocusedEnemy)
            {
                if (ignoreCurrent) { continue; }
            }
            if (Vector2.Distance(circleCenter, spawnedFocuseables[i].transform.position) < radius)
            {
                InrangeEnemies.Add(spawnedFocuseables[i]);
                InrangeDistances.Add(Vector2.Distance(circleCenter, spawnedFocuseables[i].transform.position));
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
    void OnFocusedEnemyChangeState(State newState)
    {
        if (newState.stateTag == StateTags.Dead)
        {
            focusedEnemy_StateMachine.OnStateChanged -= OnFocusedEnemyChangeState;
            const float diedRadius = 5;
            Focuseable newEnemy = GetClosestFocuseableToCircle(newState.gameObject.transform.position,
                diedRadius,
                true);
            if (newEnemy != null) { FocusNewEnemy(newEnemy); }
            else { UnfocusCurrentEnemy(); }
        }
    }
    void OnFocusInputPressed()
    {
        if(inputDetector.isControllerDetected)
        {
            if (isCurrentlyFocusing) { UnfocusCurrentEnemy(); }
            else
            {
                Focuseable newEnemy = GetClosestFocuseableToCircle(Camera.main.transform.position, JoystickRegularFocusAttempt_Radius, false);
                if(newEnemy != null) { FocusNewEnemy(newEnemy); }
            }
        }
        else
        {
            Focuseable newEnemy = GetClosestFocuseableToCircle(MouseCameraTarget.Instance.transform.position, MouseRegularFocus_Radius, true);
            if (newEnemy != null) { FocusNewEnemy(newEnemy); }
        }
    }
    public void AttemptFocusAttackedEnemy(DealtDamageInfo info)
    {
        Focuseable maybeIcon = info.AttackedRoot.GetComponentInChildren<Focuseable>();
        if (maybeIcon != null)
        {
            

            IHealth thisHealth = info.AttackedRoot.GetComponent<IHealth>();
            if (thisHealth != null && thisHealth.GetCurrentHealth() <= 0)
            {
               
            }
            else { FocusNewEnemy(maybeIcon); }
                
        }
    }
}
