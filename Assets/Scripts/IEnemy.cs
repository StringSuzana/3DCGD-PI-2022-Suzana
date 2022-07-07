using UnityEngine;

public interface IEnemy
{
    void TakeDamage(float damageAmount);
    float GetHealth();
    void FollowTarget(Transform targetTransform);
    void AttackTarget (PlayerController player);
    void StopMovingForSeconds(float damageAmount);
}
