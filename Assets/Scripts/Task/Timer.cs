using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI completionTimeText;
    float time = 0f;
    float completionTime = 0f;
    bool isRunning = true;
    bool isCompleted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isRunning && !isCompleted)
        {
            time += Time.deltaTime;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            float milliseconds = (time % 1) * 100;

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timerString;
        }
    }

    public void CompleteGame()
    {
        if (!isCompleted)
        {
            isRunning = false;
            isCompleted = true;
            completionTime = time;

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            float milliseconds = (time % 1) * 100;

            string completionString = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (completionTimeText != null)
            {
                completionTimeText.text = $"Waktu Bermain : {completionString}";
            }
        }
    }

}
