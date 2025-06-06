using UnityEngine;
using UnityEngine.Video;

public class ProximityVideoAudio : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public Transform player;
    public float maxDistance = 10f;
    public TargetWalk targetWalk;
    float maxVolume = 0.01f;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void Update()
    {
        if (audioSource == null || player == null || targetWalk == null) return;

        if (!targetWalk.isMuted && targetWalk.GetCurrentWaypoint() >= 8)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= maxDistance)
            {
                audioSource.mute = false;
                audioSource.volume = Mathf.Clamp(maxVolume * (1 - distance / maxDistance), 0f, maxVolume);
            }
            else
            {
                audioSource.mute = true;
                audioSource.volume = 0f;
            }
        }
        else
        {
            audioSource.mute = true;
            audioSource.volume = 0f;
        }
    }
}