using UnityEngine;

namespace InsaneOne.PerseidsPooling.Example
{
    public class ExampleDamageable : MonoBehaviour, IResettable
    {
        [SerializeField] float maxHealth = 100;
        [SerializeField] float health;
        [SerializeField] new Renderer renderer;
        
        static readonly int colorId = Shader.PropertyToID("_Color");
        
        void OnMouseDown() => TakeDamage(25);
        
        public void TakeDamage(float damage)
        {
            health -= damage;

            UpdateColor();
            
            if (health < 0)
                Die();
        }

        void UpdateColor()
        {
            var healthColor = Color.Lerp(Color.red, Color.green, health / maxHealth);
            
            renderer.material.SetColor(colorId, healthColor);
        }
        
        void Die() => Pool.Back(gameObject);

        void IResettable.ResetPooled()
        {
            health = maxHealth;
            UpdateColor();
        }
    }
}