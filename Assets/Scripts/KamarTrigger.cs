using UnityEngine;

public class KamarTrigger : MonoBehaviour
{
    [SerializeField] TextDialogChild textDialog;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(textDialog.ResumeInstruksi());
            textDialog.intruksi.enabled = false;
            textDialog.playerInteractions.canInteract = false;
            textDialog.windowQuest.gameObject.SetActive(false);
        }
    }
}
