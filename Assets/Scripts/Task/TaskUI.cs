using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;
    public Image checkIcon;
    public Task task;
    public ITaskProvider taskProvider; // Cuma pake ini

    RectTransform textRectTransform;
    RectTransform iconRectTransform;

    void Awake()
    {
        textRectTransform = taskText.GetComponent<RectTransform>();
        if (checkIcon != null) iconRectTransform = checkIcon.GetComponent<RectTransform>();
        if (checkIcon != null) checkIcon.enabled = false;
    }

    public void Initialize(Task task, ITaskProvider provider = null)
    {
        this.task = task;
        this.taskProvider = provider;
        UpdateTaskDisplay();

        if (task.isCompleted)
        {
            StrikeThroughText();
        }
        else
        {
            if (checkIcon != null)
            {
                checkIcon.enabled = false;
            }
        }
    }

    public void UpdateTaskDisplay()
    {
        if (taskProvider != null)
        {
            taskText.text = taskProvider.GetTaskName();
        }
        else
        {
            taskText.text = task.taskName;
        }
        UpdateCheckIconPosition();
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

    void Update()
    {
        UpdateTaskDisplay();
    }
}