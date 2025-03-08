using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPart_Instantiator : MonoBehaviour
{
    [SerializeField] Generic_EventSystem eventSystem;
    [SerializeField] Transform OrientationGuide;
    [Serializable]
    public class DeadPart
    {
        public Transform referenceBone_TF;
        public GameObject deadPart_GO;
    }
    [SerializeField] List<DeadPart> deadPartsList = new List<DeadPart>();

    public void InstantiateDeadParts(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        List<GameObject> deadParts = new List<GameObject>();
        foreach (DeadPart part in deadPartsList)
        {   
            Vector2 direction = (transform.position - args.Killer.transform.position).normalized; //Find direction
     
            GameObject InstantiatedDeadPart = Instantiate(part.deadPart_GO, transform.position, Quaternion.identity); //Instantiate

            Vector2 bonePosition = part.referenceBone_TF.position;
            Vector2 rootPosition = transform.position;
            float boneRotation = part.referenceBone_TF.rotation.z;
            Vector2 distanceRoot2Bone = bonePosition - rootPosition;
            InstantiatedDeadPart.transform.Find("DeadPart_MovingParent").position = rootPosition + new Vector2( distanceRoot2Bone.x,0);
            Rigidbody2D SimulatedChild = InstantiatedDeadPart.transform.Find("Simulated Child").GetComponent<Rigidbody2D>();
            SimulatedChild.isKinematic = true;
            SimulatedChild.position = bonePosition;
            SimulatedChild.rotation = boneRotation;
            //Debug.Log("Transform: " + SimulatedChild.transform.rotation.z + "  RB: " + SimulatedChild.rotation + "  Bone: " + boneRotation);
            SimulatedChild.isKinematic = false;

            int orientation = UsefullMethods.simplifyScale(OrientationGuide.localScale.x);
            InstantiatedDeadPart.transform.localScale = new Vector3(InstantiatedDeadPart.transform.localScale.x * orientation, 1, 1); //Fix orientation

            StartCoroutine(InvokeWithDelay(InstantiatedDeadPart, direction));//Invoke with a slight delay so everyone can subscribe

            deadParts.Add(InstantiatedDeadPart);
        }
    }
    IEnumerator InvokeWithDelay(GameObject instantiated, Vector2 direction)
    {
        yield return new WaitForSecondsRealtime(0.02f);
        instantiated.GetComponent<DeadPart_EventSystem>().OnSpawned?.Invoke(this, new Generic_EventSystem.ObjectDirectionArgs(direction));
    }
}
