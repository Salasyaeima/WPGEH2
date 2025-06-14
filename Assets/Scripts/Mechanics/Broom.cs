using Unity.VisualScripting;
using UnityEngine;

public class Broom : Interactable, ITaskProvider
{
    [SerializeField] GameObject broomInHand;
    [SerializeField] GameObject broomInRoom;
    [SerializeField] Transform playerHand;
    [SerializeField] GameObject roomBroom;
    [SerializeField] string taskName = "Bersihkan Lantai";
    [SerializeField] string interectionSFXName = "Ambilbarang";
    [SerializeField] string dorpSFXName = "LepasBarang";
    [SerializeField] string sweepSFXName = "sweep_sfx";
    TaskManager taskManager;
    Room room;
    public bool isHeld = false;

    void Start()
    {
        GetTaskNameField();
        taskManager = TaskManager.Instance;
        room = GetComponentInParent<Room>();
        if (room == null)
        {
            Debug.LogWarning($"{name} tidak menemukan Room di parent!");
        }

        if (taskManager != null)
        {
            taskManager.RegisterTask(GetTaskName(), this, room);
        }
    }

    public override void Interact()
    {
        if (!isHeld && broomInHand != null && broomInRoom != null && playerHand != null)
        {
            AudioManager.instance.PlaySFX(interectionSFXName, 0.3f);
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
            AudioManager.instance.StopLoopingSFX(sweepSFXName);
            AudioManager.instance.PlaySFX(dorpSFXName, 0.3f);
            broomInHand.SetActive(false);
            broomInRoom.SetActive(true);
            broomInRoom.transform.SetParent(null);
            isHeld = false;
            PlayerInteractions.heldItem = null;
        }
    }

    public override string Description()
    {
        string text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
        ? (isHeld ? "Press {E} to drop" : "Press {E} to pick up")
        : (isHeld ? "Tekan {E} untuk menjatuhkan" : "Tekan {E} untuk mengambil");
        return text;
    }

    public string GetTaskName()
    {
        string baseTaskName = GetTaskNameField();
        return room != null ? $"{baseTaskName} {room.GetLocalizedRoomName()}" : baseTaskName;
    }
    public Animator GetBroomAnimator() => broomInHand.GetComponent<BroomInHand>().GetBroomAnimator();
    public string GetTaskNameField()
    {
        return LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.Indonesian
            ? "Bersihkan Lantai"
            : "Clean the Floor";
    }
}