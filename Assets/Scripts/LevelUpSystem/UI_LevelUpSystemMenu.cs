using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_LevelUpSystemMenu : MonoBehaviour
{
    [SerializeField] GameObject go_UILevelUpSystem; //Game object padre de toda la interfaz del CHECK POINT sobre el lvl up system
    [SerializeField] GameObject go_LevelUpMenu;

    [SerializeField] TextMeshProUGUI xpPointText;
    [SerializeField] TextMeshProUGUI levelUpCostText;
    [SerializeField] float interactionRadius = 1f;
    private Player_Respawner playerRespawner;

    public event Action OnLevelUpSystemActivated, OnLevelUpMenuActivated, OnLevelUpMenuClosed;

    private bool isEnabled = false;
    private bool isMenuOpened = false;

    Player_LevelStatsManager playerStatPointsManager;
    private void Awake()
    {
        isEnabled = false;
        go_UILevelUpSystem.SetActive(false);
        go_LevelUpMenu.SetActive(false);

        playerRespawner = GetComponentInParent<Player_Respawner>();
        playerStatPointsManager = GlobalPlayerReferences.Instance.references.levelStatsManager;
        playerRespawner.OnRespawnerActivated += ActivateLevelUpSystemUI;
    }

    void ActivateLevelUpSystemUI() //Activa la UI del check point
    {
        go_UILevelUpSystem.SetActive(true);
        isEnabled = true;
        isMenuOpened = false;

        InputDetector.Instance.OnSelectPressed += LevelUpSystemMenu;

        OnLevelUpSystemActivated();
        playerRespawner.OnRespawnerActivated -= ActivateLevelUpSystemUI;
    }

    void LevelUpSystemMenu() //Activa el menú para el level up
    {
        float distFromPlayer = Vector3.Magnitude(GlobalPlayerReferences.Instance.playerTf.position - transform.position);

        if (distFromPlayer > interactionRadius) return; // El character está demasiado lejos para interactuar

        if(!isMenuOpened)
        {
            OpenMenu();
        }
    }

    private void Update()
    {
        xpPointText.text = "Experience Points: " + playerStatPointsManager.xpPoints().ToString();
        levelUpCostText.text = "Cost: " + playerStatPointsManager.levelUpCost().ToString();
    }
    void OpenMenu()
    {
        isMenuOpened = true;

        go_LevelUpMenu.SetActive(true);

        GlobalPlayerReferences.Instance.references.events.CallDisable(); //Desactivamos el control del jugador
    }


    #region BUTTONS
    public void CloseMenu()
    {
        isMenuOpened = false;

        go_LevelUpMenu.SetActive(false);

        GlobalPlayerReferences.Instance.references.events.CallEnable(); //Activamos el control del jugador
    }

    public void LevelUpHP()
    {
        playerStatPointsManager.LevelUpHP();
    }

    public void LevelUpAttack()
    {
        playerStatPointsManager.LevelUpDamage();
    }

    public void LevelUpStamina()
    {
        playerStatPointsManager.LevelUpStamina();
    }

    public void ResetLevel1()
    {
        playerStatPointsManager.ResetLevel1();
    }
    #endregion
}
