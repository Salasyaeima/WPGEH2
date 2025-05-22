using Unity.VisualScripting;
using UnityEngine;

public class Broom : Interactable, ITaskProvider
{
    [SerializeField] GameObject broomInHand;
    [SerializeField] GameObject broomInRoom;
    [SerializeField] Transform playerHand;
    [SerializeField] Transform sweepPoint;
    [SerializeField] GameObject roomBroom;
    [SerializeField] float sweepRadius = 0.5f;
    [SerializeField] LayerMask dirtLayer;
    [SerializeField] Animator broomAnimator;
    [SerializeField] string taskName = "Bersihkan Lantai";
    [SerializeField] float sweepRotationSpeed = 30f;
    TaskManager taskManager;
    Room room;
    bool isHeld = false;
    Quaternion originalRotation;

    void Start()
    {
        taskManager = TaskManager.Instance;
        room = GetComponentInParent<Room>();
        if (room == null)
        {
            Debug.LogWarning($"{name} tidak menemukan Room di parent!");
        }

        if (taskManager != null)
        {
            taskManager.RegisterTask(taskName, this, room);
        }

        if (broomInHand != null)
        {
            originalRotation = broomInHand.transform.localRotation;
        }
    }

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
        else if (isHeld)
        {
            Drop();
        }
    }

    public override void Drop()
    {
        if (isHeld && broomInHand != null && broomInRoom != null)
        {
            broomInHand.SetActive(false);
            broomInRoom.SetActive(true);
            broomInRoom.transform.SetParent(roomBroom.transform);
            transform.SetParent(null);
            isHeld = false;
            PlayerInteractions.heldItem = null;
            if (broomInHand != null)
            {
                broomInHand.transform.localRotation = originalRotation;
            }
        }
    }

    public override string Description()
    {
        if (!isHeld)
        {
            return "Press {E} to pick up the broom";
        }
        return "Press {E} to drop the broom";
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

    public string GetTaskName()
    {
        if (room != null)
        {
            return $"{taskName}";
        }
        return taskName;
    }

    void Update()
    {
        if (isHeld)
        {
            if (Input.GetMouseButton(0) && broomInHand != null)
            {
                float rotationAngle = Mathf.Sin(Time.time * sweepRotationSpeed) * 10f;
                broomInHand.transform.localRotation = originalRotation * Quaternion.Euler(0, rotationAngle, 0);
            }
            else if (broomInHand != null)
            {
                broomInHand.transform.localRotation = originalRotation;
            }
        }
    }
}