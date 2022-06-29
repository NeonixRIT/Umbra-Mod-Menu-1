using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;


namespace UmbraMenu.Cheats.World
{
    public class Teleporter
    {
        static Teleporter() { }
        private Teleporter() { }
        public static Teleporter instance = new();
        public void InstaTeleporter()
        {
            if (TeleporterInteraction.instance)
            {
                TeleporterInteraction.instance.holdoutZoneController.baseChargeDuration = 1;
            }
            else
            {
                var purchaseInteractions = UnityEngine.Object.FindObjectsOfType<PurchaseInteraction>().ToList();
                foreach (var holdoutZone in purchaseInteractions.Select(purchaseInteraction => purchaseInteraction.gameObject.GetComponent<HoldoutZoneController>()))
                {
                    holdoutZone.baseChargeDuration = 1;
                }
            }
        }

        public void SkipStage()
        {
            Run.instance.AdvanceStage(Run.instance.nextStageScene);
        }

        public void AddMountain()
        {
            TeleporterInteraction.instance.AddShrineStack();
        }

        public void SpawnGoldPortal()
        {
            if (!TeleporterInteraction.instance) return;
            TeleporterInteraction.instance.Network_shouldAttemptToSpawnGoldshoresPortal = true;
            TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal = true;
        }
        
        public void SpawnBluePortal()
        {
            if (!TeleporterInteraction.instance) return;
            TeleporterInteraction.instance.Network_shouldAttemptToSpawnShopPortal = true;
            TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal = true;
        }
        
        public void SpawnCelestialPortal()
        {
            if (!TeleporterInteraction.instance) return;
            TeleporterInteraction.instance.Network_shouldAttemptToSpawnMSPortal = true;
            TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal = true;
        }

        public void SpawnAllPortals()
        {
            SpawnGoldPortal();
            SpawnCelestialPortal();
            SpawnBluePortal();
        }
    }
}
