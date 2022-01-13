using UnityEngine;

namespace PerseidsPooling.Utils
{
	public class DelayedPoolDestruction : MonoBehaviour, IResettable
	{
		[SerializeField, Min(0f)] float secondsToDestruction = 3f;

		float timeLeft;
		
		void Update()
		{
			timeLeft -= Time.deltaTime;

			if (timeLeft > 0)
				return;

			Pool.Back(gameObject);
		}

		public void SetDestructionTime(float newTime) => timeLeft = newTime;
		public void ResetPooled() => SetDestructionTime(secondsToDestruction);
	}
}