using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_GesturesPrototipe : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    [SerializeField] GesturesDetector gesturesDetector;
     PlayerState ClockwiseAttack, NotClockwiseAttack;
    [SerializeField] PlayerState StartingTapAttack;

    private void OnEnable()
    {
        gesturesDetector.OnArcDetected += OnArcDetected;
        gesturesDetector.OnTapDetected += OnTapDetected;
    }
    private void OnDisable()
    {
        gesturesDetector.OnArcDetected -= OnArcDetected;
        gesturesDetector.OnTapDetected -= OnTapDetected;
    }
    
    void OnTapDetected(Vector2 direction)
    {
        playerRefs.stateMachine.RequestChangeState(StartingTapAttack);
    }
    void OnArcDetected(ArcData arcData)
    {
        if(NotClockwiseAttack == null) { GetAttacksRefs(); }
        playerRefs.stateMachine.RequestChangeState(arcData.isClockwise ? ClockwiseAttack : NotClockwiseAttack);

        //
        void GetAttacksRefs()
        {
            NotClockwiseAttack = FindObjectByName(playerRefs.StatesRoots.transform, "BasicComboAttack_01").GetComponent<PlayerState>();
            ClockwiseAttack = FindObjectByName(playerRefs.StatesRoots.transform, "BasicComboAttack_02").GetComponent<PlayerState>();
        }
    }

    GameObject FindObjectByName(Transform parent, string name)
    {
        foreach(Transform child in parent)
        {
            if (child.name == name)
            {
                Debug.Log($"Found {name} in {parent.name}");
                return child.gameObject;
            }

            GameObject foundChild = FindObjectByName(child, name);
            if(foundChild != null)
            {
                return foundChild;
            }
        }
        return null;
    }
}
