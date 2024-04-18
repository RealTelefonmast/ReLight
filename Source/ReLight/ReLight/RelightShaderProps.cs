using UnityEngine;
using Verse;

namespace ReLight;

public class RelightShaderProps
{
    private static readonly string LightMapTexture = "_LightMap";
    public static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public static readonly int LightMapTextureId = Shader.PropertyToID(LightMapTexture);
}