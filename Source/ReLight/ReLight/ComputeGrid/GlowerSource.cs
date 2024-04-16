using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReLight
{
    public struct GlowerSource
    {
        public uint index;
        public uint active;
        public float radius;
        public float overlightRadius;
        public Color color;

        public GlowerSource(uint index)
        {
            this.index = index;
            active = 0;
            radius = 0;
            overlightRadius = 0;
            color = Color.clear;
        }

        public GlowerSource(uint index, uint active, float radius, float overlightRadius, Color color)
        {
            this.index = index;
            this.active = active;
            this.radius = radius;
            this.overlightRadius = overlightRadius;
            this.color = color;
        }
    }
}
