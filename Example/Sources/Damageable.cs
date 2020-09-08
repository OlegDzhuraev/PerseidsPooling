using PerseidsPooling;
using UnityEngine;

namespace PerseidsPooling.Example
{
    public class Damageable : MonoBehaviour, IResettable
    {
        public float MaxHealth = 100;
        public float Health;
        public Renderer Renderer;
        
        static readonly int colorId = Shader.PropertyToID("_Color");
        
        void OnMouseDown() => TakeDamage(25);
        
        public void TakeDamage(float damage)
        {
            Health -= damage;

            UpdateColor();
            
            if (Health < 0)
                Die();
        }

        void UpdateColor()
        {
            var healthColor = Color.Lerp(Color.red, Color.green, Health / MaxHealth);
            
            Renderer.material.SetColor(colorId, healthColor);
        }
        
        void Die() => Pool.Back(gameObject);

        void IResettable.ResetPooled()
        {
            Health = MaxHealth;
            UpdateColor();
        }
    }
}