using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupSingleton : MonoBehaviour
{
    public static TargetGroupSingleton Instance;
    CinemachineTargetGroup targetGroup;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public static int FindEmptyTargetgroupSlot(CinemachineTargetGroup group)
    {
        for (int i = 0; i < group.m_Targets.Length; i++)
        {
            if (group.m_Targets[i].target != null) { continue; }
            else { return i; }
        }
        Debug.LogError("No empty Slot found, please create more");
        return -1;
    }
    public void AddTarget(Transform target, float weight, float radius)
    {
        //If this target is already ON, break
        foreach(CinemachineTargetGroup.Target t in targetGroup.m_Targets)
        {
            if(t.target == target) { return; }
        }
        int emptySlotIndex = FindEmptyTargetgroupSlot(targetGroup);

        targetGroup.m_Targets[emptySlotIndex].target = target;
        targetGroup.m_Targets[emptySlotIndex].weight = weight;
        targetGroup.m_Targets[emptySlotIndex].radius = radius;
    }
    public void RemoveTarget(Transform target)
    {
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == target)
            {
                targetGroup.m_Targets[i].target = null;
                targetGroup.m_Targets[i].weight = 0;
                targetGroup.m_Targets[i].radius = 0;
            }
        }
    }
    public void EditTarget(Transform target, float newWeight, float  newRadius)
    {
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == target)
            {
                targetGroup.m_Targets[i].weight = newWeight;
                targetGroup.m_Targets[i].radius = newRadius;
                return;
            }
        }
        Debug.LogWarning(target.name + " target group was not found");
    }
    public Vector2 GetTargetStats(Transform target)
    {
        Vector2 targetStats = Vector2.zero;
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == target)
            {
                targetStats = new Vector2(targetGroup.m_Targets[i].weight, targetGroup.m_Targets[i].radius);
                break;
            }
        }
        return targetStats;
    }
    
}
