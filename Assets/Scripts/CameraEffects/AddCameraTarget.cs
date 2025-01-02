using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCameraTarget : MonoBehaviour
{
   [SerializeField] Generic_OnTriggerEnterEvents _onTriggerEnterEvents;
    [SerializeField] Transform Target;
    [SerializeField] float Weight;
    [SerializeField] float Radius;

    private void OnEnable()
    {
        _onTriggerEnterEvents.AddActivatorTag(Tags.Player_SinglePointCollider);
        _onTriggerEnterEvents.OnTriggerEntered += AddTarget;
        _onTriggerEnterEvents.OnTriggerExited += removeTarget;
    }
    private void OnDisable()
    {
        _onTriggerEnterEvents.OnTriggerEntered -= AddTarget;
        _onTriggerEnterEvents.OnTriggerExited -= removeTarget;
    }
    void AddTarget(Collider2D collider)
    {
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;

        targetGroup.AddTarget(Target, Weight, Radius);
    }
    void removeTarget(Collider2D collider)
    {
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;

        targetGroup.RemoveTarget(Target);
    }
}
