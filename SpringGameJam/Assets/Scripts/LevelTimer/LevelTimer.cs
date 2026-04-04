using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    private float time;
    private bool isRunning = true;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        if (!isRunning)
            return;

        time += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime()
    {
        return time;
    }
}
