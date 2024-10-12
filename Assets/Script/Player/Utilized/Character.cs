using UnityEngine;

public abstract class Character : MonoBehaviour {
    [SerializeField] protected int health;  // Protected so subclasses can access it
    public float moveSpeed = 5f;

    protected Rigidbody2D rb;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damage) {
        health -= damage;
        Debug.Log(gameObject.name + " took damage! Remaining health: " + health);

        if (health <= 0) {
            Die();
        }
    }

    protected abstract void Die();  // Abstract method must be implemented in derived classes

    public int GetHealth() {
        return health;
    }
}
