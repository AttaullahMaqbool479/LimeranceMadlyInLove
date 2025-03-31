using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyAILevel4 : MonoBehaviour
{
    
    public bool isTaskStarted;
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    public Level4Controller level4Controller;
    private int currentHealth;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;
    public GameObject enemyCutscene;
    public GameObject collider1;
    public GameObject collider2;
    
    ///public Slider healthBar;
    public Animator animator;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        /////healthBar.maxValue = maxHealth;
        ////healthBar.value = currentHealth;
    }
    
    void Update()
    {
        if(!isTaskStarted) return;
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (currentHealth <= 0) return; // Stop execution if dead
        Debug.Log("Distance is: "+ distance);
        
        
        
        if (distance <= attackRange)
        {
            if (player.GetComponent<HealthBar>().isPlayerAttcking)
            {
                TakeDamage(10);
                return;
            }
            if(Time.time >= nextAttackTime)
                Attack();
        }
        else if (distance > attackRange)
        {
            ChasePlayer();
        }
        
        FlipSprite();
    }
    
    void ChasePlayer()
    {
        
        Debug.Log("Chasing player...");
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        animator.SetBool("isRunning", true);
    }
    
    void Attack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;
        animator.SetTrigger("Attack");
        ApplyDamageToPlayer();
    }
    
    // Called from animation event
    public void ApplyDamageToPlayer()
    {
        HealthBar playerHealth = player.GetComponent<HealthBar>();
        if (playerHealth != null && playerHealth.health > 0)
        {
            playerHealth.LoseHealth(attackDamage);
        }
        else
        {
            level4Controller.LevelFail();
        }
        isAttacking = false;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ////healthBar.value = currentHealth;
        animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        this.enabled = false;
        enemyCutscene.SetActive(false);
        if (level4Controller.isTask1Completed)
        {
            level4Controller.LevelComplete();
        }
        level4Controller.Task1Success();
        collider1.SetActive(false);
        collider2.SetActive(false);
        
        Destroy(gameObject, 2f); // Destroy enemy after death animation
    }
    
    void FlipSprite()
    {
        if (player.position.x > transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
