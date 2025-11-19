using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
{
    [Header("Damage Settings")]
    [Range(1, 5)][SerializeField] private int damage = 1;
    [Range(0f, 3f)][SerializeField] private float timePreDamage = 0.5f;
    [Range(0f, 3f)][SerializeField] private float timeDealingDamage = 1f;

    [Header("Components")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Sprite spikesDownSprite;
    [SerializeField] private Sprite spikesUpSprite;
    
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(WaitToDealDamage());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check layer mask 
        if (IsInLayerMask(other.gameObject.layer, playerLayerMask))
        {
            // Check if its damagable
            if (other.TryGetComponent<IDamageable>(out var player))
                player.TakeDamage(damage);
        }
    }

    private IEnumerator WaitToDealDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePreDamage);
            spriteRenderer.sprite = spikesUpSprite;
            boxCollider2D.enabled = true;
            yield return new WaitForSeconds(timeDealingDamage);
            spriteRenderer.sprite = spikesDownSprite;
            boxCollider2D.enabled = false;
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
