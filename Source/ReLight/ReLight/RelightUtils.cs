using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Verse;

namespace ReLight;

public static class RelightUtils
{
    public static RelightSettings Settings => RelightMod.Mod.Settings;
    
    public static RenderTexture GenerateRenderTextureFor(Map map)
    {
        //Fetch Mod Settings
        
        //
        var pixelWidth = map.Size.x * RelightSettings.TileShadingPixels;
        var pixelHeight = map.Size.z * RelightSettings.TileShadingPixels;
        var rt = new RenderTexture(pixelWidth, pixelHeight,  0, RenderTextureFormat.ARGB32);
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }
}