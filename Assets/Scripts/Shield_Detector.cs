using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Detector : MonoBehaviour
{
    Collider MC_Collider;
    void Start()
    {
        MC_Collider = GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tocando escudo");
        if (other.tag == "Enemy_attacl_hitbox")
        {
            StartCoroutine(DisableMC_Collider());
        }
    }
    IEnumerator DisableMC_Collider()
    {
        MC_Collider.enabled = false;
        yield return new WaitForSeconds(0.3f);
        MC_Collider.enabled = true;
       
    }
}
