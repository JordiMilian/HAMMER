using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_FollowMouseWithFocus_V2 : MonoBehaviour
{
    public bool isCurrentlyFocusing{ get; private set; }
     public FocusIcon CurrentlyFocusedIcon { get; private set; }
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
    List<FocusIcon> spawnedFocusIcons = new List<FocusIcon>();
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
            if(CurrentlyFocusedIcon != null)
            {
                PosToLook = CurrentlyFocusedIcon.transform.position;
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
                    FocusIcon oldEnemy = CurrentlyFocusedIcon;

                    Vector2 center = (Vector2)CurrentlyFocusedIcon.transform.position + (inputDetector.LookingDirectionInput.normalized * JoystickJoystickRefocus_Radius);

                    FocusIcon newEnemy = GetClosestEnemyToCircle(center, JoystickJoystickRefocus_Radius, false);
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
    public void FocusNewEnemy(FocusIcon newIcon)
    {
        UnfocusCurrentEnemy();

        isCurrentlyFocusing = true;
        CurrentlyFocusedIcon = newIcon;
        SubscribeToEnemy(newIcon);
        TargetGroupSingleton.Instance.AddTarget(newIcon.transform, 3, 2);
        zoomController.onFocusedEnemy();
        Debug.Log("Focused " + newIcon.name);

        //
        void SubscribeToEnemy(FocusIcon newICon)
        {
            focusedEnemy_StateMachine = newICon.RootGameObject.GetComponent<Generic_StateMachine>();

            newICon.ShowFocusIcon();
            focusedEnemy_StateMachine.OnStateChanged += OnFocusedEnemyChangeState;
        }
    }
   
    public void UnfocusCurrentEnemy()
    {
        if (!isCurrentlyFocusing) { return; }

        isCurrentlyFocusing = false;
        if(CurrentlyFocusedIcon != null) 
        { 
            UnsubscribeToEnemy(CurrentlyFocusedIcon);
            TargetGroupSingleton.Instance.RemoveTarget(CurrentlyFocusedIcon.transform);
        }
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        zoomController.onUnfocusedEnemy();
        CurrentlyFocusedIcon = null;

        //
        void UnsubscribeToEnemy(FocusIcon oldIcon)
        {
            oldIcon.HideFocusIcon();
            focusedEnemy_StateMachine.OnStateChanged -= OnFocusedEnemyChangeState;
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
    FocusIcon GetClosestEnemyToCircle(Vector2 circleCenter, float radius, bool ignoreCurrent)
    {
        spawnedFocusIcons.Clear();
        GameObject[] allFocuseables = GameObject.FindGameObjectsWithTag(Tags.FocuseableObject);
        foreach(GameObject focuseable in  allFocuseables)
        {
            FocusIcon thisFocus = focuseable.GetComponent<FocusIcon>();
            if (thisFocus != null) { spawnedFocusIcons.Add(thisFocus); }
        }

        //Unfocus current enemy but keep a reference
        FocusIcon lastFocusedEnemy = null;
        if (CurrentlyFocusedIcon != null) { lastFocusedEnemy = CurrentlyFocusedIcon; }
        UnfocusCurrentEnemy();

        List<FocusIcon> InrangeEnemies = new List<FocusIcon>();
        List<float> InrangeDistances = new List<float>();

        StartCoroutine(DrawAttemptDebug(circleCenter, radius, 2f));

        //Add to a list every enemy within range and its distance
        for (int i = 0; i < spawnedFocusIcons.Count; i++)
        {
            if (spawnedFocusIcons[i] == lastFocusedEnemy)
            {
                if (ignoreCurrent) { continue; }
            }
            if (Vector2.Distance(circleCenter, spawnedFocusIcons[i].transform.position) < radius)
            {
                InrangeEnemies.Add(spawnedFocusIcons[i]);
                InrangeDistances.Add(Vector2.Distance(circleCenter, spawnedFocusIcons[i].transform.position));
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
            const float diedRadius = 5;
            FocusIcon newEnemy = GetClosestEnemyToCircle(newState.gameObject.transform.position,
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
                FocusIcon newEnemy = GetClosestEnemyToCircle(Camera.main.transform.position, JoystickRegularFocusAttempt_Radius, true);
                if(newEnemy != null) { FocusNewEnemy(newEnemy); }
            }
        }
        else
        {
            FocusIcon newEnemy = GetClosestEnemyToCircle(MouseCameraTarget.Instance.transform.position, MouseRegularFocus_Radius, false);
            if (newEnemy != null) { FocusNewEnemy(newEnemy); }
        }
    }
    public void AttemptFocusAttackedEnemy(DealtDamageInfo info)
    {
        FocusIcon maybeIcon = info.AttackedRoot.GetComponentInChildren<FocusIcon>();
        if (maybeIcon != null)
        {
            playerRefs.followMouse.FocusNewEnemy(maybeIcon);

            IHealth thisHealth = info.AttackedRoot.GetComponent<IHealth>();
            if (thisHealth != null && thisHealth.GetCurrentHealth() <= 0)
            {
                UnfocusCurrentEnemy();
            }
        }
    }
}
