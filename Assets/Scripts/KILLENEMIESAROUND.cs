using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KILLENEMIESAROUND : MonoBehaviour, IDamageDealer
{
    List<IDamageReceiver> enemiesReceivers = new List<IDamageReceiver>();
    [SerializeField] float InstaKillEnemiesOnDistance;
    [SerializeField] Generic_DamageDealer damageDealer;
     Room_script currentRoom;

    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            KILLEMALL();
        }
    }
    private void Start()
    {
        InvokeRepeating("CheckEnemiesDistanceAndKill", 1, 5);
    }
    List<IDamageReceiver> GetEnemiesDamageReceivers()
    {
        IRoomWithEnemies tempRoomWithEnemies = GameController.Instance.roomsLoader.CurrentLoadedRoom.GetComponentInChildren<IRoomWithEnemies>();
        enemiesReceivers = new();
        if (tempRoomWithEnemies != null)
        {
            List<IDamageReceiver> receiver = new();
            foreach(GameObject enemy in tempRoomWithEnemies.CurrentlySpawnedEnemies)
            {
                receiver.Add(enemy.GetComponent<IDamageReceiver>());
            }
            enemiesReceivers = receiver;
            return receiver;
        }
        Debug.LogWarning("No Room with enemies detected");
        return null;
    }
    void KILLEMALL()
    {
        GetEnemiesDamageReceivers();
        StartCoroutine(KillEmSlowly(enemiesReceivers.ToArray()));
    }
    void CheckEnemiesDistanceAndKill()
    {
        GetEnemiesDamageReceivers();
        if(enemiesReceivers.Count == 0) { return; };
        Vector2 referencePoint = GlobalPlayerReferences.Instance.transform.position;
        foreach(IDamageReceiver receiver in enemiesReceivers)
        {
            Vector2 enemyPos = (receiver as MonoBehaviour).transform.position;
            if((enemyPos - referencePoint).magnitude > InstaKillEnemiesOnDistance)
            {
                receiver.OnDamageReceived(new ReceivedAttackInfo(Vector2.zero, Vector2.zero, Vector2.zero,gameObject, damageDealer, 100,0,false,10));
                Debug.Log("Killed: " + (receiver as MonoBehaviour).gameObject.name + "because he got too far");
            }
        }
    }
    IEnumerator KillEmSlowly(IDamageReceiver[] receiversList)
    {
        foreach(IDamageReceiver receiver in receiversList)
        {
            receiver.OnDamageReceived(new ReceivedAttackInfo(Vector2.zero, Vector2.zero, Vector2.zero, gameObject, damageDealer, 100, 0, false,10));
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
}
