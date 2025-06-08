using UnityEngine;
using UnityEngine.Playables;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private AudioClip leftFootClip;  
    [SerializeField] private AudioClip rightFootClip; 
    [SerializeField] private float stepInterval = 0.5f; 

    private AudioSource audioSource;
    private bool isPlaying = false; 
    private bool isLeftStep = true; 
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
    }

    void PlayFootstep()
    {
        AudioClip clipToPlay = isLeftStep ? leftFootClip : rightFootClip;

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }

        isLeftStep = !isLeftStep;
    }

    public void StartFootsteps()
    {
        isPlaying = true;
        stepTimer = 0f; 
    }

    public void StopFootsteps()
    {
        isPlaying = false;
        audioSource.Stop();
        stepTimer = 0f; 
    }
}