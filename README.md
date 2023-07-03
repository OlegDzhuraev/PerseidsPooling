# Perseids Pooling
Perseids Pooling is a basic object pooling implementation for Unity.

## How to use?
You can check **Example** folder, it will show you classic use case. 

To spawn poolable object (or activate it from the pool):
```cs
var instance = Pool.Spawn(DamageablePrefab);
```

It replaces GameObject.Instantiate, but only without arguments like position and rotation (in actual version).

To return object back to the pool:
```cs
Pool.Back(instance); // you can use this code from any script, instance here is gameObject link, so it can be also Pool.Back(gameObject) etc.
```

It will deactivate object and move it to the pool.

You also need to implement IResettable in all of your MonoBehaviour components, attached to the poolable objects, if there exist some data (variables), which should be resseted after respawn from the pool.

```cs
public class Damageable : MonoBehaviour, IResettable
{
  [SerializeField] float maxHealth = 100;
  [SerializeField] float health;
        
  void IResettable.ResetPooled()
  {
    health = maxHealth;
  }
}
```

When scene changed, you need to call
```cs
Pool.Reset();
```
To clean already pooled objects.

### Utilities ###
There included some utilities which implements frequently used tools with pools.

**DelayedPoolDestruction** - Simple timed destruction script, which returns object to a pool instead of destroying it.

**PoolableParticleSystem** - Replays particle systems with Play on Awake enabled after every unpooling of the object.

**LightFxTimedPoolable** - Can be used to play simple light animation effect every time object being unpooled.

## License
MIT License
