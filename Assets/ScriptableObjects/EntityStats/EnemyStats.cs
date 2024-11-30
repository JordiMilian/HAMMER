using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Entity Stats", menuName = "Entity Stats/EnemyStats")]
public class EnemyStats : EntityStats
{
    [SerializeField] private int _xpDrop;

    [HideInInspector] public UnityEvent<float> OnXpDropChange;

    public int CurrentXPDrops
    {
        get => _xpDrop;
        set
        {
            if (Mathf.Approximately(_xpDrop, value)) return;
            _xpDrop = value;
            OnXpDropChange?.Invoke(_xpDrop);
        }
    }
    public void CopyStats(EnemyStats importedStats)
    {
        MaxHp = importedStats.MaxHp;

    }
}
