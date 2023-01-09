using UnityEngine;



namespace Noises
{
    [CreateAssetMenu(fileName = "Single Thread Noise", menuName = "Noise/Single Thread", order = 0)]
    public class SingleThreadNoise : AbstractNoise
    {
        public override void Apply(in Texture2D texture2D, float angleOffset, float cellDensity)
        {
            for (var i = 0; i < texture2D.width; i++)
            for (var j = 0; j < texture2D.height; j++)
            {
                Noise.Voronoi(new Vector2(i, j), angleOffset, cellDensity, out _, out var value);
                texture2D.SetPixel(i, j, new Color(value, value, value, 1));
            }
        }
    }
}