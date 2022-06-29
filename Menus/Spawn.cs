using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UmbraMenu.Menus
{
    // TODO: Move and Refactor to Cheats.World
    public class Spawn
    {
        private List<GameObject> _spawnedObjects = new();

        //TODO: find SpawnCard by name
        private void SpawnSpawnCard(SpawnCard spawnCard, string desiredTeam, int minDistance, int maxDistance)
        {
            Enum.TryParse<TeamIndex>(desiredTeam, out var team);
            TeamCatalog.GetTeamDef(team);
            var localUser = LocalUserManager.GetFirstLocalUser();
            var body = localUser.cachedMasterController.master.GetBody().transform;
            if (!localUser.cachedMasterController || !localUser.cachedMasterController.master) return;
            var directorSpawnRequest = new DirectorSpawnRequest(spawnCard, new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                minDistance = minDistance,
                maxDistance = maxDistance,
                position = UmbraMenu.LocalPlayerBody.footPosition
            }, RoR2Application.rng)
            {
                ignoreTeamMemberLimit = true,
                teamIndexOverride = team,
                spawnCard =
                {
                    sendOverNetwork = true
                }
            };
            
            // TODO: Update this with latest list of new SpawnCards because this def changed
            var cardName = spawnCard.ToString();
            var category = "";
            if (cardName.Contains("MultiCharacterSpawnCard"))
            {
                cardName = cardName.Replace(" (RoR2.MultiCharacterSpawnCard)", "");
                category = "CharacterSpawnCard";
            }
            else if (cardName.Contains("CharacterSpawnCard"))
            {
                cardName = cardName.Replace(" (RoR2.CharacterSpawnCard)", "");
                category = "CharacterSpawnCard";
            }
            else if (cardName.Contains("InteractableSpawnCard"))
            {
                cardName = cardName.Replace(" (RoR2.InteractableSpawnCard)", "");
                category = "InteractableSpawnCard";
            }
            else if (cardName.Contains("BodySpawnCard"))
            {
                cardName = cardName.Replace(" (RoR2.BodySpawnCard)", "");
                category = "BodySpawnCard";
            }
            var path = $"SpawnCards/{category}/{cardName}";

            if (cardName.Contains("isc"))
            {
                var interactable = Resources.Load<SpawnCard>(path).DoSpawn(body.position + Vector3.forward * minDistance, body.rotation, directorSpawnRequest).spawnedInstance.gameObject;
                _spawnedObjects.Add(interactable);
            }
            else
            {
                DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            }
        }

        public static void KillAllMobs()
        {
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

            var survivorNames = SurvivorCatalog.allSurvivorDefs.Select(def => def.cachedName).ToList();


            bullseyeSearch.RefreshCandidates();
            var hurtBoxList = bullseyeSearch.GetResults();
            foreach (var hurtbox in hurtBoxList)
            {

                var mob = HurtBox.FindEntityObject(hurtbox);
                var mobName = mob.name.Replace("Body(Clone)", "");

                if (survivorNames.Contains(mobName)) continue;
                var health = mob.GetComponent<HealthComponent>();
                health.Suicide();
                Chat.AddMessage($"<color=yellow>Killed {mobName} </color>");
            }
        }

        public void DestroySpawnedObjects()
        {
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

            if (spawnedObjects != null)
            {
                foreach (var gameObject in spawnedObjects)
                {
                    UnityEngine.Object.Destroy(gameObject);
                    Chat.AddMessage($"<color=yellow>Destroyed {gameObject.name.Replace("(Clone)", "")} </color>");
                }
                spawnedObjects = new List<GameObject>();
            }
        }
    }
}
