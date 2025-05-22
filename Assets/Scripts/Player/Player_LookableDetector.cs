using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LookableDetector : MonoBehaviour
{
    public struct LookableData
    {
        public bool IsInputLookable;
        public Transform Transform;

        public LookableData(Transform lookableTransform, bool isInputLookable)
        {
            IsInputLookable = isInputLookable;
            Transform = lookableTransform;
        }
    }
    //The only thing that matters are this two things. The list and the method GetClosest(). 
    //All lookables need a collider in the proper Layer
    public List<LookableData> LookablesDetectedList = new List<LookableData>();
    public int GetClosestLookableIndex(bool ignoreInputLookable) //If it return -1, it means NULL
    {
        if (LookablesDetectedList.Count == 0) { return -1; }

        int closestIndex = -1;
        float closestDistance = 99999;
        List<int> indexesToRemove = new List<int>();
        for (int l = 0; l < LookablesDetectedList.Count; l++)
        {

            LookableData lookable = LookablesDetectedList[l];

            if (lookable.Transform == null) { indexesToRemove.Add(l); continue; } //if null, add to list to remove and continue

            if (lookable.IsInputLookable && ignoreInputLookable) { continue; } //If this is an input lookable and we are ignoring, skip 

            float thisDistance = (LookablesDetectedList[l].Transform.position - transform.position).sqrMagnitude;
            if (thisDistance < closestDistance)
            {
                closestDistance = thisDistance;
                closestIndex = l;
            }
        }
        //remove nulls
        for (int i = LookablesDetectedList.Count - 1; i >= 0; i--)
        {
            if (indexesToRemove.Contains(i))
            {
                LookablesDetectedList.RemoveAt(i);
                if(closestIndex > i) { closestIndex--; }
            }
        }
        return closestIndex;
    }
    //make sure not to look for a position too far from the player, since the list of lookables is detected from a collider in the player
    public int GetCLosestLookableIndexToPosition(bool ignoreInputLookable, Vector2 position, out float distance) 
    {
        distance = 0f;
        if (LookablesDetectedList.Count == 0) { return -1; }

        int closestIndex = -1;
        float closestDistance = 99999;
        List<int> indexesToRemove = new List<int>();
        for (int l = 0; l < LookablesDetectedList.Count; l++)
        {
            LookableData lookable = LookablesDetectedList[l];

            if (lookable.Transform == null) { indexesToRemove.Add(l); continue; } //if null, add to list to remove and continue

            if (lookable.IsInputLookable && ignoreInputLookable) { continue; } //If this is an input lookable and we are ignoring, skip 

            float thisDistance = ((Vector2)LookablesDetectedList[l].Transform.position - position).sqrMagnitude;
            if (thisDistance < closestDistance)
            {
                closestDistance = thisDistance;
                closestIndex = l;
                distance = closestDistance;
            }
        }
        //remove nulls
        for (int i = LookablesDetectedList.Count - 1; i >= 0; i--)
        {
            if (indexesToRemove.Contains(i))
            {
                LookablesDetectedList.RemoveAt(i);
                if (closestIndex > i) { closestIndex--; }
            }
        }
        return closestIndex;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform thisTransform = collision.transform;

        LookableData newLookable = new LookableData(thisTransform, IsLookableMouseInput(thisTransform));

        if (LookablesDetectedList.Contains(newLookable)) { return; }
        LookablesDetectedList.Add(newLookable);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        LookableData newLookable = new LookableData(collision.transform, IsLookableMouseInput(collision.transform));
        LookablesDetectedList.Remove(newLookable);
    }
    bool IsLookableMouseInput(Transform lookableTf)
    {
        MouseCameraTarget cameraTarget = lookableTf.GetComponent<MouseCameraTarget>();
        if (cameraTarget != null) { return true; }
        else { return false; }
    }
}


       
