using UnityEngine;

namespace BattlefieldSimulator
{
    public abstract class NoiseSettings : UpdatableData
	{
		[Space(5)]
		public float scale = 50;

		public void ValidateValues()
		{
			scale = Mathf.Max(scale, 0.01f);
		}

		public virtual float Evaluate(int seed, float x, float y, Vector2 sampleCenter, float baseScale) { return 0; }
		public virtual void GenerateNoise() { }

#if UNITY_EDITOR

		protected virtual void OnValidate()
		{
			ValidateValues();
		}
#endif
	}
}
