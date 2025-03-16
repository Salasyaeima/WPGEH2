using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    [SerializeField]
    private float speed = 20f;

    private Vector2 direction;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementDirection = new Vector3(direction.x, 0, direction.y);
        transform.position += movementDirection * speed * Time.fixedDeltaTime;
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }
}
