using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleCore;
using UnityEngine;
using Verse;

namespace ReLight
{
    public class SectionLayer_GPULightingOverlay : SectionLayer
    {
        private Material _secMat;
        
        public SectionLayer_GPULightingOverlay(Section section) : base(section)
        {
            var shader = TeleContentDB.LoadComputeShader("");
            _secMat = new Material(shader);
        }

        public override void Regenerate()
        {

        }

        public override void DrawLayer()
        {
            base.DrawLayer();
        }
    }
}
