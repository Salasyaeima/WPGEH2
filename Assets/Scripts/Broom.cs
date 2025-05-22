using UnityEngine;

public class Broom : Interactable
{
    [SerializeField] GameObject broomInHand;
    [SerializeField] GameObject broomInRoom;
    [SerializeField] Transform playerHand;
    [SerializeField] Transform sweepPoint;
    [SerializeField] float sweepRadius = 0.5f;
    [SerializeField] LayerMask dirtLayer;
    [SerializeField] Animator broomAnimator;
    bool isHeld = false;

    public override void Interact()
    {
        if (!isHeld && broomInHand != null && broomInRoom != null && playerHand != null)
        {
            broomInHand.SetActive(true);
            broomInRoom.SetActive(false);
            transform.SetParent(playerHand);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            isHeld = true;
            PlayerInteractions.heldItem = this;
        }
    }

    public override void Drop()
    {
        if (isHeld && broomInHand != null && broomInRoom != null)
        {
            broomInHand.SetActive(false);
            broomInRoom.SetActive(true);
            broomInRoom.transform.SetParent(null);
            transform.SetParent(null);
            isHeld = false;
            PlayerInteractions.heldItem = null;
        }
    }

    public override string Description()
    {
        if (!isHeld)
        {
            return "Press E to pick up the broom";
        }
        return "Press E to drop the broom";
    }

    public Transform GetSweepPoint()
    {
        return sweepPoint;
    }

    public float GetSweepRadius()
    {
        return sweepRadius;
    }

    public LayerMask GetDirtLayer()
    {
        return dirtLayer;
    }

    public Animator GetBroomAnimator()
    {
        return broomAnimator;
    }
}