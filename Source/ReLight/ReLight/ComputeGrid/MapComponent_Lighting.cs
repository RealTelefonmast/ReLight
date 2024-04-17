using UnityEngine;
using Verse;

namespace ReLight.ComputeGrid;

public class MapComponent_Lighting : MapComponent
{
    private RenderTexture _lightRT;
    private ComputeShader _lightingShader;
    
    public MapComponent_Lighting(Map map) : base(map)
    {
        _lightingShader = Resources.Load<ComputeShader>("Shaders/Lighting");
        _lightRT = LightingLayers.GetFor(map);
        
        _lightRT.
    }
}