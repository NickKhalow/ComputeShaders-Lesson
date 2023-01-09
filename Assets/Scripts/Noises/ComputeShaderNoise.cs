using UnityEngine;


namespace Noises
{
    [CreateAssetMenu(fileName = "Compute Shader Noise", menuName = "Noise/Compute Shader", order = 0)]
    public class ComputeShaderNoise : AbstractNoise
    {
        [SerializeField] private ComputeShader computeShader;
        private int kernelIndex;
        private ComputeBuffer textureBuffer;
        private int sizeX, sizeY, sizeZ;
        private RenderTexture renderTexture;


        public override void Init(int width, int height)
        {
            var size = width * height;
            textureBuffer = new ComputeBuffer(size, sizeof(float) * 4);
            kernelIndex = computeShader.FindKernel("Voronoi");

            computeShader.GetKernelThreadGroupSizes(kernelIndex, out var localX, out var localY, out var localZ);
            sizeX = (int) localX;
            sizeY = (int) localY;
            sizeZ = (int) localZ;

            renderTexture = new RenderTexture(width, height, 32)
            {
                enableRandomWrite = true
            };
            computeShader.SetTexture(kernelIndex, "Result", renderTexture);
        }


        public override void Apply(in Texture2D texture2D, float angleOffset, float cellDensity)
        {
            computeShader.SetFloat("time", angleOffset);
            computeShader.SetFloat("size", cellDensity);

            //var wholeCount = texture2D.width * texture2D.height;

            computeShader.Dispatch(kernelIndex, texture2D.width / sizeX, texture2D.height / sizeY, 1);

            // computeShader.Dispatch(
            //     kernelIndex,
            //     wholeCount / sizeX,
            //     wholeCount / sizeY,
            //     1
            // );

            toTexture2D(renderTexture, texture2D);
        }


        public static void toTexture2D(RenderTexture rTex, Texture2D tex)
        {
            var old_rt = RenderTexture.active;
            RenderTexture.active = rTex;

            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.mipMapBias = 0;
            tex.Apply();

            RenderTexture.active = old_rt;
        }


        public override void Dispose()
        {
            textureBuffer.Dispose();
        }
    }
}