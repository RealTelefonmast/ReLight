using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReLight.ComputeGrid
{
    internal class ComputeGrid_Glow
    {
        public ComputeShader shader;
        private RenderTexture rTex;

        public void Init()
        {
            rTex = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            rTex.enableRandomWrite = true;
            rTex.Create();
            shader = Resources.Load<ComputeShader>("Shaders/GlowGrid");
            rTex = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
            rTex.enableRandomWrite = true;
            rTex.Create();
        }
    }
}
