using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SwordRotationController : MonoBehaviour
{

    //FOCUSEABLE is a Prefab
    //LOOKABLE is any  gameobject in the layer LOOKABLE with a trigger collider 

    public Transform RotatingTransform;
    public float CurrentRadiantSpeedPerSecond;
    public Vector2 LookingPosition { get; private set; } 
    public Vector2 SwordDirection { get { return RotatingTransform.up; } private set { RotatingTransform.up = value; } }

    public Focuseable CurrentFocuseable { get; private set; }
    public bool isFocusing { get; private set; }
    [SerializeField] Player_LookableDetector lookablesDetector;
    [SerializeField] Player_References playerRefs;
    CameraZoomController cameraZoomer;
    private void Start()
    {
        cameraZoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
    }
    private void Update()
    {
        //ChangeFocusWithJoystick();//Input detector should have an event to subscribe to this instead of calling it on Update
        Vector2 PosToLookThisFrame = GetThisFramePosToLook();

        RotateTowardsTarget(PosToLookThisFrame);
        LookingPosition = PosToLookThisFrame;
        //
        void RotateTowardsTarget(Vector2 targetpos)
        {
            Vector2 directionToTarget = (targetpos - (Vector2)RotatingTransform.position).normalized;
            RotatingTransform.up = Vector3.RotateTowards(RotatingTransform.up, directionToTarget, CurrentRadiantSpeedPerSecond * Time.deltaTime, 10f);
        }
    }
    private void OnEnable()
    {
        InputDetector.Instance.OnFocusPressed += OnFocusInputPressed;
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnFocusPressed -= OnFocusInputPressed;
    }
    #region SET ROTATION SPEED
    const float ControllerBaseRadiantSpeed = 8; 
    const float MouseBaseRadiantSpeed = 12; 
    public void SetRotationSpeed(SpeedsEnum speed)
    {
        switch (speed)
        {
            case SpeedsEnum.Regular:
                if (InputDetector.Instance.isControllerDetected) { CurrentRadiantSpeedPerSecond = ControllerBaseRadiantSpeed; }
                else { CurrentRadiantSpeedPerSecond = MouseBaseRadiantSpeed; }
                break;
            case SpeedsEnum.Fast:
                if (InputDetector.Instance.isControllerDetected) { CurrentRadiantSpeedPerSecond = ControllerBaseRadiantSpeed *2; }
                else { CurrentRadiantSpeedPerSecond = MouseBaseRadiantSpeed*2; }
                break;
            case SpeedsEnum.Slow:
                if (InputDetector.Instance.isControllerDetected) { CurrentRadiantSpeedPerSecond = ControllerBaseRadiantSpeed/2; }
                else { CurrentRadiantSpeedPerSecond = MouseBaseRadiantSpeed/2; }
                break;
            case SpeedsEnum.VerySlow:
                if (InputDetector.Instance.isControllerDetected) { CurrentRadiantSpeedPerSecond = ControllerBaseRadiantSpeed/4; }
                else { CurrentRadiantSpeedPerSecond = MouseBaseRadiantSpeed/4; }
                break;
            case SpeedsEnum.Stopped:
                CurrentRadiantSpeedPerSecond = 0;
                break;
        }
    Debug.Log($"Rotation speed set to {speed}");
    }
    #endregion
    #region CALCULATE WHAT TO LOOK THIS FRAME
    //This is called every frame to choose a direction to look
    Vector2 lastValidControllerDirection = Vector2.up;
    Vector2 GetThisFramePosToLook()
    { 
            //If CONTROLLER
        //IF input is long enough, look there (playerpos + input)
        //If no input and focusing, look at the focusable
        //If no input and no focusable, look at closest lookable (ignore MouseLookable)
        //if nothing, look at last valid pos 


            //If MOUSE
        //If focusing, look at focusable
        //If no focusing, look at closest lookable (mouse pos can be a lookable)
        //If nothing, look at mouse pos

        InputDetector inputDetector = InputDetector.Instance;
        //CONTROLLER
        if (inputDetector.isControllerDetected)
        {
            //Player is inputing a direction
            if(inputDetector.LookingDirectionInput.sqrMagnitude > .6f)
            {
                Vector2 ValidInputPos = inputDetector.PlayerPos + inputDetector.LookingDirectionInput;
                lastValidControllerDirection = (ValidInputPos - inputDetector.PlayerPos).normalized;
                return ValidInputPos;
            }
            //Is Focusing
            if (isFocusing)
            {
                lastValidControllerDirection = ((Vector2)CurrentFocuseable.transform.position - inputDetector.PlayerPos).normalized;
                return CurrentFocuseable.transform.position;
            }
            //There is a Lookable near (Ignore mouse lookable)
            int closestLookableIndex = lookablesDetector.GetClosestLookableIndex(true);
            if (closestLookableIndex >= 0)
            {
                Vector2 closestLookablePos = lookablesDetector.LookablesDetectedList[closestLookableIndex].Transform.position;
                lastValidControllerDirection = (closestLookablePos - inputDetector.PlayerPos).normalized;
                return closestLookablePos;
            }
            //Look at current sword direction
            return inputDetector.PlayerPos + lastValidControllerDirection;
        }
        //MOUSE
        else
        {
            //Is Focusing
            if (isFocusing) { return CurrentFocuseable.transform.position; }
            //Look at closest lookable (mouse pos can be a lookable)
            int closestLookableIndex = lookablesDetector.GetClosestLookableIndex(false);
            if(closestLookableIndex >= 0)
            {
                return lookablesDetector.LookablesDetectedList[closestLookableIndex].Transform.position;
            }
            //Look at mouse pos
            return inputDetector.MousePosition;
        }
    }
    #endregion
    #region CALCULATE ADDFORCE DISTANCE
    //This is called when performing an attack to deside the distance. Called from AnimationEvents
    public float GetAddForceDistance()
    {
        //If CONTROLLER
        //If Input, dot product that thing
        //if focusing, get pos to focus
        //if lookable near, lookable pos (ignore mouse)
        //if nothing, default

        //If MOUSE
        //If focusing, get focus pos
        //If looking at closest, get lookable pos (mouse pos can be a lookable)
        //If nothing, get mouse pos

        InputDetector inputDetector = InputDetector.Instance;
        IAddForceStats addForceStats = playerRefs.stateMachine.currentState.GetComponent<IAddForceStats>();
        if (addForceStats == null) { Debug.LogError("Missing IAddForceStats in " + playerRefs.stateMachine.currentState.name); return 1; }

        //CONTROLLER
        if (inputDetector.isControllerDetected)
        {
            //If it's giving input, make an equivalence between the movement and rotation input
            if(inputDetector.LookingDirectionInput.sqrMagnitude > .6f)
            {
                Vector2 inputDirection = inputDetector.LookingDirectionInput;
                Vector2 movingDirection = InputDetector.Instance.MovementDirectionInput;

                float DotOfSwordNMovement = Vector2.Dot(inputDirection, movingDirection); // Gives a value from -1 to 1
                float normalizedDot = Mathf.InverseLerp(-1, 1, DotOfSwordNMovement); //Gives a value from 0 to 1
                float equivalenDistance = Mathf.Lerp(addForceStats.MinOtherDistance, addForceStats.MaxOtherDistance, normalizedDot);

                return equivalenDistance - addForceStats.Offset;
            }
            
            //Focusing
            if (isFocusing)
            {
                float distanceToFocuseable = ((Vector2)CurrentFocuseable.transform.position - (Vector2)transform.position).magnitude;
                return GetEquivalentAddForceDistance(addForceStats,distanceToFocuseable);
            }
            //There is a Lookable near (Ignore mouse lookable)
            int closestLookableIndex = lookablesDetector.GetClosestLookableIndex(true);
            if (closestLookableIndex >= 0)
            {
                float distanceToClosestLookable = ((Vector2)lookablesDetector.LookablesDetectedList[closestLookableIndex].Transform.position - (Vector2)transform.position).magnitude;
                return GetEquivalentAddForceDistance(addForceStats,distanceToClosestLookable);
            }
            //No lookables near
            else
            {
                return addForceStats.DefaultOtherDistance - addForceStats.Offset;
            }
        }
        //MOUSE
        else
        {
            if (isFocusing)
            {
                float distanceToFocuseable = ((Vector2)CurrentFocuseable.transform.position - (Vector2)transform.position).magnitude;
                return GetEquivalentAddForceDistance(addForceStats, distanceToFocuseable);
            }
            //There is a Lookable near (Ignore mouse lookable)
            int closestLookableIndex = lookablesDetector.GetClosestLookableIndex(true);
            if (closestLookableIndex >= 0)
            {
                float distanceToClosestLookable = ((Vector2)lookablesDetector.LookablesDetectedList[closestLookableIndex].Transform.position - (Vector2)transform.position).magnitude;
                return GetEquivalentAddForceDistance(addForceStats, distanceToClosestLookable);
            }
            //no lookables
            else
            {
                float distanceToMouse = ((Vector2)InputDetector.Instance.MousePosition - (Vector2)transform.position).magnitude;
                return GetEquivalentAddForceDistance(addForceStats, distanceToMouse);
            }
        }
        //
        float GetEquivalentAddForceDistance(IAddForceStats stats, float PointDistance)
        {
            float distance;
            if (PointDistance < stats.MinOtherDistance)
            {
                distance = stats.MinOtherDistance;
            }
            else if (PointDistance > stats.MaxOtherDistance)
            {
                distance = stats.MaxOtherDistance;
            }
            else
            {
                distance = PointDistance;
            }
            return distance - stats.Offset;
        }
    }
    #endregion
    #region CIRCLE FOCUS METHOD
    Focuseable GetClosestFocuseableToCircle(Vector2 center, float radius, bool ignoreCurrent = false)
    {
        List<Focuseable> spawnedFocuseables = FocuseablesManager.Instance.FocusaeblesList;

        List<Focuseable> InrangeEnemies = new List<Focuseable>();
        List<float> InrangeDistances = new List<float>();

        StartCoroutine(DrawAttemptDebug(center, radius, 2f));

        //Add to a list every enemy within range and its distance
        for (int i = 0; i < spawnedFocuseables.Count; i++)
        {
            if (spawnedFocuseables[i] == CurrentFocuseable)
            {
                if (ignoreCurrent) { continue; }
            }
            float distanceToFocuseable = Vector2.Distance(center, spawnedFocuseables[i].transform.position);
            if ( distanceToFocuseable < radius)
            {
                InrangeEnemies.Add(spawnedFocuseables[i]);
                InrangeDistances.Add(distanceToFocuseable);
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
    #endregion
    #region FOCUS AND UNFOCS
    IKilleable focusedEnemy_IKilleable;
    public void FocusNewEnemy(Focuseable newFocuseable)
    {
        UnfocusCurrentEnemy();

        isFocusing = true;
        CurrentFocuseable = newFocuseable;
        newFocuseable.ShowFocusIcon();
        SubscribeToEnemy(newFocuseable);
        TargetGroupSingleton.Instance.AddTarget(newFocuseable.transform, 3, 2);
        cameraZoomer.onFocusedEnemy();

        //
        void SubscribeToEnemy(Focuseable newICon)
        {
            //If the enemy is killeable, if killed, unfocus them
            focusedEnemy_IKilleable = newICon.RootGameObject.GetComponent<IKilleable>();
            if (focusedEnemy_IKilleable != null)
            {
                focusedEnemy_IKilleable.OnKilled_event += OnFocusedEnemyKilled;
            }
        }
    }
    public void UnfocusCurrentEnemy()
    {
        if (!isFocusing) { return; }

        isFocusing = false;
        if (CurrentFocuseable != null)
        {
            UnsubscribeToEnemy(CurrentFocuseable);
            CurrentFocuseable.HideFocusIcon();
            TargetGroupSingleton.Instance.RemoveTarget(CurrentFocuseable.transform);
        }
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        cameraZoomer.onUnfocusedEnemy();
        CurrentFocuseable = null;

        //
        void UnsubscribeToEnemy(Focuseable oldFocuseable)
        {
            if (focusedEnemy_IKilleable != null) { focusedEnemy_IKilleable.OnKilled_event -= OnFocusedEnemyKilled; }
        }
    }
    //Unfocus Current enemy
    #endregion
    #region METHODS TO CALL FOCUS
    //On pressed focus
    const float JoystickRegularFocusAttempt_Radius = 7;
    const float MouseRegularFocus_Radius = 3;
    void OnFocusInputPressed()
    {
        if (InputDetector.Instance.isControllerDetected)
        {
            if (isFocusing) { UnfocusCurrentEnemy(); }
            else
            {
                Focuseable newEnemy = GetClosestFocuseableToCircle(Camera.main.transform.position, JoystickRegularFocusAttempt_Radius, false);
                if (newEnemy != null) { FocusNewEnemy(newEnemy); }
            }
        }
        else
        {
            Focuseable oldFocus = CurrentFocuseable;
            Focuseable newFocus = GetClosestFocuseableToCircle(MouseCameraTarget.Instance.transform.position, MouseRegularFocus_Radius, false);
            if (newFocus == oldFocus) { UnfocusCurrentEnemy(); }
            else if(newFocus != null)
            {
                FocusNewEnemy(newFocus);
            }
            else { UnfocusCurrentEnemy(); }
        }
    }
    //DarkSould focus
    bool attemptedJoystickRefocus;
    const float JoystickJoystickRefocus_Radius = 7;
    void ChangeFocusWithJoystick() //NEEDS TESTING
    {
        InputDetector inputDetector = InputDetector.Instance;
        if (inputDetector.isControllerDetected && inputDetector.LookingDirectionInput.sqrMagnitude > .8f && isFocusing)
        {
            if (!attemptedJoystickRefocus)
            {
                Focuseable oldEnemy = CurrentFocuseable;

                Vector2 center = (Vector2)oldEnemy.transform.position + (inputDetector.LookingDirectionInput.normalized * JoystickJoystickRefocus_Radius);

                Focuseable newEnemy = GetClosestFocuseableToCircle(center, JoystickJoystickRefocus_Radius, true);
                if (newEnemy != null)
                {
                    if (UsefullMethods.IsOutsideCameraView(newEnemy.transform.position, Camera.main))
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
    //Attacked enemy
    public void AttemptFocusAttackedEnemy(DealtDamageInfo info)
    {
        
        Focuseable maybeIcon = info.AttackedRoot.GetComponentInChildren<Focuseable>();
        if (maybeIcon != null)
        {
            //If I dont kill the enemy with this blow, focus it. It would be great to have this info inside DealtDamageInfo
            IHealth thisHealth = info.AttackedRoot.GetComponent<IHealth>();
            if (thisHealth != null && thisHealth.GetCurrentHealth() <= 0) { }
            else { FocusNewEnemy(maybeIcon); }

        }
        
    }

    //FOcuse enemy killed
    void OnFocusedEnemyKilled(DeadCharacterInfo info)
    {
        focusedEnemy_IKilleable.OnKilled_event -= OnFocusedEnemyKilled;

        Focuseable focuseableNearKilled = GetClosestFocuseableToCircle(info.DeadGameObject.transform.position, 4, true);
        if (focuseableNearKilled != null) { FocusNewEnemy(focuseableNearKilled); }
        else { UnfocusCurrentEnemy(); }
    }
    #endregion
}
