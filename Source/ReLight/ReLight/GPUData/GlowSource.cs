using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ReLight;

public struct GlowSource
{
    public uint index;
    public bool active;
    public float radius;
    public float overlightRadius;
    public Color color;
}