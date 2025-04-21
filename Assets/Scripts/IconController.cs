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
        if (mother == null || vignette == null) return;

        float distanceToMother = Vector3.Distance(player.transform.position, mother.transform.position);
        bool isMotherVisible = false;
        bool isPlayerVisibleToMother = false;

        if (distanceToMother <= playerAlertRadius)
        {
            iconEye.enabled = true;
        }
        else
        {
            iconEye.enabled = false;
        }

        if (distanceToMother <= panicRadius)
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
            Vector3 directionToMother = (mother.transform.position - player.transform.position).normalized;
            RaycastHit hit;
            if (!Physics.Raycast(player.transform.position, directionToMother, out hit, playerVisionRadius, obstacleLayer))
            {
                isMotherVisible = true;
            }
        }

        if (mother.GetComponent<AreaCheck>() != null)
        {
            float motherDetectionRadius = mother.GetComponent<AreaCheck>().detectionRadius;
            if (distanceToMother <= motherDetectionRadius)
            {
                Vector3 directionToPlayer = (player.transform.position - mother.transform.position).normalized;
                RaycastHit hit;
                if (!Physics.Raycast(mother.transform.position, directionToPlayer, out hit, motherDetectionRadius, obstacleLayer))
                {
                    isPlayerVisibleToMother = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Komponen AreaCheck pada Mother tidak ditemukan!");
        }

        float intensity = 0f;
        if (isMotherVisible || isPlayerVisibleToMother)
        {
            intensity = Mathf.Lerp(maxIntensity, 0f, distanceToMother / playerVisionRadius);
        }

        vignette.intensity.value = intensity;
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