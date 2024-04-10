using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayUI_Effects : MonoBehaviour
{
    [SerializeField] VisualEffect bloodEffect;
    [SerializeField] PauseGame pauseGame;
    public enum ButtonType
    {
        unpause, restartLevel, die, exit
    }
    [SerializeField] ButtonType buttonType;
    public void EV_playEffect()
    {
        bloodEffect.Play();
    }
    public void EV_PressedAction()
    {
        switch(buttonType)
        {
            case ButtonType.unpause:
                pauseGame.Unpause();
                break;
            case ButtonType.restartLevel:
                pauseGame.restartLevel();
                break;
            case ButtonType.die:
                pauseGame.die();
                break;
            case ButtonType.exit:
                pauseGame.exit();
                break;
        }
    }

}
