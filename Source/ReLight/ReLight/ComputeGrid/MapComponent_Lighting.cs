using Unity.Mathematics;
using UnityEngine;
using Verse;

namespace ReLight.ComputeGrid;

public class MapComponent_Lighting : MapComponent
{
    private RenderTexture _lightRT;
    private ComputeShader _lightingShader;
    private int2 _mapPixelSize;
    
    
    public MapComponent_Lighting(Map map) : base(map)
    {
        //TODO: Telecore compute shader getter and instancer
        _mapPixelSize = RelightUtils.MapPixelSize(map.Size);
        _lightingShader = Resources.Load<ComputeShader>("Shaders/Lighting");
        _lightRT = LightingLayers.GetFor(map);
    }

    #region GlowerChanges

    public void Notify_AddGlower(CompGlower glower)
    {
        glowSources.SetValue(glower.parent.Position, new GlowerSource((uint)glower.parent.Position.Index(Map), 1, glower.Props.glowRadius, glower.Props.overlightRadius, glower.Props.glowColor.ToColor));
        Updater();

        GPUTools.CopyRenderTexture(renderTextureShader, renderTextureIngame);
            
    }

    #endregion
    
    private void UpdateShader()
    {
        _lightingShader.SetTexture(0, "_LightMap", _lightRT);
        _lightingShader.SetBool("useDebugMouse", true);
        _lightingShader.SetVector("debugMousePos", UI.MouseMapPosition());
        _lightingShader.Dispatch(0, _mapPixelSize.x, _mapPixelSize.y, 1);
    }
    
    public override void MapComponentUpdate()
    {
        if (Find.CurrentMap == map)
        {
            _lightingShader.Dispatch(0, _mapPixelSize.x, _mapPixelSize.y, 1);
        }

        base.MapComponentUpdate();
    }
}