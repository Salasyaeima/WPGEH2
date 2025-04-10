using UnityEngine;
using TMPro;

public class UIGameCompleted : MonoBehaviour
{
    float gameTime = 0;
    public TMP_Text onGameTimes;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        Debug.Log("Total waktu bermain: "+ gameTime);
    }
}
