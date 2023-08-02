using System;
using System.Collections.Generic;
using UnityEngine;

namespace InsaneOne.PerseidsPooling
{
	/// <summary>Pool class allows pool objects and speed up spawn of new ones. </summary>
	public static class Pool
	{
		public static bool AllowManualPooling { get; set; }
		
		static readonly Dictionary<int, List<PoolObject>> pools = new Dictionary<int, List<PoolObject>>();

		/// <summary> Use this method to reset Pool on every scene reload. </summary>
		public static void Reset() => pools.Clear();

		/// <summary>Spawn object using pools. If same object already was pooled and inactive now,
		/// it will be re-activated, not instanced again. </summary>
		public static GameObject Spawn(GameObject prefab) => Spawn(prefab, true).Instance;

		/// <summary>Spawn object using pools. If same object already was pooled and inactive now,
		/// it will be re-activated, not instanced again.
		/// <para>Set autoUnpool to false, if you want to do unpool manually after some object initialization.</para></summary>
		public static PoolObject Spawn(GameObject prefab, bool autoUnpool)
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

			if (autoUnpool)
				poolObject.Unpool();
			
			return poolObject;
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

			instance.SetActive(false);
			throw new ArgumentException("This object was not added to the pool system. Disabling.");
		}

		internal static void Log(string text) => Debug.Log($"[Perseids Pooling] {text}");
	}
	
	public class PoolObject
	{
		public int PrefabHash { get; private set; }
		public GameObject Instance { get; private set; }
		public bool IsPooled { get; private set; }

		IResettable[] resettables;

		public static PoolObject Create(GameObject prefab)
		{
			var po = new PoolObject
			{
				Instance = GameObject.Instantiate(prefab),
				PrefabHash = prefab.GetHashCode()
			};
			
			po.resettables = po.Instance.GetComponents<IResettable>();

			return po;
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
			
			for (var i = 0; i < resettables.Length; i++)
				resettables[i].ResetPooled();
		}

		public void ManualPool()
		{
			if (PerseidsPooling.Pool.AllowManualPooling)
				Pool();
			else
				PerseidsPooling.Pool.Log("Manual pooling is not enabled!");
		}
		
		public void ManualUnpool()
		{
			if (PerseidsPooling.Pool.AllowManualPooling)
				Unpool();
			else
				PerseidsPooling.Pool.Log("Manual pooling is not enabled!");
		}
	}
}