using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ui_HealthBarController : MonoBehaviour
{
    public Player_HealthSystem _HealthSystem;
    UIDocument _UiDocument;

    VisualElement root;
    ProgressBar HealthBar;
    ProgressBar UltimateBar;
    void Start()
    {
        _UiDocument = GetComponent<UIDocument>();
        root = _UiDocument.rootVisualElement;
        HealthBar = root.Q<ProgressBar>("HealthBar");
        UltimateBar = root.Q<ProgressBar>("UltimateBar");
        HealthBar.highValue = _HealthSystem.MaxHealth;
        HealthBar.lowValue = _HealthSystem.CurrentHealth;
    }
    private void Update()
    {
        
        HealthBar.value=_HealthSystem.CurrentHealth;
        HealthBar.title = _HealthSystem.CurrentHealth.ToString();
    }
    public void UpdateHealthBar()
    {
        HealthBar.value = _HealthSystem.CurrentHealth;
    }

}
