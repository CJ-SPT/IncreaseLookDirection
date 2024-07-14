using BepInEx;
using DrakiaXYZ.VersionChecker;
using HarmonyLib;
using System;
using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using Vector2 = UnityEngine.Vector2;

namespace IncreaseLookDirection;

[BepInPlugin("com.dirtbikercj.IncreaseLookDirection", "IncreaseLookDirection", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public const int TarkovVersion = 30626;

    public static Plugin instance;

    private void Awake()
    {
        if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
        {
            throw new Exception("Invalid EFT Version");
        }

        instance = this;
        DontDestroyOnLoad(this);

        //var mouseLookHorizontalLimit = AccessTools.Field(typeof(EFTHardSettings), "MOUSE_LOOK_HORIZONTAL_LIMIT");
        //mouseLookHorizontalLimit.SetValue(mouseLookHorizontalLimit, new Vector2(-90f, 90f));
        
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