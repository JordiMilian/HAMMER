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
        List<Generic_HealthSystem> healthsList = new List<Generic_HealthSystem>();
        foreach (GameObject enem in enemiesGO)
        {
            Generic_HealthSystem enemieHealth = enem.GetComponent<Generic_HealthSystem>();
            
            if (enemieHealth == null) { continue; }
            
            Vector2 enemPos = enem.transform.position;
            if ((playerPos - enemPos).magnitude > distanceFromPlayer) { continue; }

            healthsList.Add(enemieHealth);
            
        }

        StartCoroutine(KillEmSlowly(healthsList.ToArray()));
    }
    IEnumerator KillEmSlowly(Generic_HealthSystem[] healthsArray)
    {
        foreach(Generic_HealthSystem health in healthsArray)
        {
            health.RemoveLife(50, gameObject);
            yield return null;
        }
    }
}
