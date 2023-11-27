using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCristal_spawner : MonoBehaviour
{
    bool yoquese;
    [SerializeField] GameObject Cristals;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(Cristals);
    }
}
