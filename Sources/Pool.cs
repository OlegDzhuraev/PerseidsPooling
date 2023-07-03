using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.PerseidsPooling
{
	/// <summary>Pool class allows pool objects and speed up spawn of new ones. </summary>
	public static class Pool
	{
		static readonly Dictionary<int, List<PoolObject>> pools = new Dictionary<int, List<PoolObject>>();

		/// <summary> Use this method to reset Pool on every scene reload. </summary>
		public static void Reset() => pools.Clear();

		/// <summary>Spawn object using pools. If same object already was pooled and inactive now, it will be re-activated, not instanced again.</summary>
		public static GameObject Spawn(GameObject prefab)
		{
			var prefabHash = prefab.GetHashCode();

			PoolObject poolObject = null;
			
			if (pools.ContainsKey(prefabHash))
			{
				var pooledCollection = pools[prefabHash];

				for (var i = 0; i < pooledCollection.Count; i++)
				{
					var comparePoolObject = pooledCollection[i];

					if (comparePoolObject.IsPooled)
					{
						poolObject = comparePoolObject;
						break;
					}
				}

				if (poolObject == null)
				{
					poolObject = PoolObject.Create(prefab);
					pooledCollection.Add(poolObject);
				}
			}
			else
			{
				poolObject = PoolObject.Create(prefab);
				pools.Add(prefabHash, new List<PoolObject> { poolObject });
			}

			poolObject.Unpool();
			
			return poolObject.Instance;
		}

		/// <summary>Back object to the pool. Note that will work only with objects, which spawned by Pool.Spawn method. </summary>
		public static void Back(GameObject instance)
		{
			foreach (var entry in pools)
			{
				var pool = entry.Value;
				
				for (var i = 0; i < pool.Count; i++)
				{
					var poolObject = pool[i];
					
					if (!poolObject.IsPooled && poolObject.Instance == instance)
					{
						poolObject.Pool();
						return;
					}
				}
			}

			throw new ArgumentException("Perseids Pooling: This object was not added to the pool system.");
		}

		class PoolObject
		{
			public int PrefabHash { get; private set; }
			public GameObject Instance { get; private set; }
			public bool IsPooled { get; private set; }

			public static PoolObject Create(GameObject prefab)
			{
				return new PoolObject
				{
					Instance = GameObject.Instantiate(prefab),
					PrefabHash = prefab.GetHashCode()
				};
			}
			
			internal void Pool()
			{
				IsPooled = true;
				
				if (!Instance)
					throw new NullReferenceException("Perseids Pooling: No pool object Instance found. Possible reasons: \n1. Not called Pool.Reset() on game start. \n2. This object was not added into the pool system (placed directly to scene, for example).\n3. Was destroyed by some other code.");

				Instance.SetActive(false);
			}

			internal void Unpool()
			{
				IsPooled = false;
				Instance.SetActive(true);

				var resettables = Instance.GetComponents<IResettable>();

				for (var i = 0; i < resettables.Length; i++)
					resettables[i].ResetPooled();
			}
		}
	}
}