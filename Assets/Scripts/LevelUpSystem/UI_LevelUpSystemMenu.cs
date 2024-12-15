using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_LevelUpSystemMenu : MonoBehaviour
{
    [SerializeField] GameObject go_UILevelUpSystem; //Game object padre de toda la interfaz del CHECK POINT sobre el lvl up system
    [SerializeField] GameObject go_LevelUpMenu;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI xpPointText;
    [SerializeField] TextMeshProUGUI levelUpCostText;
    [SerializeField] float interactionRadius = 1f;
    private Player_Respawner playerRespawner;


    public event Action OnLevelUpSystemActivated, OnLevelUpMenuActivated, OnLevelUpMenuClosed;

    private bool isEnabled = false;
    private bool isMenuOpened = false;

    Player_LevelStatsManager playerStatPointsManager;

    [Header("Progress Bar")]
    [SerializeField] int numOfProgressBar = 5;
    [SerializeField] float offSetProgressBar = 50;

    [Space]
    [SerializeField] GameObject go_HealthBar_0;
    [SerializeField] GameObject go_StaminaBar_0;
    [SerializeField] GameObject go_DamageBar_0;

    [Space]
    [SerializeField] Color color_DefaultProgressBar;
    [SerializeField] Color color_ActivatedProgressBar;
    [SerializeField] Color color_DeactivatedProgressBar;
    [SerializeField] Color color_SelectedProgressBar;
    [SerializeField] Color color_OutOfCostProgressBar;
    [SerializeField] Color color_DefaultText;

    [Space]
    [SerializeField] List<GameObject> healthBarProgressBarList = new List<GameObject>();
    [SerializeField] List<GameObject> staminaBarProgressBarList = new List<GameObject>();
    [SerializeField] List<GameObject> damageBarProgressBarList = new List<GameObject>();

    int defaultHp = 1;
    int defaultStamina = 1;
    int defaultDamage = 1;

    int tempCurrentExperiencePoitns;
    int tempLevelUpCost;
    int tempLevel;

    [Space]
    [SerializeField] int statDivider = 3;
    private void Start()
    {
        defaultHp = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.baseStats.MaxHp / statDivider);
        defaultStamina = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.baseStats.MaxStamina / statDivider);
        defaultDamage = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.baseStats.DamageMultiplicator);

        InstantiateProgressBar(healthBarProgressBarList, go_HealthBar_0, defaultHp);
        InstantiateProgressBar(staminaBarProgressBarList, go_StaminaBar_0, defaultStamina);
        InstantiateProgressBar(damageBarProgressBarList, go_DamageBar_0, defaultDamage);

        ResetMenu();
    }

    void InstantiateProgressBar(List<GameObject> progressBarList, GameObject goProgressBar, int defaultValue)
    {
        progressBarList.Clear();
        progressBarList.Add(goProgressBar);

        for (int i = 0; i < numOfProgressBar; i++)
        {
            GameObject newBar = Instantiate(goProgressBar, goProgressBar.transform.parent);

            RectTransform newRectTransform = newBar.GetComponent<RectTransform>();
            newRectTransform.anchoredPosition = goProgressBar.GetComponent<RectTransform>().anchoredPosition;

            newRectTransform.anchoredPosition += new Vector2((i + 1) * offSetProgressBar, 0);

            progressBarList.Add(newBar);
        }

        for (int i = 0; i < progressBarList.Count; i++)
        {
            RawImage rawImage = progressBarList[i].GetComponent<RawImage>();

            if (i < defaultValue)
            {
                rawImage.color = color_DefaultProgressBar;
            }
            else rawImage.color = color_DeactivatedProgressBar;
        }
    }
    private void OnEnable()
    {
        isEnabled = false;
        go_UILevelUpSystem.SetActive(false);
        go_LevelUpMenu.SetActive(false);
 
        playerRespawner = GetComponentInParent<Player_Respawner>();
        playerStatPointsManager = GlobalPlayerReferences.Instance.references.levelStatsManager;
        playerRespawner.OnRespawnerActivated += ActivateLevelUpSystemUI;

        tempLevelUpCost = playerStatPointsManager.levelUpCost();
        tempLevel = GlobalPlayerReferences.Instance.references.currentStats.Level;
        tempCurrentExperiencePoitns = GlobalPlayerReferences.Instance.references.currentStats.ExperiencePoints;
    }
    void ActivateLevelUpSystemUI() //Activa la UI del check point
    {
        go_UILevelUpSystem.SetActive(true);
        isEnabled = true;
        isMenuOpened = false;

        InputDetector.Instance.OnSelectPressed += LevelUpSystemMenu;

        OnLevelUpSystemActivated?.Invoke();
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
        levelText.text = tempLevel.ToString();
        xpPointText.text = tempCurrentExperiencePoitns.ToString();
        levelUpCostText.text = tempLevelUpCost.ToString();
    }
    void OpenMenu()
    {
        isMenuOpened = true;

        go_LevelUpMenu.SetActive(true);
        ResetMenu();

        GlobalPlayerReferences.Instance.references.events.CallDisable(); //Desactivamos el control del jugador
    }


    #region BUTTONS
    public void CloseMenu()
    {
        isMenuOpened = false;

        ResetMenu();

        go_LevelUpMenu.SetActive(false);

        GlobalPlayerReferences.Instance.references.events.CallEnable(); //Activamos el control del jugador
    }

    void ResetMenu()
    {
        xpPointText.color = color_DefaultText;
        levelText.color = color_DefaultText;

        ResetProgressBar(healthBarProgressBarList);
        ResetProgressBar(staminaBarProgressBarList);
        ResetProgressBar(damageBarProgressBarList);

        tempLevelUpCost = playerStatPointsManager.levelUpCost();
        tempLevel = GlobalPlayerReferences.Instance.references.currentStats.Level;
        tempCurrentExperiencePoitns = GlobalPlayerReferences.Instance.references.currentStats.ExperiencePoints;

        SelectedHPBar.Clear();
        SelectedStaminaBar.Clear();
        SelectedDamageBar.Clear();

        ApplyProgressBarLevels(healthBarProgressBarList, playerStatPointsManager.GetHPLevel(), defaultHp);
        ApplyProgressBarLevels(staminaBarProgressBarList, playerStatPointsManager.GetStaminaLevel(), defaultStamina);
        ApplyProgressBarLevels(damageBarProgressBarList, playerStatPointsManager.GetDamageLevel(), defaultDamage);
    }

    void ApplyProgressBarLevels(List<GameObject> progressBar, int level, int baseLevel)
    {
        for (int i = 0; i < level; i++)
        {
            RawImage rawImage = progressBar[baseLevel + i].GetComponent<RawImage>();

            rawImage.color = color_ActivatedProgressBar;
        }
    }
    void ResetProgressBar(List<GameObject> progressBar)
    {
        for (int i = 0; i < progressBar.Count; i++)
        {
            RawImage rawImage = progressBar[i].GetComponent<RawImage>();

            if(rawImage.color != color_DefaultProgressBar) rawImage.color = color_DeactivatedProgressBar;
        }
    }
    void LevelUpHP()
    {
        playerStatPointsManager.LevelUpHP();
    }

    void LevelUpDamage()
    {
        playerStatPointsManager.LevelUpDamage();
    }

    void LevelUpStamina()
    {
        playerStatPointsManager.LevelUpStamina();
    }

    public void ConfirmLevelUp()
    {
        for (int i = 0; i < SelectedHPBar.Count; i++)
        {
            LevelUpHP();
        }

        for (int i = 0; i < SelectedStaminaBar.Count; i++)
        {
            LevelUpStamina();
        }

        for (int i = 0; i < SelectedDamageBar.Count; i++)
        {
            LevelUpDamage();
        }

        GlobalPlayerReferences.Instance.references.currentStats.Level = tempLevel;
        GlobalPlayerReferences.Instance.references.currentStats.ExperiencePoints = tempCurrentExperiencePoitns;

        ConfirmProgressBarSelection(healthBarProgressBarList);
        ConfirmProgressBarSelection(staminaBarProgressBarList);
        ConfirmProgressBarSelection(damageBarProgressBarList);

        xpPointText.color = color_DefaultText;
        levelText.color = color_DefaultText;
    }

    public void ResetLevel1()
    {
        playerStatPointsManager.ResetLevel1();
    }
    public void ReturnToOutOfRun()
    {
        SceneManager.LoadScene("OutOfRunWorld");
    }
    #endregion


    List<GameObject> SelectedHPBar = new List<GameObject>();
    List<GameObject> SelectedStaminaBar = new List<GameObject>();
    List<GameObject> SelectedDamageBar = new List<GameObject>();
    public void SelectLevelUpHP()
    {
        ChangeLevelUpBar(healthBarProgressBarList, SelectedHPBar);
    }

    public void SelectLevelUpStamina()
    {
        ChangeLevelUpBar(staminaBarProgressBarList, SelectedStaminaBar);
    }

    public void SelectLevelUpDamage()
    {
        ChangeLevelUpBar(damageBarProgressBarList, SelectedDamageBar);
    }

    void ConfirmProgressBarSelection(List<GameObject> progressBar)
    {
        for (int i = 0; i < progressBar.Count; i++)
        {
            RawImage rawImage = progressBar[i].GetComponent<RawImage>();

            if (rawImage.color == color_SelectedProgressBar) rawImage.color = color_ActivatedProgressBar;
            if (rawImage.color == color_OutOfCostProgressBar) rawImage.color = color_DeactivatedProgressBar;
        }
    }
    void ChangeLevelUpBar(List<GameObject> originalProgressBarList, List<GameObject> selectedList)
    {
        for (int i = 0; i < originalProgressBarList.Count; i++)
        {
            RawImage rawImage = originalProgressBarList[i].GetComponent<RawImage>();
            if (rawImage.color == color_OutOfCostProgressBar) break;

            if (rawImage.color == color_DeactivatedProgressBar)
            {
                if (tempCurrentExperiencePoitns - tempLevelUpCost >= 0)
                {
                    rawImage.color = color_SelectedProgressBar;
                    tempLevel++;
                    tempCurrentExperiencePoitns -= tempLevelUpCost;
                    tempLevelUpCost = playerStatPointsManager.levelUpCostCalculation(tempLevel);

                    selectedList.Add(originalProgressBarList[i]);
                    xpPointText.color = color_DefaultText;
                    levelText.color = color_DefaultText;
                    break;
                }
                else
                {
                    rawImage.color = color_OutOfCostProgressBar;
                    xpPointText.color = color_OutOfCostProgressBar;
                    levelText.color = color_OutOfCostProgressBar;
                    break;
                }
            }
        }
    }
}
