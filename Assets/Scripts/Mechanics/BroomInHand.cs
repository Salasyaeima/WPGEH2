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
    Quaternion originalRotation;
    Dictionary<Collider, float> dirtTimers = new Dictionary<Collider, float>();

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float rotationAngle = Mathf.Sin(Time.time * sweepRotationSpeed) * 5f;
            transform.localRotation = originalRotation * Quaternion.Euler(0, rotationAngle, 0);
        }
        else
        {
            transform.localRotation = originalRotation;
        }

        bool isSweeping = Input.GetMouseButton(0);
        if (broomAnimator != null)
        {
            broomAnimator.SetBool("IsSweeping", isSweeping);
        }

        if (isSweeping && sweepPoint != null)
        {
            Collider[] hits = Physics.OverlapSphere(sweepPoint.position, sweepRadius, dirtLayer);
            foreach (Collider dirtCollider in hits)
            {
                if (dirtCollider.CompareTag("Dirt"))
                {
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
            }

            List<Collider> toRemove = new List<Collider>();
            foreach (var dirt in dirtTimers.Keys)
            {
                if (!System.Array.Exists(hits, hit => hit == dirt))
                {
                    toRemove.Add(dirt);
                }
            }
            foreach (var dirt in toRemove)
            {
                dirtTimers.Remove(dirt);
            }
        }
        else
        {
            dirtTimers.Clear();
        }
    }

    public Transform GetSweepPoint() => sweepPoint;
    public float GetSweepRadius() => sweepRadius;
    public LayerMask GetDirtLayer() => dirtLayer;
    public Animator GetBroomAnimator() => broomAnimator;
}