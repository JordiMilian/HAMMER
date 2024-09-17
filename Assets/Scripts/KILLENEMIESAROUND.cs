using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KILLENEMIESAROUND : MonoBehaviour
{


    [SerializeField] float distanceFromPlayer;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            KILLEMALL();
        }
    }
    void KILLEMALL()
    {
        Vector2 playerPos = GlobalPlayerReferences.Instance.playerTf.position;

        GameObject[] enemiesGO = GameObject.FindGameObjectsWithTag(TagsCollection.Enemy);
        foreach (GameObject enem in enemiesGO)
        {
            Generic_HealthSystem enemieHealth = enem.GetComponent<Generic_HealthSystem>();
            if (enemieHealth == null) { continue; }
            Vector2 enemPos = enem.transform.position;
            if ((playerPos - enemPos).magnitude > distanceFromPlayer) { continue; }

            enemieHealth.RemoveLife(50, gameObject);
        }
    }
}
