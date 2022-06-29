using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RoR2;

namespace UmbraMenu
{
    public static class Utility
    {
        public static bool CursorIsVisible()
        {
            for (int i = 0; i < RoR2.UI.MPEventSystem.readOnlyInstancesList.Count; i++)
            {
                var mpeventSystem = RoR2.UI.MPEventSystem.readOnlyInstancesList[i];
                if (mpeventSystem.isCursorVisible)
                {
                    return true;
                }
            }
            return false;
        }

        public static SurvivorIndex GetCurrentCharacter()
        {
            var bodyIndex = BodyCatalog.FindBodyIndex(UmbraMod.LocalPlayerBody);
            var survivorIndex = SurvivorCatalog.GetSurvivorIndexFromBodyIndex(bodyIndex);
            return survivorIndex;
        }

        #region Get Lists
        public static List<SurvivorDef> GetSurvivorDefs()
        {
            var result = SurvivorCatalog.allSurvivorDefs.ToList();
            return result;
        }

        public static List<GameObject> GetBodyPrefabs()
        {
            var bodyPrefabs = new List<GameObject>();

            for (var i = 0; i < BodyCatalog.allBodyPrefabs.Count(); i++)
            {
                var prefab = BodyCatalog.allBodyPrefabs.ElementAt(i);
                if (prefab.name != "ScavSackProjectile")
                {
                    bodyPrefabs.Add(prefab);
                }
            }
            return bodyPrefabs;
        }

        public static List<EquipmentIndex> GetEquipment()
        {
            var equipment = new List<EquipmentIndex>();

            var equip = new List<EquipmentIndex>();
            var lunar = new List<EquipmentIndex>();
            var other = new List<EquipmentIndex>();

            var equipColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Equipment);
            var lunarColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);

            foreach (var equipmentIndex in EquipmentCatalog.allEquipment)
            {
                var currentEquipColor = ColorCatalog.GetColor(EquipmentCatalog.GetEquipmentDef(equipmentIndex).colorIndex);
                if (currentEquipColor.Equals(equipColor)) // equipment
                {
                    equip.Add(equipmentIndex);
                }
                else if (currentEquipColor.Equals(lunarColor)) // lunar equipment
                {
                    lunar.Add(equipmentIndex);
                }
                else // other
                {
                    other.Add(equipmentIndex);
                }
            }
            UmbraMod.instance.unreleasedEquipment = other;
            var result = equipment.Concat(lunar).Concat(equip).Concat(other).ToList();
            return result;
        }

        public static List<ItemIndex> GetItems()
        {
            var items = new List<ItemIndex>();

            var boss = new List<ItemIndex>();
            var tier3 = new List<ItemIndex>();
            var tier2 = new List<ItemIndex>();
            var tier1 = new List<ItemIndex>();
            var lunar = new List<ItemIndex>();
            var voidt = new List<ItemIndex>();
            var other = new List<ItemIndex>();

            var bossColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItem);
            var tier3Color = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier3Item);
            var tier2Color = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier2Item);
            var tier1Color = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier1Item);
            var lunarColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
            var voidColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);

            foreach (ItemIndex itemIndex in ItemCatalog.allItems)
            {
                var itemColor = ColorCatalog.GetColor(ItemCatalog.GetItemDef(itemIndex).colorIndex);
                if (itemColor.Equals(bossColor)) // boss
                {
                    boss.Add(itemIndex);
                }
                else if (itemColor.Equals(tier3Color)) // tier 3
                {
                    tier3.Add(itemIndex);
                }
                else if (itemColor.Equals(tier2Color)) // tier 2
                {
                    tier2.Add(itemIndex);
                }
                else if (itemColor.Equals(tier1Color)) // tier 1
                {
                    tier1.Add(itemIndex);
                }
                else if (itemColor.Equals(lunarColor)) // lunar
                {
                    lunar.Add(itemIndex);
                }
                else if (itemColor.Equals(voidColor)) // Void
                {
                    voidt.Add(itemIndex);
                }
                else // Other
                {
                    other.Add(itemIndex);
                }
            }

            UmbraMod.instance.bossItems = boss;
            UmbraMod.instance.unreleasedItems = other;
            var result = items.Concat(boss).Concat(tier3).Concat(tier2).Concat(tier1).Concat(lunar).Concat(voidt).Concat(other).ToList();
            return result;
        }

        public static List<BuffDef> GetBuffs()
        {
            var buffs = new List<BuffDef>();

            var eliteBuff = new List<BuffDef>();
            var nonEliteBuff = new List<BuffDef>();
            var eliteDebuff = new List<BuffDef>();
            var nonEliteDebuff = new List<BuffDef>();
            var other = new List<BuffDef>();


            foreach (var buffDef in typeof(BuffCatalog).GetField<BuffDef[]>("buffDefs"))
            {
                switch (buffDef.isDebuff)
                {
                    case false when buffDef.isElite:
                        eliteBuff.Add(buffDef);
                        break;
                    case false when !buffDef.isElite:
                        nonEliteBuff.Add(buffDef);
                        break;
                    case true when buffDef.isElite:
                        eliteDebuff.Add(buffDef);
                        break;
                    case true when !buffDef.isElite:
                        nonEliteDebuff.Add(buffDef);
                        break;
                    default:
                        other.Add(buffDef);
                        break;
                }
            }
            var result = buffs.Concat(eliteBuff).Concat(nonEliteBuff).Concat(eliteDebuff).Concat(nonEliteDebuff).Concat(other).ToList();
            return result;
        }

        public static List<SpawnCard> GetSpawnCards()
        {
            var spawnCards = Resources.FindObjectsOfTypeAll<SpawnCard>().ToList();
            return spawnCards;
        }

        public static List<HurtBox> GetHurtBoxes()
        {
            string[] allowedBoxes = { "Golem", "Jellyfish", "Wisp", "Beetle", "Lemurian", "Imp", "HermitCrab", "ClayBruiser", "Bell", "BeetleGuard", "MiniMushroom", "Bison", "GreaterWisp", "LemurianBruiser", "RoboBallMini", "Vulture",  /* BOSSES */ "BeetleQueen2", "ClayDunestrider", "Titan", "TitanGold", "TitanBlackBeach", "Grovetender", "Gravekeeper", "Mithrix", "Aurelionite", "Vagrant", "MagmaWorm", "ImpBoss", "ElectricWorm", "RoboBallBoss", "Nullifier", "Parent", "Scav", "ScavLunar1", "ClayBoss", "LunarGolem", "LunarWisp", "Brother", "BrotherHurt" };
            var localUser = LocalUserManager.GetFirstLocalUser();
            var controller = localUser.cachedMasterController;
            if (!controller)
            {
                return null;
            }
            var body = controller.master.GetBody();
            if (!body)
            {
                return null;
            }

            var inputBank = body.GetComponent<InputBankTest>();
            var aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var bullseyeSearch = new BullseyeSearch();
            var team = body.GetComponent<TeamComponent>();
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.filterByLoS = false;
            bullseyeSearch.maxDistanceFilter = 125;
            bullseyeSearch.maxAngleFilter = 40f;
            bullseyeSearch.teamMaskFilter = TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(team.teamIndex);
            bullseyeSearch.RefreshCandidates();
            var hurtBoxList = bullseyeSearch.GetResults().ToList();

            return (from hurtBox in hurtBoxList let mobName = HurtBox.FindEntityObject(hurtBox).name.Replace("Body(Clone)", "")
                where allowedBoxes.Contains(mobName) select hurtBox).ToList();
        }
        #endregion
    }
}