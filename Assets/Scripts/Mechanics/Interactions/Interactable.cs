using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold
    }

    public InteractionType interactionType;

    public abstract string Description();
    public abstract void Interact();


}
