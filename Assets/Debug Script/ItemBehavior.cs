using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioClip dropSfx;
    private AudioSource audioPlayer;
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        gameObject.layer = LayerMask.NameToLayer("SoundedObject");
        audioPlayer.PlayOneShot(dropSfx);
    }

    void OnCollisionExit(Collision other)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
