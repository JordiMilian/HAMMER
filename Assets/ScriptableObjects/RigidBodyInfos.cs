using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RigidBodyInfos : ScriptableObject
{
    
   public class RbInfo
    {
        public string Name;
        public float Mass;
        public float LinearDrag;
        public float AngularDrag;
        public float MovementRefernceStrengh;
        
        public void AssingRigidbody(Rigidbody rb)
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
    public List<RbInfo> Infos;
}
