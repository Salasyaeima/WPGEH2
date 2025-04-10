using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Cinemachine;

public class IconController : MonoBehaviour
{
    [SerializeField] Image iconEye;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] string motherTag = "Mother";
    [SerializeField] GameObject player;
    [SerializeField] Volume volume;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float maxIntensity = 0.8f;
    [SerializeField] float shakeAmplitudo = 2f;
    [SerializeField] float shakeFrequency = 2f;
    [SerializeField] CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noise;
    Vignette vignette;
    GameObject mother;



    void Start()
    {
        iconEye.enabled = false;

        mother = GameObject.FindGameObjectWithTag(motherTag);
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (mother == null)
        {
            Debug.LogWarning("Tidak ada GameObject dengan tag 'Mother' di scene!");
        }

        if (volume != null && volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
        else
        {
            Debug.LogWarning("Volume atau Vignette tidak ditemukan!");
        }
    }

    void Update()
    {
        float distanceToMother = Vector3.Distance(player.transform.position, mother.transform.position);
        if (distanceToMother <= detectionRadius)
        {
            iconEye.enabled = true;
        }
        else
        {
            iconEye.enabled = false;
            
        }


        if (mother == null || vignette == null) return;

        float intensity = 0f;
        if (distanceToMother <= maxDistance)
        {
            intensity = Mathf.Lerp(maxIntensity, 0f, distanceToMother / maxDistance);
            CameraShaking();
        }
        else
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }

        vignette.intensity.value = intensity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void CameraShaking() 
    {
        noise.m_AmplitudeGain = shakeAmplitudo;
        noise.m_FrequencyGain = shakeFrequency;
    }
}