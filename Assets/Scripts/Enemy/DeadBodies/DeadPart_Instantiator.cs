using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_Instantiator : MonoBehaviour
{
    [SerializeField] Transform OrientationGuide;
    [Serializable]
    public class DeadPart
    {
        public Transform referenceBone_TF;
        public GameObject deadPart_GO;
    }
    [SerializeField] List<DeadPart> deadPartsList = new List<DeadPart>();

    public GameObject[] InstantiateDeadParts()
    {
        List<GameObject> deadParts = new List<GameObject>();
        foreach (DeadPart part in deadPartsList)
        {   
            GameObject InstantiatedDeadPart = Instantiate(part.deadPart_GO, transform.position, Quaternion.identity); //Instantiate

            Vector2 bonePosition = part.referenceBone_TF.position;
            float boneRotation = part.referenceBone_TF.rotation.z;

            Vector2 rootPosition = transform.position;
            Vector2 distanceRoot2Bone = bonePosition - rootPosition;

            InstantiatedDeadPart.transform.Find("DeadPart_MovingParent").position = rootPosition + new Vector2( distanceRoot2Bone.x,0);
            Rigidbody2D SimulatedChild = InstantiatedDeadPart.transform.Find("Simulated Child").GetComponent<Rigidbody2D>();

            SimulatedChild.isKinematic = true;
            SimulatedChild.position = bonePosition;
            SimulatedChild.rotation = boneRotation;

            SimulatedChild.isKinematic = false;

            int orientation = UsefullMethods.simplifyScale(OrientationGuide.localScale.x);
            InstantiatedDeadPart.transform.localScale = new Vector3(InstantiatedDeadPart.transform.localScale.x * orientation, 1, 1); //Fix orientation

            InstantiatedDeadPart.GetComponent<DeadPartV3_Controller>().OnSpawnedPush();

            deadParts.Add(InstantiatedDeadPart);
        }
        return deadParts.ToArray();
    }
}
