using System.Collections.Generic;
using UnityEngine;
using RoR2;

namespace UmbraMenu.Menus
{
    // TODO: Move and Refactor to Cheats.World
    public class SpawnList
    {
        public static bool onSpawnListEnable = true;
        public static void EnableSpawnList()
        {
            if (onSpawnListEnable)
            {
                DumpInteractables(null);
                SceneDirector.onPostPopulateSceneServer += DumpInteractables;
                onSpawnListEnable = false;
            }
            else
            {
                return;
            }
        }

        public static void DisableSpawnList()
        {
            if (!onSpawnListEnable)
            {
                SceneDirector.onPostPopulateSceneServer -= DumpInteractables;
                onSpawnListEnable = true;
            }
            else
            {
                return;
            }
        }

        private static void DumpInteractables(SceneDirector obj)
        {
            UmbraMenu.spawnCards = Utility.GetSpawnCards();
        }
    }
}
