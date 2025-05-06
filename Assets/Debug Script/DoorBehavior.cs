using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField]
    private Door door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = GetComponentInChildren<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mother"))
        {
            Debug.Log("Bokep");
            StartCoroutine(door.AnimateDoor());
        }
    }

    
}
