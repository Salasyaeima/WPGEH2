using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using System.Collections;

public class FadeInAndChangeScene : MonoBehaviour
{
    [SerializeField] private Image targetImage; 
    [SerializeField] private float fadeDuration = 2f; 
    [SerializeField] private string nextSceneName = "NextScene"; 

    void Start()
    {
        if (targetImage != null)
        {
            Color imageColor = targetImage.color;
            imageColor.a = 0f;
            targetImage.color = imageColor;
        }

        StartCoroutine(FadeInAndSwitchScene());
    }

    IEnumerator FadeInAndSwitchScene()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            
            if (targetImage != null)
            {
                Color imageColor = targetImage.color;
                imageColor.a = alpha;
                targetImage.color = imageColor;
            }
            
            yield return null; 
        }

        if (targetImage != null)
        {
            Color imageColor = targetImage.color;
            imageColor.a = 1f;
            targetImage.color = imageColor;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}