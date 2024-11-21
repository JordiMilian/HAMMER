using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XP_CounterDisplay : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] float timeBeforeDisappear, timeToDisappear;
    [SerializeField] TextMeshProUGUI displayText;
    Coroutine fadeCoroutine;

    private void OnEnable()
    {
        gameState.OnXpPointsSet += onUpdatedCounter;
        onUpdatedCounter(gameState.XpPoints);
    }
    private void OnDisable()
    {
        gameState.OnXpPointsSet -= onUpdatedCounter;
    }
    void onUpdatedCounter(int newAmount)
    {
        if(fadeCoroutine != null) { StopCoroutine(fadeCoroutine); }

        displayText.text = newAmount.ToString();
        fadeCoroutine = StartCoroutine(showDisplay());
    }
    IEnumerator showDisplay()
    {
        Color originalColor = displayText.color;
        displayText.color = new Color(originalColor.r,originalColor.g,originalColor.b,1);
        yield return new WaitForSecondsRealtime(timeBeforeDisappear);

        float timer = 0;
        while (timer < timeToDisappear)
        {
            timer += Time.deltaTime;
            displayText.color = new Color(originalColor.r, originalColor.g,originalColor.b,1 - timer/timeBeforeDisappear);
            yield return null;
        }
        displayText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
