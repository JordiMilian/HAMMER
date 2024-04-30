using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ControllerControl : MonoBehaviour
{
    [SerializeField] List<Animator> ButtonAnimators = new List<Animator>();
    [SerializeField] InputDetector inputDetector;
    [SerializeField] int currentSelectedIndex;
    [HideInInspector] public bool isReadingInput = true;

    string Highlighted = "Highlighted";
    string isHighlighted = "isHighlighted";
    string Pressed = "Pressed";
    string Normal = "Normal";
    string Selected = "Selected";
    private void Start()
    {
        RestartSelection();
        isReadingInput = true;
    }
    private void OnEnable()
    {
        inputDetector.OnDownPressed += selectLowerButton;
        inputDetector.OnUpPressed += selectUpperButton;
        inputDetector.OnSelectPressed += SelectCurrentHighlight;
    }
    private void OnDisable()
    {
        inputDetector.OnDownPressed -= selectLowerButton;
        inputDetector.OnUpPressed -= selectUpperButton;
        inputDetector.OnSelectPressed -= SelectCurrentHighlight;
    }
    public void RestartSelection()
    {
        foreach (var animator in ButtonAnimators)
        {
            UsefullMethods.ResetAllTriggersInAnimator(animator);
            animator.SetBool(isHighlighted, false);
        }

        if(currentSelectedIndex == 0) { ButtonAnimators[0].SetBool(isHighlighted,true); }
        else { SwitchSelectedButtons(ButtonAnimators[currentSelectedIndex], ButtonAnimators[0]); }
        currentSelectedIndex = 0;
    }
    void selectUpperButton()
    {
        if (!isReadingInput) { return; }

        if (currentSelectedIndex == 0)
        {
            currentSelectedIndex = ButtonAnimators.Count - 1;
            SwitchSelectedButtons(ButtonAnimators[0], ButtonAnimators[currentSelectedIndex]);
        }
        else
        {
            SwitchSelectedButtons(ButtonAnimators[currentSelectedIndex], ButtonAnimators[currentSelectedIndex - 1]);
            currentSelectedIndex--;
        }
    }
    void selectLowerButton()
    {
        if (!isReadingInput) { return; }

        if (currentSelectedIndex == ButtonAnimators.Count - 1)
        {
            currentSelectedIndex = 0;
            SwitchSelectedButtons(ButtonAnimators[ButtonAnimators.Count - 1], ButtonAnimators[0]);
        }
        else
        {
            SwitchSelectedButtons(ButtonAnimators[currentSelectedIndex], ButtonAnimators[currentSelectedIndex + 1]);
            currentSelectedIndex++;
        }
    }
    void SelectCurrentHighlight()
    {
        if (!isReadingInput) { return; }

        ButtonAnimators[currentSelectedIndex].SetTrigger(Pressed);
        Debug.Log("pressed");
    }

    void SwitchSelectedButtons(Animator oldSelected,  Animator newSelected)
    {
        oldSelected.SetBool(isHighlighted,false);
        newSelected.SetBool(isHighlighted,true);
        Debug.Log("Switched");
    }
}
