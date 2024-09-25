using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupSingleton : MonoBehaviour
{
    public static TargetGroupSingleton Instance;
    CinemachineTargetGroup targetGroup;
    Vector2 DefaultPlayerStats;
    Vector2 DefaultMouseStats;
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

        DefaultPlayerStats =new Vector2(  targetGroup.m_Targets[0].weight, targetGroup.m_Targets[0].radius );
        DefaultMouseStats =new Vector2(  targetGroup.m_Targets[1].weight, targetGroup.m_Targets[1].radius );
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

        //setTargetsStats(targetGroup.m_Targets[emptySlotIndex], target, weight, radius);
        targetGroup.m_Targets[emptySlotIndex].target = target;
        targetGroup.m_Targets[emptySlotIndex].weight = weight;
        targetGroup.m_Targets[emptySlotIndex].radius = radius;
   
        Debug.Log("Added target: " + target.name);
    }
    public void RemoveTarget(Transform target, Transform extraTarget = null)
    {
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == target || targetGroup.m_Targets[i].target == extraTarget)
            {
                targetGroup.m_Targets[i].target = null;
                targetGroup.m_Targets[i].weight = 0;
                targetGroup.m_Targets[i].radius = 0;
            }
        }
        Debug.Log("Removed target: " + target.name);
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
    public void SetOnlyOneTarget(Transform targetTf, float Weight, float Radius) //millor no gastar aixo que pot causar lios
    {
        for(int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            setTargetsStats(i, null, 0,0);

        }
        setTargetsStats(0, targetTf, Weight, Radius);
    }
    public void RemovePlayersTarget()
    {
        RemoveTarget(GlobalPlayerReferences.Instance.playerTf, MouseCameraTarget.Instance.transform);
    }
    public void ReturnPlayersTarget()
    {
        AddTarget(GlobalPlayerReferences.Instance.playerTf, DefaultPlayerStats.x, DefaultPlayerStats.y);
        AddTarget(MouseCameraTarget.Instance.transform, DefaultMouseStats.x, DefaultMouseStats.y);
    }
    void setTargetsStats(int index, Transform tf , float weight, float radius)
    {
        targetGroup.m_Targets[index].target = tf;
        targetGroup.m_Targets[index].weight = weight;
        targetGroup.m_Targets[index].radius = radius;
    }
    
}
