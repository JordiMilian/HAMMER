using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator_Room : MonoBehaviour
{
    public Transform ExitPosition;
    public Bounds combinedWorldBounds;
    Renderer[] childRenderers;
    [SerializeField] bool CalculateBoundsTrigger;

    private void Update()
    {
        if(CalculateBoundsTrigger)
        {
            calculateBounds();
            CalculateBoundsTrigger = false;
        }
    }
    public void calculateBounds()
    {
        childRenderers = GetComponentsInChildren<Renderer>(); //get all renderers

        combinedWorldBounds = new Bounds(transform.position, Vector2.zero); //set the bounds as empty

        foreach (Renderer renderer in childRenderers)
        {
            combinedWorldBounds.Encapsulate(renderer.bounds);
        }
        UsefullMethods.BoundsToBoxCollider(combinedWorldBounds, transform.position, gameObject);

    }
 
}
