using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{

    private LongRangeEnemyController enemy;
    
    private void Awake()
    {
        enemy = GetComponentInParent<LongRangeEnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check layer mask
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        // Player detected
        enemy.OnPlayerDetected(other.gameObject.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check layer mask
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        // Player detected
        enemy.OnPlayerLost();
    }
}
