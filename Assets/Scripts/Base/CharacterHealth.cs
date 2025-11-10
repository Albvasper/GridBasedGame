using UnityEngine;
using System.Collections;

/// <summary>
/// Manages characters health system.
/// </summary>
public abstract class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int maxHealth = 3;
    protected int health;
    protected SpriteRenderer spriteRenderer;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamageAnimation());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual IEnumerator TakeDamageAnimation()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
