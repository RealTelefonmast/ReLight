using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ReLight;

public class SectionLayer_GPULightingOverlay : SectionLayer
{
    private RenderTexture _mapRT;
    private Mesh mesh;
    private Material _secMat;

    public SectionLayer_GPULightingOverlay(Section section) : base(section)
    {
        _mapRT = LightingLayers.GetFor(base.section.map);
        _secMat = new Material(RelightDefOf.RelightLightMapCutout.Shader);
            
        _secMat.SetTexture(RelightShaderProps.MainTex, RelightUtils.GetTextureForSection(section));
        _secMat.SetTexture(RelightShaderProps.LightMapTextureId, _mapRT);
        RelightUtils.GetUVDataFor(section, out var offset, out var scale);
        _secMat.SetTextureOffset(RelightShaderProps.LightMapTextureId, offset);
        _secMat.SetTextureScale(RelightShaderProps.LightMapTextureId, scale);
    }

    public override void Regenerate()
    {
        mesh = GetSubMesh(_secMat).mesh;
    }

    public override void DrawLayer()
    {
        if (!Visible) return;
        Graphics.DrawMesh(mesh, Matrix4x4.identity, _secMat, 0);
    }
}