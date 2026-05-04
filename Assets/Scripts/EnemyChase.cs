using UnityEngine;

/// <summary>
/// Enemy AI implemented as a finite state machine.
/// Adapted from the Wander/Attack/Flee pattern in Lecture 6,
/// using the onEnter/onUpdate/onExit lifecycle covered in lectures.
/// </summary>
public class EnemyChase : MonoBehaviour
{
    private enum State { Idle, Chase, Attack }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Combat")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 1f;

    private State currentState;
    private Transform player;
    private Rigidbody rb;
    private float damageTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        Debug.Log($"{gameObject.name} Awake — found player: {player != null}");
    }

    private void Start()
    {
        EnterState(State.Idle);
    }

    private void Update()
    {
        damageTimer -= Time.deltaTime;
        UpdateCurrentState();
        CheckTransitions();
    }

    private void EnterState(State newState)
    {
        ExitState(currentState);
        currentState = newState;

        switch (newState)
        {
            case State.Idle:    OnEnterIdle();    break;
            case State.Chase:   OnEnterChase();   break;
            case State.Attack:  OnEnterAttack();  break;
        }
    }

    private void UpdateCurrentState()
    {
        switch (currentState)
        {
            case State.Idle:    OnUpdateIdle();    break;
            case State.Chase:   OnUpdateChase();   break;
            case State.Attack:  OnUpdateAttack();  break;
        }
    }

    private void ExitState(State oldState)
    {
        switch (oldState)
        {
            case State.Idle:    OnExitIdle();    break;
            case State.Chase:   OnExitChase();   break;
            case State.Attack:  OnExitAttack();  break;
        }
    }

    private void CheckTransitions()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distance <= detectionRange) EnterState(State.Chase);
                break;

            case State.Chase:
                if (distance <= attackRange) EnterState(State.Attack);
                else if (distance > detectionRange) EnterState(State.Idle);
                break;

            case State.Attack:
                if (distance > attackRange) EnterState(State.Chase);
                break;
        }
    }

    private void OnEnterIdle() { rb.linearVelocity = Vector3.zero; }
    private void OnUpdateIdle() { }
    private void OnExitIdle() { }

    private void OnEnterChase() { }

    private void OnUpdateChase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnExitChase() { rb.linearVelocity = Vector3.zero; }

    private void OnEnterAttack() { rb.linearVelocity = Vector3.zero; }

    private void OnUpdateAttack()
    {
        if (damageTimer > 0f) return;
        if (player == null) return;

        if (player.TryGetComponent(out Health health))
        {
            health.TakeDamage(contactDamage);
            damageTimer = damageCooldown;
        }
    }

    private void OnExitAttack() { }

    private void OnDrawGizmosSelected()
    {
        // Yellow circle for detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Red circle for attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}