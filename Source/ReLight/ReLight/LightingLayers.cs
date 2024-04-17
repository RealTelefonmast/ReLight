using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ReLight;

internal static class LightingLayers
{
    private static Dictionary<Map, RenderTexture> _globaleRTs;
    
    static LightingLayers()
    {
        _globaleRTs = new Dictionary<Map, RenderTexture>();
    }
    
    internal static RenderTexture GetFor(Map map)
    {
        if (!_globaleRTs.TryGetValue(map, out var rt))
        {
            rt = RelightUtils.GenerateRenderTextureFor(map);
            _globaleRTs[map] = rt;
            return rt;
        }
        return rt;
    }
    
    internal static void Notify_Dispose()
    {
        _globaleRTs.Clear();
    }
}