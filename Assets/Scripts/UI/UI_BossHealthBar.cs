using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BossHealthBar : MonoBehaviour
{
    [SerializeField] GameObject RoomWithEnemies_InterfaceHolder;
    IRoomWithEnemies roomWithEnemies_interface;
    [SerializeField] Transform size1HealthBar;
    [SerializeField] string displayText;
    [SerializeField] Canvas displayCanvas;
    [SerializeField] TextMeshProUGUI textMeshPro;

    List<IHealth> currentStatsList = new List<IHealth>();

    float MaxHealth;

    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<IRoomWithEnemies>(ref RoomWithEnemies_InterfaceHolder, ref roomWithEnemies_interface);
    }
    private void OnEnable()
    {
        OnValidate();
        roomWithEnemies_interface.OnEnemiesSpawned += SetHealthBar;
        HideCanvas();
        textMeshPro.text = displayText;
        GameEvents.OnPlayerDeath += HideCanvas;
    }
    private void OnDisable()
    {
        roomWithEnemies_interface.OnEnemiesSpawned -= SetHealthBar;
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
    void SetHealthBar()
    {
        MaxHealth = 0;
        size1HealthBar.localScale = Vector3.one;

        foreach (GameObject enemy in roomWithEnemies_interface.CurrentlySpawnedEnemies)
        {
            enemy.transform.Find("Sprites").Find("BasicHealthbarLogic").gameObject.SetActive(false);

            IHealth thisCurrentHealth = enemy.GetComponent<IHealth>();
            currentStatsList.Add(thisCurrentHealth);
            
            MaxHealth += thisCurrentHealth.GetMaxHealth();

            thisCurrentHealth.OnHealthUpdated += UpdateHealthBarSize;
        }
    }

    void UpdateHealthBarSize()
    {
        float CurrentHealth = 0;

        foreach (IHealth helath in currentStatsList)
        {
            CurrentHealth += helath.GetCurrentHealth();
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
