using UnityEngine;
using System.Collections;

public class HarmingTile : GridBasedObject
{
    [Header("Damage Settings")]
    [Range(1, 5)][SerializeField] private int damage = 1;
    [Range(0f, 3f)][SerializeField] private float timePreDamage = 0.5f;
    [Range(0f, 3f)][SerializeField] private float timeDealingDamage = 1f;

    [Header("Components")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Sprite preDamageSprite;
    [SerializeField] private Sprite dealingDamageSprite;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;    
    public bool IsDealingDamage { get; set; }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

    private void OnEnable()
    {
        IsDealingDamage = false;
        boxCollider2D.enabled = false;
        spriteRenderer.sprite = preDamageSprite;
        StartCoroutine(WaitToDealDamage());
    }

    private void OnDisable()
    {
        boxCollider2D.enabled = false;
        IsDealingDamage = false;
        StopAllCoroutines();
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
        yield return new WaitForSeconds(timePreDamage);
        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        IsDealingDamage = true;
        spriteRenderer.sprite = dealingDamageSprite;
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(timeDealingDamage);
        gameObject.SetActive(false);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
