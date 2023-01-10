#nullable enable
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Noises
{
    public class Example : MonoBehaviour
    {
        [SerializeField] private ComputeShader computeShader = null!;
        [SerializeField] private Vector2Int resolution;
        [SerializeField] private RawImage image = null!;
        [SerializeField] private float speed = 1;


        
        [Header("Debug")] [SerializeField] private int kernel;
        [SerializeField] private RenderTexture renderTexture = null!;
        private uint x, y;


        private void Awake()
        {
            renderTexture = new RenderTexture(resolution.x, resolution.y, 32)
            {
                enableRandomWrite = true
            };
            image.texture = renderTexture;

            kernel = computeShader.FindKernel("CertainExample");
            computeShader.SetTexture(kernel, "Result", renderTexture);
            computeShader.SetInt("width", resolution.x);
            computeShader.SetInt("height", resolution.y);
            computeShader.GetKernelThreadGroupSizes(kernel, out x, out y, out _);
        }


        private void Update()
        {
            computeShader.SetFloat("time", Time.time * speed);
            computeShader.Dispatch(
                kernel,
                resolution.x / (int) x,
                resolution.y / (int) y,
                1
            );
        }


        private void OnDestroy() { }
    }
}