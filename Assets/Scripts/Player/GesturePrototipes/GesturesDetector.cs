using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
public struct ArcData
{
    public Vector2 arcDirection;
    public bool isClockwise;
    public float angle;
    public int startPos; //this should be the oldest input valid. Index is smallest
    public int endPos; //this should be the latest player input. index is largest
}
public struct TapData
{
    public Vector2 endDirection;
    public Vector2 startPosition;
    public float tapLenght;
}
public class GesturesDetector : MonoBehaviour
{
    public Action<ArcData> OnArcDetected;
    public Action<TapData> OnTapDetected;

    const float secondsOfInputStored = 1f;
    [Serializable]
    public struct InputValue
    {
        public Vector2 Input;
        public float time;
        public float magnitude;
        public bool isMovingInput; 
    }
    List<InputValue> InputsList = new List<InputValue>();
    private void FixedUpdate()
    {
        FillAndDrawInput();
        if(!isDelayingArc)
        {
            if (CheckForArc(out ArcData arcData))
            {
                DrawArc(arcData, 1);
                
                if (arcData.isClockwise) { Debug.Log($"Clockwise arc detected with{arcData.angle} degrees"); }
                else { Debug.Log($"Not clockwise arc detected with{arcData.angle} degrees"); }

                StartCoroutine(ArcDelay());
                OnArcDetected?.Invoke(arcData);
                return;
            }
        }
        
        if(!isDelayingTapCheck)
        {
            if (CheckForTap(out TapData tapData))
            {
                DrawTap(tapData, 1);
                Debug.Log("Tap detected");

                StartCoroutine(TapDelay());
                OnTapDetected?.Invoke(tapData);
            }
        }
        
    }
    #region GET INPUTS
    [SerializeField] float DotThreshold = 0.95f;
    void FillAndDrawInput()
    {
        Vector2 thisFrameInput = Input.GetAxisRaw("RightJoystickHorizontal") * Vector2.right + Input.GetAxisRaw("RightJoystickVertical") * Vector2.up; //I dont like this, its not normalized input
        if(thisFrameInput.sqrMagnitude > 1) { thisFrameInput.Normalize(); }

        InputValue inputValue = new InputValue();
        inputValue.Input = thisFrameInput;
        inputValue.time = Time.time;
        inputValue.magnitude = thisFrameInput.magnitude;

        if(InputsList.Count > 2)
        {
            float dotBetweenInputs = Vector2.Dot(thisFrameInput, InputsList[InputsList.Count - 1].Input.normalized);
            inputValue.isMovingInput = dotBetweenInputs < DotThreshold;
        }

        InputsList.Add(inputValue);

        ColorUtility.TryParseHtmlString("#827f7f", out Color shortColor);
        ColorUtility.TryParseHtmlString("#ff3c3c", out Color movingColor);
        ColorUtility.TryParseHtmlString("#ffc1c1", out Color notMovingColor);


        for (int i = InputsList.Count - 1; i >= 0; i--)
        {
            if (happenedBeforeSeconds(InputsList[i], secondsOfInputStored))
            {
                InputsList.RemoveAt(i);
            }
        }
        for (int i = InputsList.Count -1; i >= 0; i--)
        {
            if (i == InputsList.Count - 1) { Debug.DrawLine(transform.position, (Vector2)transform.position + InputsList[i].Input, Color.green); }
            if(i == 0) { return; }

            Color DotColor = InputsList[i].isMovingInput ? movingColor : notMovingColor;
            if (InputsList[i].magnitude < 0.8f) { DotColor = shortColor; }
            //Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + InputsList[i].Input, DotColor);
            Debug.DrawLine((Vector2)transform.position + InputsList[i].Input, (Vector2)transform.position + InputsList[i-1].Input, DotColor);
        }
    }
    #endregion
    #region ARC CHECK
   
    const int minInputsToCheckForArc = 10; //if there are not this many inputs stored in the list, do not check. This is for the beggining of play
    const float minAngleDegForArc = 45f;
    const float secondsToCheckForArc = 0.5f; //check values that happenend this many seconds before
    const float delayAfterArc = 0.5f; //after a true arc, delay the checks so we have new values
    const float thresholdForArc = 0.5f; //If an input vector below this magnitude is found, stop the check
    bool isDelayingArc;
    public bool CheckForArc(out ArcData arcData)
    {
        arcData = new ArcData();

        if (InputsList.Count < minInputsToCheckForArc) { return false; } //if we dont have enough inputs, return false

        for (int i = InputsList.Count -1; i > 0; i--) //Go from the newest input to the oldest
        {
            InputValue newInputValue = InputsList[i];
            if (happenedBeforeSeconds(newInputValue, secondsToCheckForArc)) { break; }

            if (!InputsList[i].isMovingInput || InputsList[i].magnitude < 0.9f) //once we found a starting point, try to find an end point
            {
                if(i == 0 || !InputsList[i - 1].isMovingInput) { continue; } //if its the last input or the one before is not arquing, continue

                
                for (int j = i - 1; j >= 0; j--) 
                {
                    InputValue oldInputValue = InputsList[j];
                    
                    if (!oldInputValue.isMovingInput || oldInputValue.magnitude < 0.9f)
                    {
                        arcData.startPos = j +1;
                        arcData.endPos = i-1;
                        goto ArcFinished;
                    }
                }
            }
        }
        return false;

    ArcFinished:
        
        float angleBetweenValues = UsefullMethods.AngleBetweenDirectionsDeg(InputsList[arcData.startPos].Input.normalized, InputsList[arcData.endPos].Input.normalized);
        if (Mathf.Abs(angleBetweenValues) >= minAngleDegForArc)
        {
            arcData.isClockwise = angleBetweenValues < 0;
            arcData.arcDirection = InputsList[InputsList.Count - 2].Input.normalized; //this needs a better calculation, check for medians somehow doesnt work

            arcData.angle = angleBetweenValues;
            return true;
        }
        return false;
    } 
    bool isValidEndArcPoint(int index)
    {

        if (InputsList[index].Input.sqrMagnitude < thresholdForArc) { return true; }
        return false;
        //finish this after the prototype
        float dotBetweenInputs = Vector2.Dot(InputsList[index].Input.normalized, InputsList[index - 1].Input.normalized);
        if (dotBetweenInputs > 0.98f) { return true; } 
        return false;
    }
    Vector2 GetMedianVectorOfInputs(int lastInputIndex)
    {
        Vector2 medianVector = Vector2.zero;
        for (int i = InputsList.Count -1; i >= lastInputIndex; i--)
        {
            medianVector += InputsList[i].Input;
        }
        return medianVector.normalized;
    }
    void DrawArc(ArcData arcData, float duration)
    {
        Color startColor = Color.cyan;
        Color endColor = Color.blue;
        for (int i = arcData.endPos; i >= arcData.startPos; i--)
        {
            Vector2 thisLine = InputsList[i].Input;
            float normalizedValue = (i - arcData.startPos) / (float)(arcData.endPos - arcData.startPos);
            Color thisColor = Color.Lerp(startColor, endColor, normalizedValue);
            Debug.DrawLine(transform.position, (Vector2)transform.position + thisLine, thisColor, duration);
        }
    }
    IEnumerator ArcDelay()
    {
        isDelayingArc = true;
        yield return new WaitForSeconds(delayAfterArc);
        isDelayingArc = false;
    }

    #endregion
    #region TAP CHECK
    const float thresholdForTap = 0.5f; //The active input value needs to be above this magnitude to be considered a tap
    const float secondsToCheckForTaps = 0.1f; //Check inputs that happened this many seconds before this frame
    const float delayAfterTap = 0.2f; //After an active Tap, delay so we have new values
    bool isDelayingTapCheck;
    public bool CheckForTap(out TapData tapData)
    {
        tapData = new TapData();
        if (InputsList.Count < 4) { return false; }

        if (!isBelowThreshold(InputsList[InputsList.Count - 1],thresholdForTap)) { return false; } //if last input is active, return false

        for (int i = InputsList.Count -1; i > 0; i--) //go thorw all the inputs to check for a positive input, if we do, check for a negative input right after
        {
            InputValue inputValue = InputsList[i];
            if (happenedBeforeSeconds(inputValue,secondsToCheckForTaps)) { return false; } //if the input is too old, break

            if (!isBelowThreshold(inputValue, thresholdForTap))
            {
                for (int j = i; j > 0; j--)
                {
                    if (isBelowThreshold(InputsList[j], thresholdForTap)) 
                    {
                        Vector2 startVector = InputsList[i-1].Input;
                        Vector2 endVector = InputsList[j].Input;
                        tapData.endDirection = endVector.normalized;
                        return true; 

                        float dotBetweenVectors = Vector2.Dot(startVector.normalized, endVector.normalized);
                        if(Mathf.Abs(dotBetweenVectors) < 0.9f)
                        {
                            tapData.endDirection = endVector.normalized;
                            tapData.startPosition = startVector;
                            tapData.tapLenght = (startVector - endVector).magnitude;
                            return true;
                        }
                        
                    }
                }
            }
        }
        return false;
    }
    IEnumerator TapDelay()
    {
        isDelayingTapCheck = true;
        yield return new WaitForSeconds(delayAfterTap);
        isDelayingTapCheck = false;
    }
    void DrawTap(TapData tapData, float duration)
    {
        Debug.DrawLine((Vector2)transform.position + tapData.startPosition, (Vector2)transform.position + tapData.endDirection, Color.yellow, duration);
    }
    bool isBelowThreshold(InputValue inputValue, float threshold)
    {
        return inputValue.Input.sqrMagnitude < threshold;
    }
    bool happenedBeforeSeconds(InputValue inputValue, float secondsBefore)
    {
        return inputValue.time < (Time.time - secondsBefore);
    }
#endregion
}
