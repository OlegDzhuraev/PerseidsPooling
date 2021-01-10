# Perseids Pooling
Perseids Pooling is a simple object pooling implementation for Unity.

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
  public float MaxHealth = 100;
  public float Health;
        
  void IResettable.ResetPooled()
  {
    Health = MaxHealth;
  }
}
```

When scene changed, you need to call
```cs
Pool.Reset();
```
To clean already pooled objects.

## License
MIT License
