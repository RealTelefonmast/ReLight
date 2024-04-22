using System.Collections.Generic;
using Verse;

namespace ReLight.Glow;

internal static class RelightData
{
    private static Dictionary<Map, RelightGlowData> _mapGlowData = new Dictionary<Map, RelightGlowData>();
    
    internal static void InitGrid(GlowGrid grid)
    {
        _mapGlowData[grid.map] = new RelightGlowData(grid);
    }
    
    internal static void DeInitGrid(GlowGrid grid)
    {
        _mapGlowData.Remove(grid.map);
    }
    
    public static void Register(GlowGrid onGrid, CompGlower glower)
    {
        
    }
    
    public static void DeRegister(GlowGrid onGrid, CompGlower glower)
    {
        
    }
}

internal class RelightGlowData
{
    private GlowGrid grid;
    
    public Map Map => grid.map;
    
    public RelightGlowData(GlowGrid grid)
    {
        this.grid = grid;
    }
}