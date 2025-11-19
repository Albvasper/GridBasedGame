using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy that detects player position and launches an attack at that location.
/// </summary>
public class LongRangeEnemyController : MonoBehaviour
{

    [Header("Settings")]
    [Range(0f, 6f)][SerializeField] private float attackCooldown = 5f;

    [Header("Components")]
    [SerializeField] private HarmingTile harmingTile;
    
    private bool playerInRange = false;
    private Coroutine attackCoroutine;
    private Transform player;

    private void Awake()
    {
        harmingTile.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Only track player if is about to deal damage, if its dealing damage dont move it.
        if (harmingTile.gameObject.activeSelf && !harmingTile.IsDealingDamage)
            harmingTile.transform.position = player.position;
    }

    public void OnPlayerDetected(Transform playerTransform)
    {
        playerInRange = true;
        player = playerTransform;
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(Attack());
    }

    public void OnPlayerLost()
    {
        playerInRange = false;
        player = null;
        if (!harmingTile.IsDealingDamage)
            harmingTile.gameObject.SetActive(false);
        attackCoroutine = null;
    }
    
    private IEnumerator Attack()
    {
        while(playerInRange && player != null)
        {
            harmingTile.gameObject.SetActive(true);
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
