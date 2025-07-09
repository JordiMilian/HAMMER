using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GroundBloodPlayer : MonoBehaviour
{
    [SerializeField] Transform vfxPlayerTF;
    [SerializeField] VisualEffect groundBloodVFX;
    public static GroundBloodPlayer Instance;
    float baseLifetime;
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
    public void PlayGroundBlood(Vector2 position, Vector2 direction, float intensity = 1)
    {
        vfxPlayerTF.position = position;
        vfxPlayerTF.up = direction;
        groundBloodVFX.SetFloat("Intensity", intensity);
        groundBloodVFX.Play();
    }
    public void ResetGroundBloods()
    {
        baseLifetime = groundBloodVFX.GetFloat("MaxLifetime");
        groundBloodVFX.SetFloat("MaxLifetime", 0);
        Invoke("ReturnLifetime", 0.1f);
    }
    void ReturnLifetime()
    {
        groundBloodVFX.SetFloat("MaxLifetime", baseLifetime);
    }
}
