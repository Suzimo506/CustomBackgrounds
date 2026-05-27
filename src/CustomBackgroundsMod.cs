using MelonLoader;
using HarmonyLib;
using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.Database;
using UnityEngine;

namespace Suzimo.MuseDashMods.CustomBackgrounds
{
    public class CustomBackgroundsMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("CustomBackgroundsMod initialized! The chosen home background will persist into the song selection page.");
        }
    }

    [HarmonyPatch(typeof(BgsRoot), nameof(BgsRoot.RefreshBgAlbum))]
    internal class BgsRootPatch
    {
        private static void Postfix(BgsRoot __instance)
        {
            if (__instance.m_BgAlbumWeekFree != null)
            {
                var img1 = __instance.m_BgAlbumWeekFree.GetComponent<UnityEngine.UI.Image>();
                if (img1 != null) img1.enabled = false;
            }

            if (__instance.m_BgAlbumLock != null)
            {
                var img2 = __instance.m_BgAlbumLock.GetComponent<UnityEngine.UI.Image>();
                if (img2 != null) img2.enabled = false;
            }
            
            var img3 = __instance.GetComponent<UnityEngine.UI.Image>();
            if (img3 != null) img3.enabled = false;
        }
    }

    [HarmonyPatch(typeof(Il2CppAssets.Scripts.UI.Panels.PnlMenu), nameof(Il2CppAssets.Scripts.UI.Panels.PnlMenu.OnEnable))]
    internal class PnlMenuEnablePatch
    {
        private static void Postfix(Il2CppAssets.Scripts.UI.Panels.PnlMenu __instance)
        {
            var loader = UnityEngine.Object.FindObjectOfType<Il2CppAssets.Scripts.UI.StageBackground.LobbyBackgroundLoader>();
            if (loader != null)
            {
                var bgTransform = loader.transform.parent;
                // Move Bg out of PnlMenu and into the common parent (UI/Standerd)
                if (bgTransform != null && bgTransform.name == "Bg" && bgTransform.parent != null && bgTransform.parent.name == "PnlHome")
                {
                    bgTransform.SetParent(bgTransform.parent.parent, true);
                    bgTransform.SetAsFirstSibling();
                    MelonLogger.Msg("Moved Bg out of PnlMenu to UI/Standerd.");
                }
            }
        }
    }

    [HarmonyPatch(typeof(PnlStage), nameof(PnlStage.OnEnable))]
    internal class PnlStageEnablePatch
    {
        private static void Postfix(PnlStage __instance)
        {
            var loader = UnityEngine.Object.FindObjectOfType<Il2CppAssets.Scripts.UI.StageBackground.LobbyBackgroundLoader>();
            if (loader != null)
            {
                var bgTransform = loader.transform.parent;
                if (bgTransform != null && bgTransform.name == "Bg" && bgTransform.parent != null && bgTransform.parent.name == "PnlHome")
                {
                    bgTransform.SetParent(bgTransform.parent.parent, true);
                    bgTransform.SetAsFirstSibling();
                    MelonLogger.Msg("Moved Bg out of PnlMenu to UI/Standerd from PnlStage.");
                }
            }

            // Hide the default purple background of PnlStage itself so the Bg behind it shows through
            var pnlStageImg = __instance.GetComponent<UnityEngine.UI.Image>();
            if (pnlStageImg != null) 
            {
                pnlStageImg.enabled = false;
                MelonLogger.Msg("Disabled PnlStage default background Image.");
            }
        }
    }
}
