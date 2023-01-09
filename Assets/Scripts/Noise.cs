using System;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;


namespace Noises
{
    //The whole code is stolen from Shader Graph Generated code
    public static class Noise
    {
        private static Vector2 VoronoiRandomVectorFloat(Vector2 UV, float offset)
        {
            float2x2 m = new float2x2(15.27f, 47.63f, 99.41f, 89.98f);
            UV = frac(sin(mul(UV, m)));
            return new Vector2(
                sin(UV.y * +offset) * 0.5f + 0.5f,
                cos(UV.x * offset) * 0.5f + 0.5f
            );
        }


        public static void Voronoi(Vector2 UV, float AngleOffset, float CellDensity, out float Out,
            out float Cells)
        {
            Vector2 g = floor(UV * CellDensity);
            Vector2 f = frac(UV * CellDensity);
            float t = 8.0f;
            Vector3 res = new Vector3(8, 0, 0);

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    var lattice = new Vector2(x, y);
                    var offset = VoronoiRandomVectorFloat(lattice + g, AngleOffset);
                    var d = Vector2.Distance(lattice + offset, f);

                    if (d < res.x)
                    {
                        res = new Vector3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                        return;
                    }
                }
            }

            throw new InvalidOperationException();
        }
        
        public static Vector2 Coordinate(int index, int width, int height)
        {
            return new Vector2(
                index / width,
                index % width
            );
        }
    }
}