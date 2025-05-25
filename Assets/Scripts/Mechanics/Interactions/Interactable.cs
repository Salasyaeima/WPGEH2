using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold,
        Item
    }


    public InteractionType interactionType;

    float holdTime;
    float isHold;

    public abstract string Description();
    public abstract void Interact();
    public virtual void Drop() { }
    public void increaseHoldTime() => holdTime += Time.deltaTime;
    public void resetHoldTime() => holdTime = 0f;
    public float HoldTime() => holdTime;
    public float isHolding() => isHold = holdTime;

    public virtual void OnHoldStart() { }
    public virtual void OnHoldEnd() { }







}
