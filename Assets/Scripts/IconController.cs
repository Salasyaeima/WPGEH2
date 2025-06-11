using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Cinemachine;

public class IconController : MonoBehaviour
{
    [SerializeField] Image iconEye;
    [SerializeField] float panicRadius = 8f;
    [SerializeField] float playerVisionRadius = 14f;
    [SerializeField] float playerAlertRadius = 20f;
    [SerializeField] string motherTag = "Mother";
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] GameObject player;
    [SerializeField] Volume volume;
    [SerializeField] float maxIntensity = 0.8f;
    [SerializeField] float vignetteTransitionSpeed = 2f;
    [SerializeField] float shakeAmplitudo = 2f;
    [SerializeField] float shakeFrequency = 2f;
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] string heartRate = "detakJantung";
    CinemachineBasicMultiChannelPerlin noise;
    Vignette vignette;
    GameObject mother;
    bool heartbeatPlaying = false;

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
            vignette.intensity.value = 0.3f;

        }
        else
        {
            Debug.LogWarning("Volume atau Vignette tidak ditemukan!");
        }
    }

    void Update()
    {
        if (mother == null || vignette == null) return;

        float distanceToMother = Vector3.Distance(player.transform.position, mother.transform.position);
        float motherDetectionRadius = mother.GetComponent<AreaCheck>().detectionRadius;
        bool isMotherVisible = false;
        bool isPlayerVisibleToMother = false;

        if (distanceToMother <= playerAlertRadius)
        {
            Vector3 directionToPlayer = (player.transform.position - mother.transform.position).normalized;
            RaycastHit hit;
            if (!Physics.Raycast(mother.transform.position, directionToPlayer, out hit, motherDetectionRadius, obstacleLayer))
            {
                iconEye.enabled = true;
            }
        }
        else
        {
            iconEye.enabled = false;
        }

        if (distanceToMother <= playerAlertRadius)
        {
            CameraShaking();
        }
        else
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }

        if (distanceToMother <= playerVisionRadius)
        {
            isMotherVisible = true;
        }
        else
        {
            isMotherVisible = false;
        }

        if (distanceToMother <= playerAlertRadius)
        {
            if (!heartbeatPlaying)
            {
                AudioManager.instance.PlayLoopingSFX(heartRate);
                heartbeatPlaying = true;
            }
        }
        else
        {
            if (heartbeatPlaying)
            {
                AudioManager.instance.StopLoopingSFXWithFade(heartRate, 5f);
                heartbeatPlaying = false;
            }
        }


        LineOfSight lineOfSight = mother.GetComponent<LineOfSight>();
        if (lineOfSight != null)
        {
            GameObject detected = lineOfSight.CheckInSight(player);
            if (detected != null && detected == player)
            {
                isPlayerVisibleToMother = true;
            }
            else
            {
                isPlayerVisibleToMother = false;
            }
        }
        else
        {
            Debug.LogWarning("Komponen AreaCheck pada Mother tidak ditemukan!");
        }

        float targetIntensity = (isMotherVisible || isPlayerVisibleToMother) ? maxIntensity : 0.3f;
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * vignetteTransitionSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(player.transform.position, playerAlertRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, playerVisionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, panicRadius);
    }

    void CameraShaking()
    {
        noise.m_AmplitudeGain = shakeAmplitudo;
        noise.m_FrequencyGain = shakeFrequency;
    }
}