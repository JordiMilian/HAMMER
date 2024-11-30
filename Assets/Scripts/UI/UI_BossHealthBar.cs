using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BossHealthBar : MonoBehaviour
{
    [SerializeField] RoomWithEnemiesLogic roomWithEnemies;
    [SerializeField] Transform size1HealthBar;
    [SerializeField] string displayText;
    [SerializeField] Canvas displayCanvas;
    [SerializeField] TextMeshProUGUI textMeshPro;

    List<EnemyStats> currentStatsList = new List<EnemyStats>();

    float MaxHealth;

    private void OnEnable()
    {
        roomWithEnemies.onEnemiesSpawned += RefillHealthBar;
        HideCanvas();
        textMeshPro.text = displayText;
        GameEvents.OnPlayerDeath += HideCanvas;
    }
    private void OnDisable()
    {
        roomWithEnemies.onEnemiesSpawned -= RefillHealthBar;
        GameEvents.OnPlayerDeath -= HideCanvas;
    }
    public void HideCanvas()
    {
        displayCanvas.enabled = false;
    }
    public void ShowCanvas() //Canvas is shown throw the BossRoomCutscene
    {
        displayCanvas.enabled = true;
    }
    void RefillHealthBar()
    {
        //Unsubscribe from old health and remove them
        foreach (EnemyStats thisCurrentStat in currentStatsList)
        {
            thisCurrentStat.OnCurrentHpChange -= UpdateHealthBarSize;
        }
        currentStatsList.Clear();

        MaxHealth = 0;
        size1HealthBar.localScale = Vector3.one;

        foreach (GameObject enemy in roomWithEnemies.CurrentlySpawnedEnemies)
        {
            enemy.transform.Find("Sprites").Find("BasicHealthbarLogic").gameObject.SetActive(false);

            EnemyStats thisCurrentStats = enemy.GetComponent<Enemy_References>().currentEnemyStats;
            currentStatsList.Add(thisCurrentStats);
            
            MaxHealth += thisCurrentStats.MaxHp;

            thisCurrentStats.OnCurrentHpChange += UpdateHealthBarSize;
        }
    }

    void UpdateHealthBarSize(float notUsed)
    {
        
        float CurrentHealth = 0;

        foreach (EnemyStats thisCurrentStats in currentStatsList)
        {
            CurrentHealth += thisCurrentStats.CurrentHp;
        }
        float normalizedSize = Mathf.InverseLerp(0, MaxHealth, CurrentHealth);

        size1HealthBar.localScale = new Vector3(normalizedSize, 1, 1);

        //Debug.Log("boss health updated: " + CurrentHealth + "/" + MaxHealth);

        if (CurrentHealth <= 0)
        {
            StartCoroutine(delayedHideCanvas());
        }
    }

    IEnumerator delayedHideCanvas()
    {
        yield return new WaitForSeconds(1);
        HideCanvas();
    }
}
