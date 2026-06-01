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
            var standerd = __instance.transform.name == "Standerd" ? __instance.transform : __instance.transform.parent;
            var bgTransform = standerd.Find("Bg");
            
            if (bgTransform == null)
            {
                var pnlHome = standerd.Find("PnlHome");
                if (pnlHome != null) bgTransform = pnlHome.Find("Bg");
            }

            if (bgTransform != null)
            {
                if (bgTransform.parent != null && bgTransform.parent.name == "PnlHome")
                {
                    bgTransform.SetParent(standerd, true);
                }
                
                bgTransform.SetAsFirstSibling();
                bgTransform.gameObject.SetActive(true);
            }
        }
    }

    [HarmonyPatch(typeof(PnlStage), nameof(PnlStage.OnEnable))]
    internal class PnlStageEnablePatch
    {
        private static void Postfix(PnlStage __instance)
        {
            var standerd = __instance.transform.name == "Standerd" ? __instance.transform : __instance.transform.parent;
            var bgTransform = standerd.Find("Bg");
            
            if (bgTransform == null)
            {
                var pnlHome = standerd.Find("PnlHome");
                if (pnlHome != null) bgTransform = pnlHome.Find("Bg");
            }

            if (bgTransform != null)
            {
                if (bgTransform.parent != null && bgTransform.parent.name == "PnlHome")
                {
                    bgTransform.SetParent(standerd, true);
                }
                
                bgTransform.SetAsFirstSibling();
                bgTransform.gameObject.SetActive(true);
            }

            // Hide the default purple background of PnlStage itself so the Bg behind it shows through
            var pnlStageImg = __instance.GetComponent<UnityEngine.UI.Image>();
            if (pnlStageImg != null) 
            {
                pnlStageImg.enabled = false;
                pnlStageImg.color = new UnityEngine.Color(0, 0, 0, 0); // Force invisible
            }
        }
    }

    [HarmonyPatch(typeof(PnlTroves), nameof(PnlTroves.OnEnable))]
    internal class PnlTrovesEnablePatch
    {
        private static void Postfix(PnlTroves __instance)
        {
            var standerd = UIUtils.FindStanderd(__instance.transform);
            var bgTransform = standerd?.Find("Bg");
            if (bgTransform != null)
            {
                bgTransform.gameObject.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(PnlTroves), nameof(PnlTroves.OnDisable))]
    internal class PnlTrovesDisablePatch
    {
        private static void Postfix(PnlTroves __instance)
        {
            var standerd = UIUtils.FindStanderd(__instance.transform);
            var bgTransform = standerd?.Find("Bg");
            if (bgTransform != null)
            {
                bgTransform.gameObject.SetActive(true);
            }
        }
    }

    public static class UIUtils
    {
        public static Transform? FindStanderd(Transform transform)
        {
            var current = transform;
            while (current != null)
            {
                if (current.name == "Standerd") return current;
                current = current.parent;
            }
            return null;
        }
    }
}
