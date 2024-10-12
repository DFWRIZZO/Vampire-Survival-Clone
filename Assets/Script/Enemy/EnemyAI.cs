using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float moveSpeed = 3f;  // Speed of movement
    public Transform player;      // Reference to the player's transform
    public Transform attackPoint; // Reference for the adjustable attack position
    public int attackDamage = 1;  // Amount of damage dealt
    public float attackRange = 1f;  // Range within which the enemy will attack
    public float attackRate = 1f;   // Rate of attacks per second
    private float nextAttackTime = 0f;  // Time until the next attack
    public float detectionRange = 10f;  // Range in which the enemy detects the player
    private bool isPlayerAlive = true;  // Check if the player is alive
    public Animator animator;           // Reference to the Animator

    private Rigidbody2D rb;             // Rigidbody2D component for movement
    public int expValue = 20; // Experience value the enemy gives upon death
    public GameObject expPrefab; // Prefab for the EXP drop

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        // Subscribe to the player death event
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    void Update() {
        if (!isPlayerAlive) {
            return;  // Skip updates if the player or enemy is dead
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Set the "isChasing" animation parameter based on detection range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange) {
            animator.SetBool("isChasing", true);
            ChasePlayer();
        } else {
            animator.SetBool("isChasing", false);
        }

        // Calculate distance from attack point to player
        float attackPointToPlayerDistance = Vector2.Distance(attackPoint.position, player.position);

        // Set the "isAttacking" animation parameter if in attack range and cooldown is complete
        if (attackPointToPlayerDistance <= attackRange && Time.time >= nextAttackTime) {
            animator.SetTrigger("Attack");
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack() {
        player.GetComponent<Player>().TakeDamage(attackDamage);
    }

    private void ChasePlayer() {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Only chase if the player is outside the attack range
        if (distanceToPlayer > attackRange) {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = Vector2.Lerp(rb.position, rb.position + direction * (moveSpeed * Time.fixedDeltaTime), 0.5f);
            rb.MovePosition(newPosition);

            // Flip the sprite based on movement direction
            if (direction.x > 0) {
                transform.localScale = new Vector3(1, 1, 1);  // Face right
            } else if (direction.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1);  // Face left
            }
        }
    }

    void HandlePlayerDeath() {
        isPlayerAlive = false;  // Stop chasing the player
        animator.SetBool("isChasing", false);  // Ensure the enemy stops chasing visually
    }

    void OnDrawGizmosSelected() {
        if (attackPoint != null) {
            // Draw a wire sphere at the attack point to visualize the adjustable attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    private void OnDestroy() {
        // Unsubscribe from the event to avoid memory leaks
        Player.OnPlayerDeath -= HandlePlayerDeath;
        
        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }
    }
}
