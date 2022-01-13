using UnityEngine;

namespace PerseidsPooling.Utils
{
    public class PoolableParticleSystem : MonoBehaviour, IResettable
    {
        [SerializeField] ParticleSystem[] systems;
        
        public void ResetPooled()
        {
            foreach (var system in systems)
                if (system && system.main.playOnAwake)
                    system.Play();
        }
    }
}
