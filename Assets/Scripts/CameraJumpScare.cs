using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CameraJumpScare : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject blackBackground;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 0.1f;
    [SerializeField] Volume postProcessVolume;
    [SerializeField] Sight sight;
    [SerializeField] AudioSource jumpScareAudioSource;
    [SerializeField] AudioClip soundScare;
    Vignette vignette;
    ChromaticAberration chromaticAberration;
    Coroutine shakeCoroutine;
    bool hasStarted = false;


    void Start()
    {
        blackBackground.SetActive(false);
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0.3f;
        }

        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out chromaticAberration))
        {
            chromaticAberration.intensity.value = 0f;
        }
    }

    public void StartShakeAndZoom()
    {
        if (hasStarted) return;
        hasStarted = true;

        if (jumpScareAudioSource != null && soundScare != null)
        {
            jumpScareAudioSource.PlayOneShot(soundScare);
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip untuk jumpscare tidak diatur!");
        }

        shakeCoroutine = StartCoroutine(Shake());
        StartCoroutine(VignetteFlash(0.50f, 1.5f));
        StartCoroutine(ChromaticAberrationEffect());
        StartCoroutine(ActiveBackgroundBlack());
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }

    IEnumerator VignetteFlash(float targetIntensity, float duration)
    {
        if (vignette == null) yield break;
        float elapsed = 0f;
        float startIntensity = vignette.intensity.value;

        while (elapsed < duration)
        {
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }

    IEnumerator ChromaticAberrationEffect()
    {
        if (chromaticAberration == null) yield break;

        chromaticAberration.intensity.value = 1f;
        yield return new WaitForSeconds(1.3f);
        chromaticAberration.intensity.value = 0f;
    }

    IEnumerator ActiveBackgroundBlack()
    {
        yield return new WaitForSeconds(1.8f);
        blackBackground.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        blackBackground.SetActive(false);
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        if (vignette != null)
            vignette.intensity.value = 0f;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 0f;


        Cursor.lockState = CursorLockMode.None;
        sight.enabled = false;
        Time.timeScale = 0f;
        Cursor.visible = true;
        TaskManager.Instance.panelResult.SetActive(true);
        if (Timer.Instance != null)
        {
            Timer.Instance.CompleteGame();
        }
    }


}
