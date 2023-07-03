using UnityEngine;

namespace InsaneOne.PerseidsPooling
{
    /// <summary> Allows to use and control lightning in the pooling. Primary used for animation effects light.</summary>
    public sealed class LightFxTimedPoolable : MonoBehaviour, IResettable
    {
        [Tooltip("Time from this parameter will be used to animate this light.")]
        [SerializeField] AnimationCurve intensityByTime;
        [SerializeField] bool colorizeByTime;
        [SerializeField] Gradient colorByTime;
        [SerializeField] new Light light;

        float timePassed;

        void Start() => ResetPooled();

        public void ResetPooled() => timePassed = 0;
        
        void Update()
        {
            if (timePassed >= intensityByTime[intensityByTime.length - 1].time)
                return;

            light.intensity = intensityByTime.Evaluate(timePassed);

            if (colorizeByTime)
                light.color = colorByTime.Evaluate(timePassed);
            
            timePassed += Time.smoothDeltaTime;
        }
    }
}