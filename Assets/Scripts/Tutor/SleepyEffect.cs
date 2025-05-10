using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SleepyBlinkEffect : MonoBehaviour
{
    [SerializeField] Image eyelidTop;
    [SerializeField] Image eyelidBottom;
    [SerializeField] Image darkOverlay;
    [SerializeField] PostProcessVolume postProcessVolume;
    [SerializeField] float delayBeforeFade = 5f;
    [SerializeField] float fadeToBlackDuration = 2f;
    [SerializeField] float resetDelay = 3f;
    Vignette vignette;
    DepthOfField depthOfField;
    bool isBlinking = true;
    float blinkTimer = 0f;
    float sleepTimer = 0f;
    float fadeTimer = 0f;
    float resetTimer = 0f;
    bool isFadingToBlack = false;
    bool isFullyBlack = false;
    bool startSleepCountdown = false;
    bool startResetCountdown = false;

    float maxEyelidDistance;
    [SerializeField] float blinkSpeed = 1f;

    void Start()
    {
        isBlinking = true;
        maxEyelidDistance = Screen.height / 2f;


        if (eyelidTop != null)
        {
            eyelidTop.enabled = false;
            eyelidTop.rectTransform.anchoredPosition = new Vector2(0, maxEyelidDistance);
        }
        ;
        if (eyelidBottom != null)
        {
            eyelidBottom.enabled = false;
            eyelidBottom.rectTransform.anchoredPosition = new Vector2(0, -maxEyelidDistance);
        }
        if (darkOverlay != null)
        {
            darkOverlay.enabled = false;
            Color c = darkOverlay.color;
            c.a = 0f;
            darkOverlay.color = c;
        }

        if (postProcessVolume)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
            postProcessVolume.profile.TryGetSettings(out depthOfField);
            if (vignette)
            {
                vignette.intensity.value = 0f;
            }
            else
            {
                Debug.LogWarning("Vignette effect not found in PostProcessVolume profile.");
            }

            if (depthOfField)
            {
                depthOfField.aperture.value = 32f;
                depthOfField.focusDistance.value = 10f;
                depthOfField.active = true;
            }
            else
            {
                Debug.LogWarning("DepthOfField effect not found in PostProcessVolume profile.");
            }
        }
    }

    public void StartFadeToBlack()
    {
        startSleepCountdown = true;
        isBlinking = true;
        sleepTimer = 0f;
    }

    void Update()
    {
        if (startSleepCountdown)
        {
            sleepTimer += Time.deltaTime;
            if (sleepTimer >= delayBeforeFade && !isFadingToBlack)
            {
                isFadingToBlack = true;
                fadeTimer = 0f;
            }
        }

        if (isFadingToBlack)
        {
            fadeTimer += Time.deltaTime;
            float fadeAmount = fadeTimer / fadeToBlackDuration;

            if (darkOverlay)
            {
                darkOverlay.enabled = true;
                Color c = darkOverlay.color;
                c.a = Mathf.Lerp(0f, 1f, fadeAmount);
                darkOverlay.color = c;
            }

            if (fadeTimer >= fadeToBlackDuration)
            {
                isFadingToBlack = false;
                isFullyBlack = true;
                startResetCountdown = true;
                if (eyelidTop)
                {
                    eyelidTop.enabled = false;
                }
                if (eyelidBottom)
                {
                    eyelidBottom.enabled = false;
                }
                if (darkOverlay)
                {
                    Color c = darkOverlay.color;
                    c.a = 1f;
                    darkOverlay.color = c;
                }
                Debug.Log("SleepyBlinkEffect: Screen is fully black");
            }
        }

        if (startResetCountdown)
        {
            resetTimer += Time.deltaTime;
            Debug.Log($"SleepyBlinkEffect: resetTimer: {resetTimer}/{resetDelay}");
            if (resetTimer >= resetDelay)
            {
                isBlinking = false;
                isFullyBlack = false;
                startResetCountdown = false;
                startSleepCountdown = false;
                ResetEffects();
                Debug.Log("SleepyBlinkEffect: Effects reset, mata terbuka");
            }
        }

        if (isBlinking && !isFadingToBlack && !isFullyBlack)
        {
            blinkTimer += Time.deltaTime * blinkSpeed;
            float blinkAmount = Mathf.PingPong(blinkTimer, 1f);

            if (eyelidTop)
            {
                eyelidTop.enabled = true;
                float topY = Mathf.Lerp(maxEyelidDistance, 0f, blinkAmount);
                eyelidTop.rectTransform.anchoredPosition = new Vector2(0, topY);
            }
            if (eyelidBottom)
            {
                eyelidBottom.enabled = true;
                float bottomY = Mathf.Lerp(-maxEyelidDistance, 0f, blinkAmount);
                eyelidBottom.rectTransform.anchoredPosition = new Vector2(0, bottomY);
            }

            if (darkOverlay)
            {
                darkOverlay.enabled = true;
                Color c = darkOverlay.color;
                c.a = Mathf.Lerp(0f, 0.9f, blinkAmount);
                darkOverlay.color = c;
            }

            if (vignette)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.6f, Time.deltaTime * 2f);
            }

            if (depthOfField)
            {
                depthOfField.aperture.value = Mathf.Lerp(depthOfField.aperture.value, 1.4f, Time.deltaTime * 2f);
                depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, 5f, Time.deltaTime * 2f);
            }
        }
        else if (!isFullyBlack)
        {
            if (eyelidTop)
            {
                float topY = Mathf.Lerp(eyelidTop.rectTransform.anchoredPosition.y, maxEyelidDistance, Time.deltaTime * 5f);
                eyelidTop.rectTransform.anchoredPosition = new Vector2(0, topY);
            }
            if (eyelidBottom)
            {
                float bottomY = Mathf.Lerp(eyelidBottom.rectTransform.anchoredPosition.y, -maxEyelidDistance, Time.deltaTime * 5f);
                eyelidBottom.rectTransform.anchoredPosition = new Vector2(0, bottomY);
            }
            if (darkOverlay)
            {
                Color c = darkOverlay.color;
                c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * 5f);
                darkOverlay.color = c;
            }

            if (vignette)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * 5f);
            }
            if (depthOfField)
            {
                depthOfField.aperture.value = Mathf.Lerp(depthOfField.aperture.value, 32f, Time.deltaTime * 5f);
                depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, 10f, Time.deltaTime * 5f);
            }
        }

        if (isFullyBlack)
        {
            if (darkOverlay)
            {
                Color c = darkOverlay.color;
                c.a = 1f;
                darkOverlay.color = c;
            }
        }
    }

    void ResetEffects()
    {
        isBlinking = false;
        isFullyBlack = false;
        sleepTimer = 0f;
        fadeTimer = 0f;
        blinkTimer = 0f;
        resetTimer = 0f;

        if (eyelidTop)
        {
            eyelidTop.enabled = false;
            eyelidTop.rectTransform.anchoredPosition = new Vector2(0, maxEyelidDistance);
        }
        if (eyelidBottom)
        {
            eyelidBottom.enabled = false;
            eyelidBottom.rectTransform.anchoredPosition = new Vector2(0, -maxEyelidDistance);
        }
        if (darkOverlay)
        {
            darkOverlay.enabled = false;
            Color c = darkOverlay.color;
            c.a = 0f;
            darkOverlay.color = c;
        }

        if (vignette)
        {
            vignette.intensity.value = 0f;
        }
        if (depthOfField)
        {
            depthOfField.aperture.value = 32f;
            depthOfField.focusDistance.value = 10f;
        }

        gameObject.SetActive(false);
    }
}
