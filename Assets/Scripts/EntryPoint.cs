using Noises;
using System;
using UnityEngine;
using UnityEngine.UI;


public class EntryPoint : MonoBehaviour
{
    [SerializeField] private AbstractNoise noise;
    [SerializeField] private Vector2Int resolution = new(512, 512);
    [SerializeField] private float speed = 1;
    [SerializeField] private float cellDensity = 1;
    [Header("UI")] [SerializeField] private RawImage image;
    [Header("Debug")] [SerializeField] private Texture2D texture2D;


    private void Awake()
    {
        if (noise == null)
        {
            throw new NullReferenceException();
        }

        if (image == null)
        {
            throw new NullReferenceException();
        }

        texture2D = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, true);
        image.texture = texture2D;
        noise.Init(texture2D.width, texture2D.height);
    }


    private void Update()
    {
        noise.Apply(texture2D, speed * Time.time, cellDensity);
        texture2D.Apply();
        //enabled = false;
    }


    private void OnDestroy()
    {
        noise.Dispose();
    }
}