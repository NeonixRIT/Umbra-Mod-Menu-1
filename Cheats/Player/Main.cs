using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoR2;
using System.Text;
using HarmonyLib;
using JetBrains.Annotations;


namespace UmbraMenu.Cheats.Player
{
    public class Main
    {
        static Main() { }
        private Main() { }
        public static Main instance = new();
        
        // TODO: maybe get body prefab from string or leave as is and use index method with other lists
        public void ChangeCharacter(int prefabIndex)
        {
            var newBody = BodyCatalog.FindBodyPrefab(UmbraMod.instance.bodyPrefabs[prefabIndex].name);
            if (newBody == null) return;
            var localUser = LocalUserManager.GetFirstLocalUser();
            if (localUser == null || localUser.cachedMasterController == null || localUser.cachedMasterController.master == null) return;
            var master = localUser.cachedMasterController.master;

            master.bodyPrefab = newBody;
            master.Respawn(master.GetBody().transform.position, master.GetBody().transform.rotation);
            UmbraMod.instance.GetCharacter();
        }
        
        // TODO: get BuffDef from string
        public void ApplyBuff(BuffDef buffDef)
        {
            var localUser = LocalUserManager.GetFirstLocalUser();
            if (localUser.cachedMasterController && localUser.cachedMasterController.master)
            {
                UmbraMod.LocalPlayerBody.AddBuff(buffDef);
            }
        }
        
        public void respawnPlayer()
        {
            UmbraMod.LocalPlayer.GetComponent<CharacterMaster>().RespawnExtraLife();
        }

        public void RemoveAllBuffs()
        {
            foreach (var buffDef in UmbraMod.instance.buffs)
            {
                try
                {
                    while (UmbraMod.LocalPlayerBody.HasBuff(buffDef))
                    {
                        UmbraMod.LocalPlayerBody.RemoveBuff(buffDef);
                    }
                }
                catch
                { 
                    // ignored
                }
            }
        }

        // self explanatory
        public void GiveXp(ulong xpToGive)
        {
            UmbraMod.LocalPlayer.GiveExperience(xpToGive);
        }

        public void GiveMoney(uint moneyToGive)
        {
            UmbraMod.LocalPlayer.GiveMoney(moneyToGive);
        }

        //uh, duh.
        public void GiveLunarCoins(uint coinsToGive)
        {
            UmbraMod.LocalNetworkUser.AwardLunarCoins(coinsToGive);
        }

        public void AimBot()
        {
            if (Utility.CursorIsVisible())
            {
                return;
            }

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

            var inputBank = body.GetComponent<InputBankTest>();
            var aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var bullseyeSearch = new BullseyeSearch();
            var team = body.GetComponent<TeamComponent>();
            bullseyeSearch.teamMaskFilter = TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(team.teamIndex);
            bullseyeSearch.filterByLoS = true;
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
            bullseyeSearch.maxDistanceFilter = float.MaxValue;
            bullseyeSearch.maxAngleFilter = 20f;// float.MaxValue;
            bullseyeSearch.RefreshCandidates();
            var hurtBox = bullseyeSearch.GetResults().FirstOrDefault();
            if (!hurtBox) return;
            var selectedBox = hurtBox;
            if (Utility.GetCurrentCharacter() == SurvivorCatalog.FindSurvivorIndex("Railgunner"))
            {
                foreach (var box in hurtBox.hurtBoxGroup.hurtBoxes)
                {
                    if (!box.isSniperTarget) continue;
                    selectedBox = box;
                    break;
                }
            }
            var direction = selectedBox.transform.position - aimRay.origin;
            inputBank.aimDirection = direction;
        }

        public void EnableGodMode(int godVersion)
        {
            switch (godVersion)
            {
                case 0:
                    {
                        // works
                        // Normal
                        UmbraMod.LocalHealth.godMode = true;
                        break;
                    }

                case 1:
                    {
                        // works
                        // Buff
                        if (!UmbraMod.LocalPlayerBody.HasBuff(BuffCatalog.FindBuffIndex("bdIntangible")))
                        {
                            UmbraMod.LocalPlayerBody.AddBuff(BuffCatalog.FindBuffIndex("bdIntangible"));
                        }
                        break;
                    }

                case 2:
                    {
                        // works
                        // Regen
                        UmbraMod.LocalHealth.Heal(float.MaxValue, new ProcChainMask(), false);
                        break;
                    }

                case 3:
                    {
                        // works
                        // Negative
                        Traverse.Create(UmbraMod.LocalHealth).Field("wasAlive").SetValue(false);
                        //UmbraMenu.LocalHealth.SetField<bool>("wasAlive", false);
                        break;
                    }

                case 4:
                    {
                        // works
                        // Revive
                        Traverse.Create(UmbraMod.LocalHealth).Field("wasAlive").SetValue(false);
                        var itemElCount = UmbraMod.LocalPlayerInv.GetItemCount(ItemCatalog.FindItemIndex("ExtraLife"));
                        var itemElcCount = UmbraMod.LocalPlayerInv.GetItemCount(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                        if (UmbraMod.LocalHealth.health < 1)
                        {
                            if (itemElCount == 0)
                            {
                                ItemList.GiveItem(ItemCatalog.FindItemIndex("ExtraLife"));
                                Traverse.Create(UmbraMod.LocalHealth).Field("wasAlive").SetValue(true);
                            }
                        }
                        if (itemElcCount > 0)
                        {
                            UmbraMod.LocalPlayerInv.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"), itemElcCount);
                        }
                        if (itemElCount > 0)
                        {
                            UmbraMod.LocalPlayerInv.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"), itemElCount);
                        }
                        break;
                    }
            }
        }

        public void DisabledGodMode(int godVersion)
        {
            switch (godVersion)
            {
                case 0:
                    {
                        UmbraMod.LocalHealth.godMode = false;
                        break;
                    }

                case 1:
                    {
                        UmbraMod.LocalPlayerBody.RemoveBuff(BuffCatalog.FindBuffIndex("bdIntangible"));
                        break;
                    }

                case 3:
                    {
                        if (UmbraMod.LocalHealth.health < 0)
                        {
                            UmbraMod.LocalHealth.health = 1;
                        }
                        Traverse.Create(UmbraMod.LocalHealth).Field("wasAlive").SetValue(true);
                        break;
                    }

                case 4:
                    {
                        if (UmbraMod.LocalHealth.health < 0)
                        {
                            UmbraMod.LocalHealth.health = 1;
                        }
                        Traverse.Create(UmbraMod.LocalHealth).Field("wasAlive").SetValue(true);
                        var itemElCount = UmbraMod.LocalPlayerInv.GetItemCount(ItemCatalog.FindItemIndex("ExtraLife"));
                        var itemElcCount = UmbraMod.LocalPlayerInv.GetItemCount(ItemCatalog.FindItemIndex("ExtraLifeConsumed"));
                        if (itemElcCount > 0)
                        {
                            UmbraMod.LocalPlayerInv.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"), itemElcCount);
                        }
                        if (itemElCount > 0)
                        {
                            UmbraMod.LocalPlayerInv.RemoveItem(ItemCatalog.FindItemIndex("ExtraLifeConsumed"), itemElCount);
                        }
                        break;
                    }
            }
        }
        
        // TODO: Separate this into multiple functions to unlock specifics
        public void UnlockAll()
        {
            // TODO: split into separate functions
            //This is needed to unlock logs
            var unlockables = UmbraMod.instance.unlockables;
            foreach (var unlockable in unlockables)
            {
                NetworkUser networkUser = Util.LookUpBodyNetworkUser(UmbraMod.LocalPlayerBody);
                if (networkUser)
                {
                    networkUser.ServerHandleUnlock(unlockable.Value);
                }
            }

            //Gives all achievements.
            var achievementManager = AchievementManager.GetUserAchievementManager(LocalUserManager.GetFirstLocalUser());
            foreach (var achievement in AchievementManager.allAchievementDefs)
            {
                achievementManager.GrantAchievement(achievement);
            }

            //Give all survivors
            var profile = LocalUserManager.GetFirstLocalUser().userProfile;
            foreach (var survivor in SurvivorCatalog.allSurvivorDefs)
            {
                if (profile.statSheet.GetStatValueDouble(RoR2.Stats.PerBodyStatDef.totalTimeAlive, survivor.bodyPrefab.name) == 0.0)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.totalTimeAlive.FindStatDef(survivor.bodyPrefab.name), "0.1");
                if (profile.statSheet.GetStatValueULong(RoR2.Stats.PerBodyStatDef.totalWins, survivor.bodyPrefab.name) == 0L)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.totalWins.FindStatDef(survivor.bodyPrefab.name), "1");
                if (profile.statSheet.GetStatValueULong(RoR2.Stats.PerBodyStatDef.timesPicked, survivor.bodyPrefab.name) == 0L)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.timesPicked.FindStatDef(survivor.bodyPrefab.name), "1");
            }

            //All items and equipments
            foreach (var itemIndex in ItemCatalog.allItems)
            {
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex(itemIndex));
            }

            foreach (var equipmentIndex in EquipmentCatalog.allEquipment)
            {
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex(equipmentIndex));
            }

            //All Eclipse unlockables as well
            var stringBuilder = HG.StringBuilderPool.RentStringBuilder();
            foreach (var survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                for (var i = 2; i < 9; i++)
                {
                    stringBuilder.Clear().Append("Eclipse.").Append(survivorDef.cachedName).Append(".").AppendInt(i, 0U);
                    var unlockableDef = UnlockableCatalog.GetUnlockableDef(stringBuilder.ToString());
                    var networkUser = Util.LookUpBodyNetworkUser(UmbraMod.LocalPlayerBody);
                    if (!networkUser) continue;
                    if (unlockableDef != null) networkUser.ServerHandleUnlock(unlockableDef);
                }
            }
        }
    }
}

