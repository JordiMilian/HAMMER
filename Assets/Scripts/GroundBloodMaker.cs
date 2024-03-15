using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GroundBloodMaker : MonoBehaviour
{
    [SerializeField] Transform BloodOffset;
    [SerializeField] VisualEffect BloodVFX;

    public static GroundBloodMaker Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Play(Vector2 position, Vector2 direction, float intensity)
    {
        BloodOffset.position = position;
        BloodOffset.up = direction;
        BloodVFX.SetFloat("Intensity", intensity);
        BloodVFX.Play();
    }
}
