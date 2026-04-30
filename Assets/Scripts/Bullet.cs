using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Bullet flies in whatever direction it's facing when spawned
        rb.linearVelocity = transform.forward * speed;

        // Auto-destroy so we don't litter the scene
        Destroy(gameObject, lifetime);
    }
    
        private void OnTriggerEnter(Collider other)
    {
        // Try to damage what we hit
        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }

        // Either way, the bullet is gone
        Destroy(gameObject);
    }
}
