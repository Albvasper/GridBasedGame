using UnityEngine;
using System.Collections;

/// <summary>
/// Manages player health
/// </summary>
public class PlayerHealth : CharacterHealth
{
    private BoxCollider2D boxCollider2D;
    private float damageCooldown = 3f;      // Time after a hit to enable collider and stop blinking animation.
    private float blinkInterval = 0.1f;     // Interval between blinking sprite in damage animation.

    protected override void Start()
    {
        base.Start();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public override void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamageAnimation());
        StartCoroutine(DisableCollider());
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected override IEnumerator TakeDamageAnimation()
    {
        Color transparent = Color.white;
        transparent.a = 0f;
        float elapsed = 0f;

        while (elapsed < damageCooldown)
        {
            // Make sprite transparent quickly then make it visible again until recovery is over.
            spriteRenderer.color = transparent;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;
        }
        spriteRenderer.color = Color.white;

    }

    protected IEnumerator DisableCollider()
    {
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(damageCooldown);
        boxCollider2D.enabled = true;
    }
}
