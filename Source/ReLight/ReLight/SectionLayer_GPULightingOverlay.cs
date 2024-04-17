using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ReLight
{
    public class SectionLayer_GPULightingOverlay : SectionLayer
    {
        private RenderTexture _mapRT;
        private Material _secMat;
        
        public SectionLayer_GPULightingOverlay(Section section) : base(section)
        {
            _mapRT = LightingLayers.GetFor(base.section.map);
            
            Shader shader = ;
            _secMat = new Material(shader);
            _secMat.SetTexture("_MainTex", _mapRT);
            _secMat.SetTextureOffset("_MainTex", new Vector2(0, 0));
            _secMat.SetTextureScale("_MainTex", new Vector2(1, 1));
        }

        public override void Regenerate()
        {
            var submesh = GetSubMesh(_secMat);
            this.subMeshes.Add(submesh);
        }

        public override void DrawLayer()
        {
            base.DrawLayer();
        }
    }
}
