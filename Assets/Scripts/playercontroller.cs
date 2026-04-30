using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.25f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private float fireTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateAiming();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(movementInput.x, 0f, movementInput.y) * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    private void ReadMovementInput()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  horizontal -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontal += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  vertical -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    vertical += 1f;

        movementInput = new Vector2(horizontal, vertical).normalized;
    }

    private void UpdateAiming()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || Camera.main == null) return;

        // Cast a ray from the camera through the mouse position onto an imaginary
        // horizontal plane at the player's height. Where it hits = where to aim.
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 aimPoint = ray.GetPoint(distance);
            Vector3 lookDirection = aimPoint - transform.position;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    private void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.isPressed && fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireCooldown;
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}