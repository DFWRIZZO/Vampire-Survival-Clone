using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public int attackDamage = 1;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 4f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] public ParticleSystem critEffect; 
    [SerializeField] public float critDamage = 4f;
    public ParticleDamage particleDamage;
   
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            animator.SetTrigger("Attack");
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            int finalDamage = attackDamage;

            // Calculate if the hit is critical
            bool isCriticalHit = Random.value < critChance;
            if (isCriticalHit)
            {
                finalDamage = Mathf.RoundToInt(attackDamage * (critMultiplier + critDamage));
                PlayCritEffect();
            }

            // Apply damage to the enemy
            enemy.GetComponent<Enemy>().TakeDamage(finalDamage);
        }
    }

    private void PlayCritEffect()
    {
        if (critEffect != null)
        {
            critEffect.transform.position = attackPoint.position;  // Set position to the attack point
            critEffect.Stop();  // Stop any existing particle emission
            critEffect.Play();  // Play the effect
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
