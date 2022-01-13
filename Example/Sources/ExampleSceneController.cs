using UnityEngine;

namespace PerseidsPooling.Example
{
    public class ExampleSceneController : MonoBehaviour
    {
        [SerializeField] GameObject damageablePrefab;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var instance = Pool.Spawn(damageablePrefab);
                
                instance.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            }
        }
    }
}