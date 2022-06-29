using System.Collections.Generic;
using System.Linq;
using RoR2;
using UnityEngine;
using HarmonyLib;

namespace UmbraMenu.Menus
{
    // TODO: Move and Refactor to Cheats.World
    public class ChestItemList
    {
        public static List<PurchaseInteraction> purchaseInteractions = new List<PurchaseInteraction>();
        public static List<ChestBehavior> chests = new List<ChestBehavior>();
        public static bool onChestsEnable = true;
        private static bool isClosestChestEquip = false;

        public static void EnableChests()
        {
            if (onChestsEnable)
            {
                DumpInteractables(null);
                SceneDirector.onPostPopulateSceneServer += DumpInteractables;
                onChestsEnable = false;
            }
            else
            {
                return;
            }
        }

        public static void DisableChests()
        {
            if (!onChestsEnable)
            {
                SceneDirector.onPostPopulateSceneServer -= DumpInteractables;
                onChestsEnable = true;
            }
            else
            {
                return;
            }
        }

        private static void DumpInteractables(SceneDirector obj)
        {
            chests = UnityEngine.Object.FindObjectsOfType<ChestBehavior>().ToList();
        }

        public static ChestBehavior FindClosestChest()
        {
            Dictionary<float, ChestBehavior> chestsWithDistance = new Dictionary<float, ChestBehavior>();
            foreach (var chest in chests)
            {
                if (chest && chest.dropPickup != null)
                {
                    float distanceToChest = Vector3.Distance(Camera.main.transform.position, chest.transform.position);
                    chestsWithDistance.Add(distanceToChest, chest);
                }
            }
            var keys = chestsWithDistance.Keys.ToList();
            keys.Sort();
            float leastDistance = keys[0];
            chestsWithDistance.TryGetValue(leastDistance, out ChestBehavior closestChest);
            return closestChest;
        }

        public static void RenderClosestChest()
        {
            ChestBehavior chest = FindClosestChest();
            Vector3 chestPosition = Camera.main.WorldToScreenPoint(chest.transform.position);
            var chestBoundingVector = new Vector3(chestPosition.x, chestPosition.y, chestPosition.z);
            if (chestBoundingVector.z > 0.01)
            {
                string dropNameColored = Util.GenerateColoredString(Language.GetString(PickupCatalog.GetPickupDef(chest.dropPickup).nameToken), PickupCatalog.GetPickupDef(chest.dropPickup).baseColor);
                float distanceToChest = Vector3.Distance(Camera.main.transform.position, FindClosestChest().transform.position);
                float width = 100f * (distanceToChest / 100);
                if (width > 125)
                {
                    width = 125;
                }
                float height = 100f * (distanceToChest / 100);
                if (height > 125)
                {
                    height = 125;
                }

                if (Render.renderInteractables)
                {
                    GUI.Label(new Rect(chestBoundingVector.x - 50f, (float)Screen.height - chestBoundingVector.y + 35f, 100f, 50f), $"Selected Chest", Styles.SelectedChestStyle);
                }
                else
                {
                    GUI.Label(new Rect(chestBoundingVector.x - 50f, (float)Screen.height - chestBoundingVector.y + 35f, 100f, 50f), $"Selected Chest\n{dropNameColored}", Styles.SelectedChestStyle);
                }
                ESPHelper.DrawBox(chestBoundingVector.x - width / 2, (float)Screen.height - chestBoundingVector.y - height / 2, width, height, new Color32(17, 204, 238, 255));
            }
        }

        public static void SetChestItem(ItemIndex itemIndex)
        {
            var chest = FindClosestChest();
            PickupIndex newPickupIndex = PickupCatalog.FindPickupIndex(itemIndex);
            Traverse.Create(chest).Field("<dropPickup>k__BackingField").SetValue(newPickupIndex);
        }

        public static void SetChestEquipment(EquipmentIndex equipmentIndex)
        {
            var chest = FindClosestChest();
            PickupIndex newPickupIndex = PickupCatalog.FindPickupIndex(equipmentIndex);
            Traverse.Create(chest).Field("<dropPickup>k__BackingField").SetValue(newPickupIndex);
        }

        public static bool CheckClosestChestEquip()
        {
            var chest = FindClosestChest();
            var equipmentDrop = PickupCatalog.GetPickupDef(chest.dropPickup).equipmentIndex;
            if (UmbraMenu.equipment.Contains(equipmentDrop) && equipmentDrop != EquipmentIndex.None)
            {
                return true;
            }
            return false;
        }
    }
}