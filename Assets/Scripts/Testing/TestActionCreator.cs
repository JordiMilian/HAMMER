using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActionCreator : MonoBehaviour
{
    [SerializeField] Player_ActionPerformer actionPerformer;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attack");
            actionPerformer.AddAction(new Player_ActionPerformer.Action("Attack"));
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Roll");
            actionPerformer.AddAction(new Player_ActionPerformer.Action("Roll"));
        }
    }
}
