using System.Collections.Generic;
using RimWorld;
using TeleCore;
using UnityEngine;
using Verse;

namespace ReLight;

public class SectionLayer_GPULightingOverlay : SectionLayer
{
    private RenderTexture _mapRT;
    private Mesh mesh;
    private Material _secMat;

    public override bool Visible => DebugViewSettings.drawLightingOverlay;

    public SectionLayer_GPULightingOverlay(Section section) : base(section)
    {
        _secMat = new Material(RelightDefOf.RelightLightMapCutout.Shader);
        //var sectionTex =RelightUtils.GetTextureForSection(section);
        Regenerate();
    }

    public override void Regenerate()
    {
	    _mapRT = LightingLayers.GetFor(base.section.map);
	    _secMat.SetTexture("_MainTex", _mapRT);
	    RelightUtils.GetUVDataFor(section, out var offset, out var scale);
	    _secMat.SetTextureOffset("_MainTex", offset); //RelightShaderProps.LightMapTextureId
	    _secMat.SetTextureScale("_MainTex", scale);
	    
	    /*var width = section.CellRect.Width;
	    var height = section.CellRect.Height;
	    var pos = section.botLeft;

	    var triangles = new int[6];
	    var verts = new Vector3[4];
	    var uvs = new Vector2[verts.Length];
	    
	    var x = pos.x;
	    var z = pos.z;
	    var vert = x + z * 1;
	    var tris = vert * 6;
	    var vecs = new Vector3[4];

	    int BL = 0,
		    BR = 0 + 1,
		    TL = 0 + 1 + 1,
		    TR = 0 + 1 + 2;
	    
	    verts[BL] = new Vector3(x, 0, z);
	    verts[BR] = new Vector3(x + width, 0, z);
	    verts[TL] = new Vector3(x, 0, z + height);
	    verts[TR] = new Vector3(x + width, 0, z + height);
	    
	    uvs[BL] = new Vector2(verts[BL].x, verts[BL].z);
	    uvs[BR] = new Vector2(verts[BR].x, verts[BR].z);
	    uvs[TL] = new Vector2(verts[TL].x, verts[TL].z);
	    uvs[TR] = new Vector2(verts[TR].x, verts[TR].z);
	    
	    triangles[0] = BL;
	    triangles[1] = TL;
	    triangles[2] = BR;
	    triangles[3] = BR;
	    triangles[4] = TL;
	    triangles[5] = TR;*/
	    
	    mesh = new Mesh();
	    mesh.name = $"{section.botLeft}_mesh";
	    mesh.vertices = new []
	    {
		    new Vector3(0,0,0),
		    new Vector3(1,0,0),
		    new Vector3(0,0,1),
		    new Vector3(1,0,1),
	    };
	    mesh.uv = new []
	    {
		    new Vector2(0,0),
		    new Vector2(1,0),
		    new Vector2(0,1),
		    new Vector2(1,1),
	    };
	    
	    mesh.triangles = new []
	    {
		    0, 2, 1,
		    1, 2, 3
	    };
	    
    }

    public override void DrawLayer()
    {
        if (!Visible) return;
        Matrix4x4 matrix = Matrix4x4.TRS(section.botLeft.ToVector3() + new Vector3(0, AltitudeLayer.LightingOverlay.AltitudeFor(), 0), Quaternion.identity, Vector3.one * 17);
        Graphics.DrawMesh(mesh, matrix, _secMat, 0);
    }
}