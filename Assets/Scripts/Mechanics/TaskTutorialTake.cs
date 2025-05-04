using UnityEngine;

public class TaskTutorialTake : Interactable
{
    bool isDone = false;

    public override string Description()
    {
        if (isDone == false)
        {
            return "Press {E} to interact.";
        }
        else
        {
            return " ";
        }
    }

    public override void Interact()
    {
        isDone = true;
    }
}
