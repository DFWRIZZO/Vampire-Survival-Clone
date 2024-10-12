using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public Animator animator;
    private Vector2 movement;
    public Transform attackPoint;
    public Vector3 attackOffset;
    public static event System.Action OnPlayerDeath;
    public float maxHealth = 5f;
    private float currentHealth;
    public Vector3 startingPosition;

    [Header("Blocking Properties")]
    public GameObject shield; // Reference to the shield GameObject
    public float blockDuration = 5f; // Duration of the block (in seconds)
    public float blockCooldown = 10f; // Cooldown time before blocking can be used again
    public bool isBlocking = false; // Track if the player is currently blocking
    public bool canBlock = false; // Block mechanic is disabled until the skill is selected

    [Header("Cooldown Timer UI")]
    public GameObject blockCooldownUI; // Reference to the entire parent UI containing timer and background
    public Image blockCooldownTimer; // Reference to the circular cooldown timer image

    private float blockCooldownTimerValue = 0f; // Track remaining cooldown time

    public float Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    protected override void Start()
    {
        base.Start();
        Health = maxHealth;
        startingPosition = transform.position;
        ResetPlayer();

        // Ensure the shield is disabled initially
        if (shield != null)
        {
            shield.SetActive(false);
        }

        // Ensure the entire cooldown UI (timer + background) is hidden initially
        if (blockCooldownUI != null)
        {
            blockCooldownUI.SetActive(false);  // Hide cooldown UI until block skill is unlocked
        }
    }

    void Update()
    {
        if (Health <= 0) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        transform.Translate(moveSpeed * Time.deltaTime * movement);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Only allow blocking if the player has unlocked the block skill and the block is ready to use
        if (Input.GetKeyDown(KeyCode.B) && canBlock)
        {
            StartCoroutine(Block());
        }

        // Update cooldown timer UI if the block is on cooldown
        if (!canBlock && blockCooldownTimer != null)
        {
            blockCooldownTimer.fillAmount = blockCooldownTimerValue / blockCooldown;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isBlocking)
        {
            Debug.Log("Blocked the attack! No damage taken.");
            return; // Prevent taking damage when blocking
        }

        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        OnPlayerDeath?.Invoke();
        FindObjectOfType<GameManager>().TriggerGameOver();
        Destroy(gameObject);
    }

    // Coroutine to handle blocking
    private IEnumerator Block()
    {
        Debug.Log("Player is blocking.");
        isBlocking = true;
        canBlock = false;

        if (shield != null)
        {
            shield.SetActive(true); // Enable the shield GameObject
        }

        animator.SetTrigger("Block"); // Trigger block animation (if available)

        yield return new WaitForSeconds(blockDuration); // Wait for the block duration

        isBlocking = false; // Remove blocking state
        if (shield != null)
        {
            shield.SetActive(false); // Disable the shield GameObject
        }

        Debug.Log("Block ended.");

        // Start cooldown timer
        StartCoroutine(BlockCooldown());
    }

    // Coroutine to handle cooldown
    private IEnumerator BlockCooldown()
    {
        blockCooldownTimerValue = blockCooldown; // Set initial cooldown value

        while (blockCooldownTimerValue > 0)
        {
            blockCooldownTimerValue -= Time.deltaTime;
            blockCooldownTimer.fillAmount = blockCooldownTimerValue / blockCooldown; // Update UI fill amount
            yield return null;
        }

        blockCooldownTimer.fillAmount = 0f; // Reset fill amount to 0 when cooldown is complete
        canBlock = true; // Allow blocking again
        Debug.Log("Player can block again.");
    }

    public void ResetPlayer()
    {
        Health = maxHealth; // Reset health to max
        transform.position = startingPosition; // Reset position to starting position
    }

    // Method to enable block skill when the power-up is selected
    public void EnableBlockSkill()
    {
        canBlock = true;

        // Show the entire cooldown timer UI (including background) once the block skill is unlocked
        if (blockCooldownUI != null)
        {
            blockCooldownUI.SetActive(true);  // Enable the cooldown UI parent GameObject
        }
    }
}
