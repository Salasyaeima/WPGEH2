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

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Bakso");
        if (other.gameObject.CompareTag("Mother"))
        {
            Debug.Log("Bokep");
        }
    }
}
