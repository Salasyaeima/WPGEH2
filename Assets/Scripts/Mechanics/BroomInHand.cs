using UnityEngine;
using System.Collections.Generic;

public class BroomInHand : MonoBehaviour
{
    [SerializeField] Transform sweepPoint;
    [SerializeField] float sweepRadius = 0.5f;
    [SerializeField] LayerMask dirtLayer;
    [SerializeField] Animator broomAnimator;
    [SerializeField] float sweepRotationSpeed = 30f;
    [SerializeField] float sweepTimeRequired = 2f;
    [SerializeField] string sweepSFXName = "sweep_sfx";
    Quaternion originalRotation;
    Dictionary<Collider, float> dirtTimers = new Dictionary<Collider, float>();
    bool wasSweeping = false;
    bool isSweepingSFXPlaying = false;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        bool isSweeping = Input.GetMouseButton(0);

        if (isSweeping)
        {
            float rotationAngle = Mathf.Sin(Time.time * sweepRotationSpeed) * 5f;
            transform.localRotation = originalRotation * Quaternion.Euler(0, rotationAngle, 0);
        }
        else
        {
            transform.localRotation = originalRotation;
        }

        if (broomAnimator != null)
        {
            broomAnimator.SetBool("IsSweeping", isSweeping);
        }

        if (isSweeping && !isSweepingSFXPlaying)
        {
            AudioManager.instance.PlayLoopingSFX(sweepSFXName);
            isSweepingSFXPlaying = true;
        }
        else if (!isSweeping && wasSweeping)
        {
            AudioManager.instance.StopLoopingSFX(sweepSFXName);
            isSweepingSFXPlaying = false;
        }
        wasSweeping = isSweeping;

        if (!isSweeping)
        {
            dirtTimers.Clear();
        }
    }
    public void HandleDirt(Collider dirtCollider)
    {
        if (!dirtCollider.CompareTag("Dirt") || !Input.GetMouseButton(0))
            return;

        if (!dirtTimers.ContainsKey(dirtCollider))
        {
            dirtTimers[dirtCollider] = 0f;
        }

        dirtTimers[dirtCollider] += Time.deltaTime;

        if (dirtTimers[dirtCollider] >= sweepTimeRequired)
        {
            Destroy(dirtCollider.gameObject);
            dirtTimers.Remove(dirtCollider);
        }
    }

    public Animator GetBroomAnimator() => broomAnimator;
}