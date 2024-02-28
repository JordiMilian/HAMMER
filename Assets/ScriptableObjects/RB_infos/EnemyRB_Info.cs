using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Rigidboy info", menuName = "Enemy_")]
public class EnemyRB_Info : ScriptableObject
{
    public string Name;
    public float Mass;
    public float LinearDrag;
    public float AngularDrag;
    public float MovementRefernceStrengh;
    public void AssingRigidbody(Rigidbody2D rb)
    {
        rb.mass = Mass;
        rb.drag = LinearDrag;
        rb.angularDrag = AngularDrag;
    }
    public void AssingMovementRef(CurveToRigidBody C2R)
    {
        C2R.Strengh = MovementRefernceStrengh;
    }
}
