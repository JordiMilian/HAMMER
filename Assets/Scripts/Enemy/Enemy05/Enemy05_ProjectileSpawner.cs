using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy05_ProjectileSpawner : MonoBehaviour
{
    [SerializeField] Transform WeaponRootTf;
    [SerializeField] GameObject ProjectilePrefab;
    public void EV_SpawnDirectionProjectile()
    {
        GameObject newProjectile = Instantiate(ProjectilePrefab, WeaponRootTf.position, WeaponRootTf.rotation);
        newProjectile.transform.position = WeaponRootTf.position;
        Debug.Log(WeaponRootTf.position);
    }
}
