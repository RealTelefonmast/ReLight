using Verse;

namespace ReLight;

public class RelightMod : Mod
{
    public static RelightMod Mod { get; private set; }
    public RelightSettings Settings => (RelightSettings)modSettings;
    
    public RelightMod(ModContentPack content) : base(content)
    {
        Mod = this;
        modSettings = new RelightSettings();
    }
}

public class RelightSettings : ModSettings
{
    private int _pixelDensity = 32;

    public int PixelsPerTile => _pixelDensity;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref _pixelDensity, "PixelsPerTile", 32);
    }
}