using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementSpeeds
{
    Regular,
    Running,
    Slow,
    VerySlow,
    Stopped
}
public class Player_Movement2 : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    PlayerStats currentStats;
    private void Awake()
    {
        currentStats = playerRefs.currentStats;
    }
    public void SetMovementSpeed(MovementSpeeds speedType)
    {
        switch(speedType)
        {
            case MovementSpeeds.Regular:
                currentStats.Speed = currentStats.BaseSpeed;
                break;
            case MovementSpeeds.Running:
                currentStats.Speed = currentStats.BaseSpeed * 1.5f;
                break;
            case MovementSpeeds.Slow:
                currentStats.Speed = currentStats.BaseSpeed * 0.5f;
                break;
            case MovementSpeeds.VerySlow:
                currentStats.Speed = currentStats.BaseSpeed * 0.25f;
                break;
            case MovementSpeeds.Stopped:
                currentStats.Speed = 0;
                break;
        }
    }

    private void Update()
    {
        Vector2 inputDirection = InputDetector.Instance.MovementDirectionInput;
        playerRefs.characterMover.MovementVectorsPerSecond.Add(inputDirection.normalized * currentStats.Speed);
    }
}
