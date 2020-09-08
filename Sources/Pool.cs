using System.Collections.Generic;
using UnityEngine;

namespace PerseidsPooling
{
	/// <summary>Pool class allows pool objects and speed up spawn of new ones. </summary>
	public static class Pool
	{
		static readonly List<PoolObject> poolableObjects = new List<PoolObject>();

		/// <summary>Spawn object using pools. If same object is pooled, it will be re-activated, not instanced again.</summary>
		public static GameObject Spawn(GameObject prefab)
		{
			GameObject spawnedObject = null;

			for (var i = 0; i < poolableObjects.Count; i++)
			{
				var pooled = poolableObjects[i];

				if (pooled.IsPooled && pooled.PrefabHash == prefab.GetHashCode())
				{
					pooled.Unpool();
					spawnedObject = pooled.Instance;
					break;
				}
			}

			if (!spawnedObject)
			{
				var poolObject = PoolObject.Create(prefab);
				spawnedObject = poolObject.Instance;

				poolObject.Unpool();
				
				poolableObjects.Add(poolObject);
			}

			return spawnedObject;
		}

		/// <summary>Back object to the pool. Note that will work only with objects, which spawned by Pool.Spawn method. </summary>
		public static void Back(GameObject instance)
		{
			for (var i = 0; i < poolableObjects.Count; i++)
			{
				var pooled = poolableObjects[i];

				if (!pooled.IsPooled && pooled.Instance == instance)
				{
					pooled.Pool();
					return;
				}
			}
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