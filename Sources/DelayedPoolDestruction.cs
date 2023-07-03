using UnityEngine;

namespace InsaneOne.PerseidsPooling
{
	public sealed class DelayedPoolDestruction : MonoBehaviour, IResettable
	{
		[SerializeField] [Range(0f, 1000f)] float secondsToDestruction = 3f;

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