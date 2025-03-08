using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour, IDamageReceiver, IDamageDealer, IHealth, IStats
{
    Player_References playerRefs;

    #region HEALTH MANAGEMENT
    public float GetCurrentHealth()
    {
        throw new System.NotImplementedException();
    }
    public float GetMaxHealth()
    {
        throw new System.NotImplementedException();
    }
    public void OnZeroHealth()
    {
        throw new System.NotImplementedException();
    }
    public void RemoveHealth(float health)
    {
        throw new System.NotImplementedException();
    }
    public void RestoreAllHealth()
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region DAMAGE DEALT
    public void OnDamageDealt(DealtDamageInfo info)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region DAMAGE RECEIVED
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        // Remove Health
        // Flasher
        // Push player
        // Stance?
    }
    #endregion
    #region STATS
    public EntityStats GetBaseStats()
    {
        throw new System.NotImplementedException();
    }
    public EntityStats GetCurrentStats()
    {
        throw new System.NotImplementedException();
    }
    public void SetBaseStats(EntityStats stats)
    {
        throw new System.NotImplementedException();
    }
    public void SetCurrentStats(EntityStats stats)
    {
        throw new System.NotImplementedException();
    }
#endregion
}
