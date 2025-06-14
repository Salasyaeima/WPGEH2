using System.Collections;
using TMPro;
using UnityEngine;

public class KamarTrigger : MonoBehaviour
{
    [SerializeField] TextDialogChild textDialog;
    [SerializeField] GameObject boxPenghalang;
    [SerializeField] TextMeshProUGUI intruksi;
    bool hasTriggered = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(textDialog.ResumeInstruksi());
            textDialog.intruksi.enabled = false;
            textDialog.playerInteractions.canInteract = false;
            textDialog.windowQuest.gameObject.SetActive(false);
            boxPenghalang.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mother"))
        {
            StartCoroutine(ResumeTextDialog());
        }
    }

    IEnumerator ResumeTextDialog()
    {
        yield return new WaitForSeconds(3f);
        textDialog.ResumeDisplayingText();
        yield return new WaitForSeconds(3f);
        LoadingScreen.Instance.SwitchToScene("Rooms");
    }
}
