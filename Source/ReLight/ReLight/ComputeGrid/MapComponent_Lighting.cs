using System.IO;
using System.Runtime.InteropServices;
using HarmonyLib;
using ReLight.GPUData;
using TeleCore;
using TeleCore.Data.Events;
using Unity.Mathematics;
using UnityEngine;
using Verse;

namespace ReLight.ComputeGrid;

public class MapComponent_Lighting : MapComponent
{
    private RenderTexture _lightRT;
    private ComputeShader _lightingShader;
    private int2 _mapPixelSize;
    
    //
    private ComputeGrid<LightBlocker> _blockers;
    private ComputeGrid<GlowSource> _glowSources;
    
    
    public MapComponent_Lighting(Map map) : base(map)
    {
        _mapPixelSize = RelightUtils.MapPixelSize(map.Size);
        
        LongEventHandler.QueueLongEvent(ThreadSafeFinalize, string.Empty, false, null, false);
    }
    
    //IMPORTANT - Crashes the game otherwise
    private void ThreadSafeFinalize()
    {
        _lightingShader = TeleContentDB.LoadComputeShader("GlowFlooder");
        _lightRT = LightingLayers.GetFor(map);
        _lightingShader.SetTexture(0, "_LightMap", _lightRT);
        _lightingShader.SetVector("MAP_SIZE", new Vector4(map.Size.x, map.Size.z));
        _lightingShader.SetInt("MAP_INDICES", map.cellIndices.NumGridCells);
        _lightingShader.SetInt("PIXEL_FACTOR", _mapPixelSize.x);
        
        //
        _glowSources = new ComputeGrid<GlowSource>(map);
        _blockers = new ComputeGrid<LightBlocker>(map);
        
        GlobalEventHandler.ThingSpawned += Notify_ThingSpawned;
    }

    private void Notify_ThingSpawned(ThingStateChangedEventArgs args)
    {
        if (args.Thing.def.blockLight)
        {
            if (args.Thing.RotatedSize.x > 1 || args.Thing.RotatedSize.z > 1)
            {
                args.Thing.OccupiedRect().Do(AddBlocker);
            }
            else 
                AddBlocker(args.Thing.Position);
        }
    }

    #region GlowerChanges

    // public void Notify_AddGlower(CompGlower glower)
    // {
    //     glowSources.SetValue(glower.parent.Position, new GlowerSource((uint)glower.parent.Position.Index(Map), 1, glower.Props.glowRadius, glower.Props.overlightRadius, glower.Props.glowColor.ToColor));
    //     Updater();
    //
    //     GPUTools.CopyRenderTexture(renderTextureShader, renderTextureIngame);
    //         
    // }

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
            _lightingShader.SetBool("useDebugMouse", true);
            _lightingShader.SetVector("debugMousePos", UI.MouseMapPosition() * RelightUtils.TileShadingPixels);
            _lightingShader.Dispatch(0, map.Size.x, map.Size.z, 1);
        }

        base.MapComponentUpdate();
    }

    public void AddGlowSource(CompGlower glower)
    {
        var index = glower.parent.Position.Index(map);
        var source = new GlowSource
        {
            index = (uint)index,
            active = glower.ShouldBeLitNow,
            radius = glower.Props.glowRadius,
            overlightRadius = glower.Props.overlightRadius,
            color = glower.GlowColor.ToColor,
        };
        _glowSources.SetData(glower.parent.Position, source);
        _glowSources.SetBufferOn(_lightingShader, 0, "_GlowSources");
    }
    
    public void AddBlocker(IntVec3 pos)
    {
        var index = pos.Index(map);
        var blocker = new LightBlocker
        {
            Min = new int2(pos.x, pos.z),
            Max = new int2(pos.x + 1, pos.z + 1),
        };
        _blockers.SetData(pos, blocker);
        _blockers.SetBufferOn(_lightingShader, 0, "_Blockers");
    }
    
    public void SetSkyColor(SkyTarget target)
    {
        var colors = target.colors;
        _lightingShader.SetVector("SkyColor_Sky", colors.sky);
        _lightingShader.SetVector("SkyColor_Shadow", colors.shadow);
        _lightingShader.SetVector("SkyColor_Overlay", colors.overlay);
        _lightingShader.SetFloat("SkyColor_Saturation", colors.saturation);
    }
}