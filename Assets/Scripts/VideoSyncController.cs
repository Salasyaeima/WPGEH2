using UnityEngine;
using UnityEngine.Video;
public class VideoSyncController : MonoBehaviour
{
    public VideoPlayer videoPlayer1;
    public GameObject screen2;
    public GameObject hpDummy;
    Vector3 position;
    Quaternion rotation;

    void Start()
    {
        videoPlayer1.Play();
        screen2.SetActive(false);
        position = hpDummy.transform.position;
        rotation = hpDummy.transform.rotation;
    }

    public void ActivateScreen2()
    {
        hpDummy.transform.position = screen2.transform.position;
        hpDummy.transform.rotation = screen2.transform.rotation;
    }
    public void ActivateScreen1()
    {
        videoPlayer1.SetDirectAudioVolume(0, 0.002f);
        hpDummy.transform.position = position;
        hpDummy.transform.rotation = rotation;
    }
    
    public void UnMuteVideo()
    {
        videoPlayer1.SetDirectAudioMute(0, false);
    }
    public void MuteVideo()
    {
        videoPlayer1.SetDirectAudioMute(0, true);
    }
}