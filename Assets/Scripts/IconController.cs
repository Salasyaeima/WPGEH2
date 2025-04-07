using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Sprite handSprite;
    [SerializeField] Sprite eyeSprite;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] string motherTag = "Mother";
    [SerializeField] GameObject player;
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}