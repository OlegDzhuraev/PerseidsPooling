using PerseidsPooling;
using UnityEngine;

namespace PerseidsPooling.Example
{
    public class SceneController : MonoBehaviour
    {
        public GameObject DamageablePrefab;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var instance = Pool.Spawn(DamageablePrefab);
                
                instance.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            }
        }
    }
}