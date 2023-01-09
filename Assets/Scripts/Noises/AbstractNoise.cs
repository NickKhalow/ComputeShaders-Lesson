using System;
using UnityEngine;


namespace Noises
{
    public abstract class AbstractNoise : ScriptableObject, IDisposable
    {
        public virtual void Init(int width, int height)
        {
            
        }
        
        public abstract void Apply(in Texture2D texture2D, float angleOffset, float cellDensity);


        public virtual void Dispose() { }
    }
}