using UnityEngine;

public class ItemData : MonoBehaviour
{
    public enum ItemCategory
    {
        Toy,
        Clothes,
        Book,
        Box
    }

    public ItemCategory category;
}
