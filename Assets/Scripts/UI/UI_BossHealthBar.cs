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

    List<Generic_HealthSystem> healthsList = new List<Generic_HealthSystem>();

    float MaxHealth;

    private void OnEnable()
    {
        roomWithEnemies.onEnemiesSpawned += RefillHealthBar;
        HideCanvas();
        textMeshPro.text = displayText;
    }
    private void OnDisable()
    {
        roomWithEnemies.onEnemiesSpawned -= RefillHealthBar;
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
        foreach (Generic_HealthSystem thisHealth in healthsList)
        {
            thisHealth.CurrentHP.OnValueChanged -= UpdateHealthBarSize;
        }
        healthsList.Clear();

        MaxHealth = 0;
        size1HealthBar.localScale = Vector3.one;

        foreach (GameObject enemy in roomWithEnemies.CurrentlySpawnedEnemies)
        {
            enemy.transform.Find("Sprites").Find("BasicHealthbarLogic").gameObject.SetActive(false);

            Generic_HealthSystem thisHealth = enemy.GetComponent<Generic_HealthSystem>();
            healthsList.Add(thisHealth);
            
            MaxHealth += thisHealth.MaxHP.GetValue();

            thisHealth.CurrentHP.OnValueChanged += UpdateHealthBarSize;
        }
    }

    void UpdateHealthBarSize()
    {
        
        float CurrentHealth = 0;

        foreach (Generic_HealthSystem thisHealth in healthsList)
        {
            CurrentHealth += thisHealth.CurrentHP.GetValue();
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
