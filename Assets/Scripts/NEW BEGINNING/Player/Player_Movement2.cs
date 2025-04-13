using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedsEnum
{
    Regular,
    Fast,
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
    public void SetMovementSpeed(SpeedsEnum speedType)
    {
        switch(speedType)
        {
            case SpeedsEnum.Regular:
                currentStats.Speed = currentStats.BaseSpeed;
                break;
            case SpeedsEnum.Fast:
                currentStats.Speed = currentStats.BaseSpeed * 2f;
                break;
            case SpeedsEnum.Slow:
                currentStats.Speed = currentStats.BaseSpeed * 0.5f;
                break;
            case SpeedsEnum.VerySlow:
                currentStats.Speed = currentStats.BaseSpeed * 0.25f;
                break;
            case SpeedsEnum.Stopped:
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
