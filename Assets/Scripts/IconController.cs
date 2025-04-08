using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite handSprite;
    [SerializeField] Sprite eyeSprite;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] string motherTag = "Mother";
    [SerializeField] GameObject player;
    [SerializeField] Volume volume;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float maxIntensity = 0.8f;
    Vignette vignette;
    GameObject mother;

    void Start()
    {
        if (iconImage != null)
        {
            iconImage.sprite = handSprite;
        }

        mother = GameObject.FindGameObjectWithTag(motherTag);
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
        if (mother == null || iconImage == null) return;

        float distanceToMother = Vector3.Distance(player.transform.position, mother.transform.position);

        if (distanceToMother <= detectionRadius)
        {
            iconImage.sprite = eyeSprite;
        }
        else
        {
            iconImage.sprite = handSprite;
        }

        if (mother == null || vignette == null) return;

        float intensity = 0f;
        if (distanceToMother <= maxDistance)
        {
            intensity = Mathf.Lerp(maxIntensity, 0f, distanceToMother / maxDistance);
        }

        vignette.intensity.value = intensity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}