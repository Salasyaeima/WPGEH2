using UnityEngine;

public class KamarTrigger : MonoBehaviour
{
    [SerializeField] TextDialogChild textDialog;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Masuk");
            StartCoroutine(textDialog.ResumeInstruksi());
        }
    }
}
