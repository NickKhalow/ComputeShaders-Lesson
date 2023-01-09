using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace Noises
{
    [CreateAssetMenu(fileName = "Job System Noise", menuName = "Noise/Job System", order = 0)]
    public class JobSystemNoise : AbstractNoise
    {
        [SerializeField] private int innerLoopBatchCount = 4;
        private NativeArray<Color> colors;


        public override void Init(int width, int height)
        {
            colors = new NativeArray<Color>(width * height, Allocator.Persistent);
        }


        public override void Apply(in Texture2D texture2D, float angleOffset, float cellDensity)
        {
            new VoronoiJob(
                colors,
                texture2D.width,
                texture2D.height,
                angleOffset,
                cellDensity
            ).Schedule(
                texture2D.width * texture2D.height,
                innerLoopBatchCount
            ).Complete();
            texture2D.SetPixels(colors.ToArray());
        }


        public override void Dispose()
        {
            colors.Dispose();
        }

        private struct VoronoiJob : IJobParallelFor
        {
            [WriteOnly] private NativeArray<Color> colors;
            private readonly int width, height;
            private readonly float angleOffset;
            private readonly float cellDensity;


            public VoronoiJob(NativeArray<Color> colors,
                int width,
                int height,
                float angleOffset,
                float cellDensity)
            {
                this.colors = colors;
                this.width = width;
                this.height = height;
                this.angleOffset = angleOffset;
                this.cellDensity = cellDensity;
            }


            public void Execute(int index)
            {
                Noise.Voronoi(
                    Noise.Coordinate(
                        index,
                        width,
                        height
                    ),
                    angleOffset,
                    cellDensity,
                    out _,
                    out var value
                );
                colors[index] = new Color(value, value, value, 1);
            }
        }
    }
}