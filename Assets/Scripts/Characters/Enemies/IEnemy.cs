namespace Characters
{

    public interface IEnemy
    {
        void TakeDamage(float damageAmount);
        float GetHealth();
        void FollowTarget();
        void AttackTarget();
        void Stun(float damageAmount);
        void Stop();
    }

}