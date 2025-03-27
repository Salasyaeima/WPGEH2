using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;
    public Image checkIcon;
    public Task task;

    RectTransform textRectTransform;
    RectTransform iconRectTransform;

    void Awake()
    {
        textRectTransform = taskText.GetComponent<RectTransform>();
        if (checkIcon != null) iconRectTransform = checkIcon.GetComponent<RectTransform>();
        checkIcon.enabled = false;
    }

    public void Initialize(Task task)
    {
        this.task = task;
        taskText.text = task.taskName;

        Debug.Log("Masuk");
        UpdateCheckIconPosition();

        if (task.isCompleted)
        {
            StrikeThroughText();
        }
        else
        {
            taskText.color = Color.white;
            if (checkIcon != null)
            {
                checkIcon.enabled = false;
            }
        }
    }

    public void StrikeThroughText()
    {
        taskText.color = Color.gray;
        taskText.fontStyle = FontStyles.Strikethrough;
        if (checkIcon != null)
        {
            checkIcon.enabled = true;
            Debug.Log("CheckIcon diaktifkan untuk: " + task.taskName);
        }
    }

    public void UpdateCheckIconPosition()
    {
        if (checkIcon == null || textRectTransform == null || iconRectTransform == null)
            return;

        float textWidth = taskText.preferredWidth;
        Vector2 textSize = textRectTransform.sizeDelta;
        textSize.x = textWidth;
        textRectTransform.sizeDelta = textSize;

        float iconOffset = 10f;
        iconRectTransform.anchoredPosition = new Vector2(textWidth + iconOffset, 0f);
        iconRectTransform.sizeDelta = new Vector2(20f, 20f);
    }
}