using Unity.VisualScripting;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    private AudioSource dropSfx;
    void Start()
    {
        dropSfx = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        gameObject.layer = LayerMask.NameToLayer("SoundedObject");
        dropSfx.Play();
    }

    void OnCollisionExit(Collision other)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
