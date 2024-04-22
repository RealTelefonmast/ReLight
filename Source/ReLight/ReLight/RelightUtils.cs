using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.Mathematics;
using UnityEngine;
using Verse;

namespace ReLight;

public static class RelightUtils
{
    private static Dictionary<CellRect, Texture2D> _sectionTextures = new Dictionary<CellRect, Texture2D>();
    
    public static RelightSettings Settings => RelightMod.Mod.Settings;
    
    public static int TileShadingPixels => Settings.PixelsPerTile;
    
    public static RenderTexture GenerateRenderTextureFor(Map map)
    {
        var pixelWidth = map.Size.x * TileShadingPixels;
        var pixelHeight = map.Size.z * TileShadingPixels;
        var rt = new RenderTexture(pixelWidth, pixelHeight,  0, RenderTextureFormat.ARGB32);
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }

    public static void GetUVDataFor(Section section, out Vector2 offset, out Vector2 scale)
    {
        var ms = section.map.Size;
        var mps = new Vector2(ms.x * TileShadingPixels, ms.z * TileShadingPixels);
        var sps = new Vector2(section.CellRect.Width * TileShadingPixels, section.CellRect.Height * TileShadingPixels);
        offset = new Vector2(section.botLeft.x * TileShadingPixels / mps.x, section.botLeft.z * TileShadingPixels / mps.y);
        scale = new Vector2(sps.x / mps.x, sps.y / mps.y);
    }

    public static Texture2D GetTextureForSection(Section section)
    {
        if (_sectionTextures.TryGetValue(section.bounds, out var tex))
        {
            return tex;
        }
        tex = new Texture2D(section.CellRect.Width * TileShadingPixels, section.CellRect.Height * TileShadingPixels, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Trilinear;
        _sectionTextures[section.bounds] = tex;
        tex.Apply();
        return tex;
    }

    public static int2 MapPixelSize(IntVec3 mapSize)
    {
        return new int2(mapSize.x * TileShadingPixels, mapSize.z * TileShadingPixels);
    }
}