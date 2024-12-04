using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

/* <summary>
/// Abstract base class representing a generic character in a 3D Top-Down Dungeon Crawler.
/// Specific character types (e.g., Warrior, Mage) should inherit from this class.
/// </summary> */
public abstract class CharacterBase : NetworkBehaviour
{
    // Character Attributes
    [Header("Character Attributes")]
    public string CharacterName = "Hero";
    public int Level = 1;
    public float Health = 100f;
    public float MaxHealth = 100f;
    public float Mana = 50f;
    public float MaxMana = 50f;
    public float Stamina = 100f;
    public float MaxStamina = 100f;

    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float RotationSpeed = 10f;

    [Header("Combat")]
    public float AttackPower = 10f;
    public float Defense = 5f;
    public float AttackRange = 1.5f;
    public float CooldownBetweenAttacks = 1.0f;

    [Header("Audio")]
    public AudioClip AttackSound;
    public AudioClip HurtSound;
    public AudioClip DeathSound;

    [Header("Status Effects")]
    public List<StatusEffect> ActiveStatusEffects = new List<StatusEffect>();
    public List<Shield> ActiveShields = new List<Shield>();

    [Header("Dungeon Interaction")]
    public bool CanInteract = true;

    protected Animator animator;
    protected Rigidbody rb;
    protected NetworkObject no;
    private float lastAttackTime;

    /* <summary>
    /// Initialization logic for the character. Override as needed.
    /// </summary> */
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        no = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if(no.HasAuthority) {
            Vector3 movementVector = Vector3.zero;

        }

        // Common update logic
        HandleMovement();

        // Allow child classes to define custom update behavior
        HandleCustomUpdate();
    }

    /* <summary>
    /// Handle character movement.
    /// </summary> */
    protected virtual void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude > 0.1f)
        {
            rb.MovePosition(transform.position + movement * MoveSpeed * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);

            if (animator != null)
            {
                animator.SetBool("isWalking", true);
            }
        }
        else if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }
    }

    /* <summary>
    /// Attempts to perform an attack if not on cooldown.
    /// </summary> */
    public virtual void AttemptAttack()
    {
        if (Time.time - lastAttackTime >= CooldownBetweenAttacks)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }
    }

    /* <summary>
    /// Executes the attack logic. Can be overridden by child classes.
    /// </summary> */
    protected virtual void PerformAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Play attack sound
        if (AttackSound != null)
        {
            AudioSource.PlayClipAtPoint(AttackSound, transform.position);
        }

        // Attack logic (e.g., damaging enemies)
        StartCoroutine(DealDamage());
    }

    /* <summary>
    /// Deals damage to enemies within attack range.
    /// </summary> */
    protected virtual IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.5f); // Sync with attack animation        
    }

    /* <summary>
    /// Reduces the character's health when taking damage. Handles death if health reaches zero.
    /// </summary> 
    /// <param name="damage">Amount of damage taken.</param> */
    public virtual void TakeDamage(float damage)
    {
        var damageAfterShield = TakeShieldDamage(damage);
        Health -= damageAfterShield;

        if (HurtSound != null)
        {
            AudioSource.PlayClipAtPoint(HurtSound, transform.position);
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual float TakeShieldDamage(float damage) {
        float damageLeft = damage;
        foreach(var Shield in ActiveShields) {
            if(Shield.ShieldAmount <= damageLeft) {
                damageLeft -= Shield.ShieldAmount;
                ActiveShields.Remove(Shield);
            } else {
                Shield.ShieldAmount -= damageLeft;
                return 0;
            }
        }
        return damageLeft;
    }

    /* <summary>
    /// Handles character death.
    /// </summary> */
    protected virtual void Die()
    {
        Health = 0;

        if (DeathSound != null)
        {
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        CanInteract = false;
        Destroy(gameObject, 2.0f);
    }

    /* <summary>
    /// Heals the character by a specified amount.
    /// </summary>
    /// <param name="amount">Amount of health restored.</param> */
    public virtual void Heal(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
    }

    /* <summary>
    /// Override this method to implement custom behavior in derived classes.
    /// </summary> */
    protected abstract void HandleCustomUpdate();
}
