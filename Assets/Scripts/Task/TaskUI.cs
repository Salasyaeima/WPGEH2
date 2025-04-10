using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;
    public Task task;
    public ITaskProvider taskProvider; // Cuma pake ini

    RectTransform textRectTransform;

    void Awake()
    {
        textRectTransform = taskText.GetComponent<RectTransform>();
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
    }

    public void UpdateCheckIconPosition()
    {
        float textWidth = taskText.preferredWidth;
        Vector2 textSize = textRectTransform.sizeDelta;
        textSize.x = textWidth;
        textRectTransform.sizeDelta = textSize;
    }

    void Update()
    {
        UpdateTaskDisplay();
    }
}