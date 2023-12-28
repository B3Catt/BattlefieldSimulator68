using UnityEngine;

namespace BattlefieldSimulator
{
    public static class RuntimeExtensions
    {

        /// <summary> Converts given bitmask to layer number </summary>
        /// <returns> layer number </returns>
        public static int ToLayer(this LayerMask bitmask)
        {
            int result = bitmask > 0 ? 0 : 31;
            while (bitmask > 1)
            {
                bitmask = bitmask >> 1;
                result++;
            }
            return result;
        }

        public static BiomeDataSettings.DetailData GetRandomWeightedPrefab(this BiomeDataSettings.DetailData[] details, float density)
        {
            if (details == null || details.Length == 0) return null;

            int i = 0;
            float[] weights = new float[details.Length];

            for (i = 0; i < weights.Length; i++)
            {
                weights[i] = details[i].density;
            }

            float w;
            float t = 0;
            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];

                if (float.IsPositiveInfinity(w))
                {
                    return details[i];
                }
                else if (w >= 0f && !float.IsNaN(w))
                {
                    t += weights[i];
                }
            }

            float r = UnityEngine.Random.value;
            float s = 0f;

            if (r > density)
                return null;

            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];
                if (float.IsNaN(w) || w <= 0f) continue;

                s += w / t;
                if (s >= r) return details[i];
            }

            return null;
        }

        public static BiomeDataSettings.DetailData GetRandomWeightedPrefab(this BiomeDataSettings.DetailData[] details, float x, float y, int seed, float density, float scale = 1.1f)
        {

            if (details == null || details.Length == 0) return null;

            int i = 0;
            float[] weights = new float[details.Length];

            for (i = 0; i < weights.Length; i++)
            {
                weights[i] = details[i].density;
            }

            float w;
            float t = 0;
            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];

                if (float.IsPositiveInfinity(w))
                {
                    return details[i];
                }
                else if (w >= 0f && !float.IsNaN(w))
                {
                    t += weights[i];
                }
            }

            float r = Mathf.PerlinNoise((x + seed) / scale, (y + seed) / scale);
            float s = 0f;

            if (r > density)
                return null;

            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];
                if (float.IsNaN(w) || w <= 0f) continue;

                s += w / t;
                if (s >= r) return details[i];
            }

            return null;
        }
    }
}
