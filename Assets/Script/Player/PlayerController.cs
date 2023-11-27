using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Range(0f, 1f)]
    public float rotationSpeed = 0.15f; // Degrees per second

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Subscribe to the input system's events
    }

    void OnDisable()
    {
        // Unsubscribe from the input system's events
    }



    void FixedUpdate()
    {
        // Use FixedUpdate for physics-related updates
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // This method is called by the Input System
        moveInput = context.ReadValue<Vector2>();
    }

    public void Move()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        // Move the player
        if (movement.magnitude > 0.1f) // Check if there is significant movement input
        {
            movement.Normalize(); // Normalize to maintain consistent speed
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            // Rotate the player to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed);
        }
    }

}
