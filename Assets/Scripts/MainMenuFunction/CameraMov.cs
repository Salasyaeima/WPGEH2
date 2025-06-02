using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    [SerializeField] public List<Camera> cameras;
    [SerializeField] float intervalSwitch = 10f;
    [SerializeField] private int cameraIndex = 0;
    private float timer;
    void Start()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].gameObject.SetActive(i == cameraIndex);
            }
        }
        timer = intervalSwitch;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            switchCamera();
            timer = intervalSwitch;
        }
    }

    void switchCamera()
    {
        if (cameras.Count == 0) return;

        if (cameras[cameraIndex] != null)
        {
            cameras[cameraIndex].gameObject.SetActive(false);
        }

        cameraIndex = (cameraIndex + 1) % cameras.Count;

        if (cameras[cameraIndex] != null)
        {
            cameras[cameraIndex].gameObject.SetActive(true);
        }
    }
}
