using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ui_HealthBarController : MonoBehaviour
{
    public Player_HealthSystem _HealthSystem;
    public FloatVariable PlayerHP;
    public FloatVariable PlayerMaxHP;
    UIDocument _UiDocument;

    VisualElement root;
    ProgressBar HealthBar;
    void Start()
    {
        _HealthSystem = GameObject.Find("MainCharacter").GetComponent<Player_HealthSystem>();
        _UiDocument = GetComponent<UIDocument>();
        root = _UiDocument.rootVisualElement;
        HealthBar = root.Q<ProgressBar>("HealthBar");
        HealthBar.highValue = PlayerMaxHP.Value;
        HealthBar.lowValue = PlayerHP.Value;
    }
    private void Update()
    {
        
        HealthBar.value = PlayerHP.Value;
        HealthBar.title = PlayerHP.Value.ToString();
    }
    public void UpdateHealthBar()
    {
        HealthBar.value = PlayerHP.Value;
    }

}
