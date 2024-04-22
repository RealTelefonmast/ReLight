using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ReLight.ComputeGrid;
using ReLight.Glow;
using UnityEngine;
using Verse;

namespace ReLight;

internal static class GlowPatches
{
    #region Vanilla SectionLayer_LightingOverlay
    
    [HarmonyPatch(typeof(SectionLayer_LightingOverlay), nameof(SectionLayer_LightingOverlay.Regenerate))]
    internal static class LightingOverlay_Regenerate_Patch
    {
        public static bool Prefix(SectionLayer_LightingOverlay __instance)
        {
            //DESTROY IT
            //DESTROY THE LIGHT
            return false;
        }
    }
    
    [HarmonyPatch(typeof(SectionLayer_LightingOverlay), nameof(SectionLayer_LightingOverlay.GlowReportAt))]
    internal static class LightingOverlay_GlowReportAt_Patch
    {
        internal static bool Prefix(IntVec3 c, SectionLayer_LightingOverlay __instance, ref string __result)
        {
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof(SectionLayer_LightingOverlay), nameof(SectionLayer_LightingOverlay.Visible), MethodType.Getter)]
    internal static class LightingOverlay_Visible_Patch
    {
        internal static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }


    #endregion

    #region Glower Updates

    [HarmonyPatch(typeof(Map), nameof(Map.ConstructComponents))]
    internal static class GlowGrid_Constructor_Patch
    {
        internal static void Postfix(Map __instance)
        {
            RelightData.InitGrid(__instance.glowGrid);
        }
    }
    
    [HarmonyPatch(typeof(Map), nameof(Map.Dispose))]
    internal static class GlowGrid_Dispose_Patch
    {
        internal static void Prefix(Map __instance)
        {
            RelightData.DeInitGrid(__instance.glowGrid);
        }
    }
    
    [HarmonyPatch(typeof(GlowGrid), nameof(GlowGrid.RegisterGlower))]
    internal static class GlowGrid_RegisterGlower_Patch
    {
        public static void Postfix(GlowGrid __instance, CompGlower newGlow)
        {
            RelightData.Register(__instance, newGlow);
            __instance.map.GetComponent<MapComponent_Lighting>().AddGlowSource(newGlow);
        }
    }

    [HarmonyPatch(typeof(GlowGrid), nameof(GlowGrid.DeRegisterGlower))]
    internal static class GlowGrid_DeRegisterGlower_Patch
    {
        internal static void Postfix(GlowGrid __instance, CompGlower oldGlow)
        {
            RelightData.DeRegister(__instance, oldGlow);
        }
    }

    #endregion

    #region SkyColors

    [HarmonyPatch(typeof(SkyManager), nameof(SkyManager.SkyManagerUpdate))]
    internal static class SkyManager_SkyManagerUpdate_Patch
    {
        private static MethodInfo SetSkyColorToShaderMethod = AccessTools.Method(typeof(SkyManager_SkyManagerUpdate_Patch), nameof(SetSkyColorToShader));
        private static FieldInfo MapField = AccessTools.Field(typeof(SkyManager), "map");
        
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool loadedFirst = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldsfld && !loadedFirst)
                {
                    Log.Message($"Patching SkyManager.SkyManagerUpdate {instruction.opcode}: {((FieldInfo)instruction.operand).FieldType}");
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, MapField);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, SetSkyColorToShaderMethod);
                    loadedFirst = true;
                }
                yield return instruction;
            }
        }

        private static void SetSkyColorToShader(Map map, SkyTarget skytarget)
        {
            map.GetComponent<MapComponent_Lighting>().SetSkyColor(skytarget);
        }
    }

    #endregion
}