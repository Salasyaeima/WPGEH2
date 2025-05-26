using Unity.VisualScripting;
using UnityEngine;

public class Broom : Interactable, ITaskProvider
{
    [SerializeField] GameObject broomInHand;
    [SerializeField] GameObject broomInRoom;
    [SerializeField] Transform playerHand;
    [SerializeField] GameObject roomBroom;
    [SerializeField] string taskName = "Bersihkan Lantai";
    TaskManager taskManager;
    Room room;
    public bool isHeld = false;

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
    }

    public override void Interact()
    {
        if (!isHeld && broomInHand != null && broomInRoom != null && playerHand != null)
        {
            broomInHand.SetActive(true);
            broomInRoom.SetActive(false);
            broomInRoom.transform.SetParent(playerHand);
            broomInRoom.transform.localPosition = Vector3.zero;
            broomInRoom.transform.localRotation = Quaternion.identity;
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
            broomInRoom.transform.SetParent(null);
            isHeld = false;
            PlayerInteractions.heldItem = null;
        }
    }

    public override string Description()
    {
        return isHeld ? "Tekan {E} untuk meletakkan sapu" : "Tekan {E} untuk mengambil sapu";
    }

    public string GetTaskName()
    {
        return room != null ? $"{taskName}" : taskName;
    }

    public Transform GetSweepPoint() => broomInHand.GetComponent<BroomInHand>().GetSweepPoint();
    public float GetSweepRadius() => broomInHand.GetComponent<BroomInHand>().GetSweepRadius();
    public LayerMask GetDirtLayer() => broomInHand.GetComponent<BroomInHand>().GetDirtLayer();
    public Animator GetBroomAnimator() => broomInHand.GetComponent<BroomInHand>().GetBroomAnimator();
}