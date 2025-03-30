using UnityEngine;

namespace ThroughTheWoods
{
    public interface IHealth
    {
        void TakeDamage(float damage, bool isCristical = false);
        // void Heal(float amount);
        // float GetCurrentHealth();
        // float GetMaxHealth();
    }
    public class Health : MonoBehaviour
    {
        IHealth rootHealth;
        void Start()
        {
            rootHealth = GetComponent<IHealth>();
        }
        public void SetRootHealth(IHealth health)
        {
            rootHealth = health;
        }
        public void TakeDamage(float damage, bool isCristical)
        {
            rootHealth.TakeDamage(damage, isCristical);
        }
    }
}

