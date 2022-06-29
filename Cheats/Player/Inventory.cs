using System;
using System.Collections.Generic;
using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace UmbraMenu.Cheats.Player
{
    public class Inventory
    {
        static Inventory() { }
        private Inventory() { }
        public static Inventory instance = new();
        
        public bool isDropItemForAll, isDropItemFromInventory, noEquipmentCD;

        private readonly WeightedSelection<List<ItemIndex>> _weightedSelection = BuildRollItemsDropTable();

        #region Drop Item Handle

        private const short HandleItemId = 99;

        private class DropItemPacket : MessageBase
        {
            public GameObject player;
            public ItemIndex itemIndex;

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(player);
                writer.Write((ushort)itemIndex);
            }

            public override void Deserialize(NetworkReader reader)
            {
                player = reader.ReadGameObject();
                itemIndex = (ItemIndex)reader.ReadUInt16();
            }
        }

        private static void SendDropItem(GameObject player, ItemIndex itemIndex)
        {
            NetworkServer.SendToAll(HandleItemId, new DropItemPacket
            {
                player = player,
                itemIndex = itemIndex
            });
        }

        [RoR2.Networking.NetworkMessageHandler(msgType = HandleItemId, client = true)]
        private static void HandleDropItem(NetworkMessage netMsg)
        {
            var dropItem = netMsg.ReadMessage<DropItemPacket>();
            var body = dropItem.player.GetComponent<CharacterBody>();
            
            Transform transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(dropItem.itemIndex), (transform = body.transform).position + Vector3.up * 1.5f, Vector3.up * 20f + transform.forward * 2f);
        }

        public static void DropItemMethod(ItemIndex itemIndex)
        {
            var user = LocalUserManager.GetFirstLocalUser();
            var networkClient = NetworkClient.allClients.FirstOrDefault();
            networkClient?.RegisterHandlerSafe(HandleItemId, HandleDropItem);
            SendDropItem(user.cachedBody.gameObject, itemIndex);
        }
        #endregion

        #region Drop Equipment Handle

        private const short HandleEquipmentId = 98;

        private class DropEquipmentPacket : MessageBase
        {
            public GameObject player;
            public EquipmentIndex equipmentIndex;
            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(player);
                writer.Write((ushort)equipmentIndex);
            }

            public override void Deserialize(NetworkReader reader)
            {
                player = reader.ReadGameObject();
                equipmentIndex = (EquipmentIndex)reader.ReadUInt16();
            }
        }

        private static void SendDropEquipment(GameObject player, EquipmentIndex equipmentIndex)
        {
            NetworkServer.SendToAll(HandleEquipmentId, new DropEquipmentPacket
            {
                player = player,
                equipmentIndex = equipmentIndex
            });
        }

        [RoR2.Networking.NetworkMessageHandler(msgType = HandleEquipmentId, client = true)]
        private static void HandleDropEquipment(NetworkMessage netMsg)
        {
            var dropEquipment = netMsg.ReadMessage<DropEquipmentPacket>();
            var body = dropEquipment.player.GetComponent<CharacterBody>();
            
            Transform transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(dropEquipment.equipmentIndex), (transform = body.transform).position + Vector3.up * 1.5f, Vector3.up * 20f + transform.forward * 2f);
        }

        public static void DropEquipmentMethod(EquipmentIndex equipmentIndex)
        {
            var user = RoR2.LocalUserManager.GetFirstLocalUser();
            var networkClient = NetworkClient.allClients.FirstOrDefault();
            networkClient?.RegisterHandlerSafe(HandleEquipmentId, HandleDropEquipment);
            SendDropEquipment(user.cachedBody.gameObject, equipmentIndex);
        }
        #endregion

        // Clears inventory, duh.
        public void Clear()
        {
            if (UmbraMenu.LocalPlayerInv)
            {
                // Loops through every item in ItemIndex enum
                foreach (ItemIndex itemIndex in CurrentInventory())
                {
                    // If an item exists, delete the whole stack of it
                    UmbraMenu.LocalPlayerInv.itemAcquisitionOrder.Remove(itemIndex);
                    UmbraMenu.LocalPlayerInv.ResetItem(itemIndex);
                    var itemCount = UmbraMenu.LocalPlayerInv.GetItemCount(itemIndex);
                    UmbraMenu.LocalPlayerInv.RemoveItem(itemIndex, itemCount);

                    // Destroys BeetleGuardAllies on inventory clear, other wise they dont get removed until next stage.
                    var localUser = LocalUserManager.GetFirstLocalUser();
                    var controller = localUser.cachedMasterController;
                    if (!controller)
                    {
                        return;
                    }
                    var body = controller.master.GetBody();
                    if (!body)
                    {
                        return;
                    }

                    var bullseyeSearch = new BullseyeSearch
                    {
                        filterByLoS = false,
                        maxDistanceFilter = float.MaxValue,
                        maxAngleFilter = float.MaxValue
                    };
                    bullseyeSearch.RefreshCandidates();
                    var hurtBoxList = bullseyeSearch.GetResults();
                    foreach (var hurtbox in hurtBoxList)
                    {
                        var mob = HurtBox.FindEntityObject(hurtbox);
                        var mobName = mob.name.Replace("Body(Clone)", "");
                        if (mobName != "BeetleGuardAlly") continue;
                        var health = mob.GetComponent<HealthComponent>();
                        health.Suicide();
                    }
                }
                UmbraMenu.LocalPlayerInv.SetEquipmentIndex(EquipmentIndex.None);
            }
            Main.RemoveAllBuffs();
        }

        // random items
        public void RollItems(int itemsToRoll)
        {
            try
            {
                if (itemsToRoll <= 0) return;
                for (var i = 0; i < itemsToRoll; i++)
                {
                    var list = _weightedSelection.Evaluate(UnityEngine.Random.value);
                    UmbraMenu.LocalPlayerInv.GiveItem(list[UnityEngine.Random.Range(0, list.Count)], 1);
                }
            }
            catch (ArgumentException)
            {
            }
        }

        //Builds loot table for RollItems()
        private WeightedSelection<List<ItemIndex>> BuildRollItemsDropTable()
        {
            var weightedSelection = new WeightedSelection<List<ItemIndex>>(8);

            var boss = UmbraMenu.bossItems;

            var tier1 = ItemCatalog.tier1ItemList.ToList();
            var tier2 = ItemCatalog.tier2ItemList.ToList();
            var tier3 = ItemCatalog.tier3ItemList.ToList();
            var lunar = ItemCatalog.lunarItemList.ToList();


            weightedSelection.AddChoice(tier1, 63.5f);
            weightedSelection.AddChoice(tier2, 27f);
            weightedSelection.AddChoice(tier3, 3.5f);
            weightedSelection.AddChoice(boss, 3.5f);
            weightedSelection.AddChoice(lunar, 2.5f);
            return weightedSelection;
        }

        //Gives all items
        public void GiveAllItems(int allItemsQuantity)
        {
            if (!UmbraMenu.LocalPlayerInv) return;
            foreach (var itemIndex in from itemIndex in UmbraMenu.items let itemDef = ItemCatalog.GetItemDef(itemIndex) 
                     let itemName = itemDef.name where itemName is not 
                         ("PlantOnHit" or "HealthDecay" or "TonicAffliction" or "BurnNearby" or "CrippleWardOnLevel" or "Ghost" or "ExtraLifeConsumed")
                     select itemIndex)
            {
                UmbraMenu.LocalPlayerInv.GiveItem(itemIndex, allItemsQuantity);
            }
        }

        //Does the same thing as the shrine of order. Orders all your items into stacks of several random items.
        public void StackInventory()
        {
            UmbraMenu.LocalPlayerInv.ShrineRestackInventory(Run.instance.runRNG);
        }

        //Sets equipment cooldown to 0 if its on cooldown
        public void NoEquipmentCooldown()
        {
            var equipment = UmbraMod.LocalPlayerInv.GetEquipment(UmbraMod.LocalPlayerInv.activeEquipmentSlot);

            if (equipment.chargeFinishTime != Run.FixedTimeStamp.zero)
            {
                UmbraMod.LocalPlayerInv.SetEquipment(new EquipmentState(equipment.equipmentIndex, Run.FixedTimeStamp.zero, equipment.charges), UmbraMod.LocalPlayerInv.activeEquipmentSlot);
            }
        }

        //Gets players current items to make sure they can drop item from inventory if enabled
        public List<ItemIndex> CurrentInventory()
        {
            string[] unreleasedItems = { "Count", "None" };

            return (from itemIndex in ItemCatalog.allItems let itemDef = ItemCatalog.GetItemDef(itemIndex) 
                let unreleasednullItem = unreleasedItems.Any(itemDef.name.Contains) 
                where !unreleasednullItem let itemCount = UmbraMenu.LocalPlayerInv.GetItemCount(itemIndex) 
                where itemCount > 0 
                select itemIndex).ToList();
        }
        
        // TODO: Get item def/index from string or use index method like with character prefabs
        public void GiveItem(ItemIndex itemIndex)
        {
            var localUser = LocalUserManager.GetFirstLocalUser();
            if (!localUser.cachedMasterController || !localUser.cachedMasterController.master) return;
            if (Items.isDropItemForAll)
            {
                Items.DropItemMethod(itemIndex);
            }
            else if (Items.isDropItemFromInventory)
            {
                if (Items.CurrentInventory().Contains(itemIndex))
                {
                    UmbraMenu.LocalPlayerInv.RemoveItem(itemIndex);
                    Items.DropItemMethod(itemIndex);
                }
                else
                {
                    Chat.AddMessage("<color=yellow> You do not have that item and therefore cannot drop it from your inventory.</color>");
                    Chat.AddMessage(" ");
                }
            }
            else
            {
                UmbraMenu.LocalPlayerInv.GiveItem(itemIndex);
            }
        }
        
        // TODO: change to get equipment index from string or use index method
        public void GiveEquipment(EquipmentIndex equipIndex)
        {
            var localUser = LocalUserManager.GetFirstLocalUser();
            if (!localUser.cachedMasterController || !localUser.cachedMasterController.master) return;
            if (Items.isDropItemForAll)
            {
                Items.DropEquipmentMethod(equipIndex);
            }
            else if (Items.isDropItemFromInventory)
            {
                if (UmbraMenu.LocalPlayerInv.currentEquipmentIndex == equipIndex)
                {
                    UmbraMenu.LocalPlayerInv.SetEquipmentIndex(EquipmentIndex.None);
                    Items.DropEquipmentMethod(equipIndex);
                }
                else
                {
                    Chat.AddMessage($"<color=yellow> You do not have that equipment and therefore cannot drop it.</color>");
                    Chat.AddMessage($" ");
                }
            }
            else
            {
                UmbraMenu.LocalPlayerInv.SetEquipmentIndex(equipIndex);
            }
        }
    }
}