using BepInEx;
using DrakiaXYZ.VersionChecker;
using HarmonyLib;
using System;
using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using Vector2 = UnityEngine.Vector2;

namespace IncreaseLookDirection;

[BepInPlugin("com.dirtbikercj.IncreaseLookDirection", "IncreaseLookDirection", "1.0.2")]
public class Plugin : BaseUnityPlugin
{
    public const int TarkovVersion = 35392;
    
    private void Awake()
    {
        if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
        {
            throw new Exception("Invalid EFT Version");
        }
        
        DontDestroyOnLoad(this);
        
        new LookPatch().Enable();
    }

    /// <summary>
    /// This is a transpiler patch because we need to edit the code in a very specific fashion.
    /// </summary>
    public sealed class LookPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.Look));
        }

        [PatchPrefix]
        public static void Prefix()
        {
            EFTHardSettings.Instance.MOUSE_LOOK_HORIZONTAL_LIMIT = new Vector2(-90f, 90f);
        }
    }
}