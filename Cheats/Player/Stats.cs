using System;
using System.Collections.Generic;
using UnityEngine;

namespace UmbraMenu.Cheats.Player
{
    public class Stats
    {
        static Stats() { }
        private Stats() { }
        public static Stats instance = new();
        public bool armorToggle, attackSpeedToggle, critToggle, damageToggle, moveSpeedToggle, regenToggle;

        public void LevelPlayersCrit(float critPerLvl)
        {
            UmbraMenu.LocalPlayerBody.levelCrit = critPerLvl;
        }

        public void LevelPlayersDamage(float damagePerLvl)
        {
            UmbraMenu.LocalPlayerBody.levelDamage = damagePerLvl;
        }

        public void SetPlayersAttackSpeed(float attackSpeed)
        {
            UmbraMenu.LocalPlayerBody.baseAttackSpeed = attackSpeed;
        }

        public void SetPlayersArmor(float armor)
        {
            UmbraMenu.LocalPlayerBody.baseArmor = armor;
        }

        public void SetPlayersMoveSpeed(float moveSpeed)
        {
            UmbraMenu.LocalPlayerBody.baseMoveSpeed = moveSpeed;
        }
    }
}
