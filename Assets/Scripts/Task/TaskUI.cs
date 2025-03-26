using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;

    public Task task;

    public void Initialize(Task task)
    {
        this.task = task;
        taskText.text = task.taskName;

        if (task.isCompleted)
        {
            StrikeThroughText();
        }
    }

    public void StrikeThroughText()
    {
        taskText.color = Color.gray;
        taskText.fontStyle = FontStyles.Strikethrough;
    }
}