using UnityEngine;
using TMPro;

public class FullNotification : MonoBehaviour
{
    [SerializeField] GameObject containerObject;
    [SerializeField] TextMeshProUGUI fullText;
    [SerializeField] TextDialogChild textDialogChild;
    Container container;
    bool hasShownFullMessage = false;

    void Start()
    {
        container = containerObject.GetComponent<Container>();
    }

    void Update()
    {
        if (!hasShownFullMessage &&
            container.containerType == Container.ContainerType.wardrobe &&
            container.storedItems.Count >= container.maxCapacity)
        {
            fullText.enabled = false;
            textDialogChild.ResumeDisplayingText();
            hasShownFullMessage = true;
        }
    }
}
