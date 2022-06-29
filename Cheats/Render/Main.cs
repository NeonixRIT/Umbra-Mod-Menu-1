using System;
using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UmbraMenu.monsoon;
using Renderer = UnityEngine.Renderer;

namespace UmbraMenu.Cheats.Render
{
    public class Main
    {
        static Main() { }
        private Main() { }
        public static Main instance = new();
        
        private static Camera camera;

        // TODO: Change these maybe?
        public static List<PurchaseInteraction> purchaseInteractions = new();
        public static List<BarrelInteraction> barrelInteractions = new();
        public static List<PressurePlateController> secretButtons = new();
        public static List<ScrapperController> scrappers = new();
        public static List<HurtBox> hurtBoxes;
        public static bool onRenderIntEnable = true, renderMobs, renderInteractables, renderMods = true;
        
        public void EnableInteractables()
        {
            if (!onRenderIntEnable) return;
            DumpInteractables(null);
            SceneDirector.onPostPopulateSceneServer += DumpInteractables;
            onRenderIntEnable = false;
        }

        public void DisableInteractables()
        {
            if (onRenderIntEnable) return;
            SceneDirector.onPostPopulateSceneServer -= DumpInteractables;
            onRenderIntEnable = true;
        }

        public void DumpInteractables(SceneDirector obj)
        {
            barrelInteractions = MonoBehaviour.FindObjectsOfType<BarrelInteraction>().ToList();
            purchaseInteractions = MonoBehaviour.FindObjectsOfType<PurchaseInteraction>().ToList();
            secretButtons = MonoBehaviour.FindObjectsOfType<PressurePlateController>().ToList();
            scrappers = MonoBehaviour.FindObjectsOfType<ScrapperController>().ToList();
        }

        public void DrawTeleporter() {
            camera = Camera.main;

            if (!TeleporterInteraction.instance) return;
            var teleporterInteraction = TeleporterInteraction.instance;
            var position = teleporterInteraction.transform.position;
            var distanceToObject = Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, position);
            var location = camera.WorldToScreenPoint(position);
            if (!(location.z > 0.01)) return;
            var distance = (int)distanceToObject;
            var friendlyName = "Teleporter";
            var status = "" + (
                teleporterInteraction.isIdle ? "Idle" :
                teleporterInteraction.isCharging ? "Charging" :
                teleporterInteraction.isCharged ? "Charged" :
                teleporterInteraction.isActiveAndEnabled ? "Idle" :
                teleporterInteraction.isIdleToCharging ? "Idle-Charging" :
                teleporterInteraction.isInFinalSequence ? "Final-Sequence" :
                "???");
            var boxText = $"{friendlyName}\n{status}\n{distance}m";
            monsoon.Renderer.DrawString(new Vector2(location.x, Screen.height - location.y),
                boxText, Styles.TeleporterStyle);
        }

        public void DrawInteractables()
        {
            for (var i = 0; i < purchaseInteractions.Count; i++)
            {
                var purchaseInteraction = purchaseInteractions[i];

                if (!purchaseInteraction.available) continue;
                string dropName = null;
                var chest = purchaseInteraction.gameObject.GetComponent<ChestBehavior>();
                if (chest)
                {
                    dropName = Util.GenerateColoredString(Language.GetString(chest.dropPickup.GetPickupNameToken()), chest.dropPickup.GetPickupColor());
                }

                var position = purchaseInteraction.transform.position;
                var distanceToObject = Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, position);
                var location = camera.WorldToScreenPoint(position);

                if (!(location.z > 0.01)) continue;
                var distance = (int)distanceToObject;
                var friendlyName = purchaseInteraction.GetDisplayName();
                var cost = purchaseInteraction.cost;

                string boxText;
                if (friendlyName.Contains("Mountain") || friendlyName.Contains("Combat") || friendlyName.Contains("Printer"))
                {
                    boxText = $"{friendlyName}\n{distance}m";
                }
                else
                {
                    boxText = dropName != null 
                        ? $"{friendlyName}\n{BuyingUnit(friendlyName, cost)}\n{distance}m\n{dropName}" 
                        : $"{friendlyName}\n{BuyingUnit(friendlyName, cost)}\n{distance}m";
                }
                monsoon.Renderer.DrawString(new Vector2(location.x, Screen.height - location.y),
                    boxText, ChooseStyle(friendlyName));

            }
        }

        public void DrawBarrels()
        {
            for (var i = 0; i < barrelInteractions.Count; i++)
            {
                var barrel = barrelInteractions[i];

                if (barrel.Networkopened) continue;
                var friendlyName = "Barrel";
                var location = camera.WorldToScreenPoint(barrel.transform.position);
                if (!(location.z > 0.01)) continue;
                float distance = (int)Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, barrel.transform.position);
                var boxText = $"{friendlyName}\n{distance}m";
                monsoon.Renderer.DrawString(new Vector2(location.x, Screen.height - location.y),
                    boxText, ChooseStyle(friendlyName));
            }
        }

        public void DrawPressurePlates()
        {
            for (int i = 0; i < secretButtons.Count; i++)
            {
                var secretButton = secretButtons[i];
                if (!secretButton) continue;
                var friendlyName = "Secret Button";
                var location = camera.WorldToScreenPoint(secretButton.transform.position);
                if (!(location.z > 0.01)) continue;
                float distance = (int)Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, secretButton.transform.position);
                var boxText = $"{friendlyName}\n{distance}m";
                monsoon.Renderer.DrawString(new Vector2(location.x, Screen.height - location.y),
                    boxText, ChooseStyle(friendlyName));
            }
        }

        public void DrawScrappers()
        {
            for (int i = 0; i < scrappers.Count; i++)
            {
                var scrapper = scrappers[i];
                if (!scrapper) continue;
                var friendlyName = "Scrapper";
                var location = camera.WorldToScreenPoint(scrapper.transform.position);
                if (!(location.z > 0.01)) continue;
                float distance = (int)Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, scrapper.transform.position);
                var boxText = $"{friendlyName}\n{distance}m";
                monsoon.Renderer.DrawString(new Vector2(location.x, Screen.height - location.y),
                    boxText, ChooseStyle(friendlyName));
            }
        }

        private static string BuyingUnit(string name, int cost)
        {
            if (name.Contains("Newt") || name.Contains("Lunar") || name.Contains("Order") || name.Contains("Slab"))
                return cost + "LC";
            if (name.Contains("Void") || name.Contains("Blood"))
                return cost + "%HP";
            return "$" + cost;
        }
        private static GUIStyle ChooseStyle(string name)
        {
            if (name.Contains("Newt"))
                return Styles.NewtStyle;
            if (name.Contains("Chest") || name.Contains("Multishop") || name.Contains("Printer"))
                return Styles.ChestStyle;
            if (name.Contains("Equipment"))
                return Styles.EquipmentStyle;
            if (name.Contains("Lunar"))
                return Styles.LunarStyle;
            if (name.Contains("Void"))
                return Styles.VoidStyle;
            if (name.Contains("Gold") || name.Contains("Chance"))
                return Styles.ChanceStyle;
            if (name.Contains("Blood"))
                return Styles.BloodStyle;
            if (name.Contains("Combat") || name.Contains("Order"))
                return Styles.CombatStyle;
            if (name.Contains("Mountain"))
                return Styles.MountainStyle;
            return name.Contains("Woods") ? Styles.WoodsStyle : Styles.DefaultStyle;
        }

        // TODO: USE Material, Textures, and Renderer components to make wall hack or cham or different types of Mob ESP
        public static void Mobs()
        {
            for (var i = 0; i < hurtBoxes.Count; i++)
            {
                var mob = HurtBox.FindEntityObject(hurtBoxes[i]);
                if (!mob) continue;
                var position = mob.transform.position;
                var location = camera.WorldToScreenPoint(position);
                var distanceToMob = Vector3.Distance(UmbraMenu.LocalPlayerBody.transform.position, position);
                if (!(location.z > 0.01)) continue;
                var width = 100f * (distanceToMob / 100);
                if (width > 125)
                {
                    width = 125;
                }
                var height = 100f * (distanceToMob / 100);
                if (height > 125)
                {
                    height = 125;
                }
                var mobName = mob.name.Replace("Body(Clone)", "");
                var mobDistance = (int)distanceToMob;
                var mobBoxText = $"{mobName}\n{mobDistance}m";
                GUI.Label(new Rect(location.x - 50f, Screen.height - location.y + 50f, 100f, 50f), mobBoxText, Styles.RenderMobsStyle);
                ESPHelper.DrawBox(location.x - width / 2, Screen.height - location.y - height / 2, width, height, new Color32(255, 0, 0, 255));
            }
        }
    }
}