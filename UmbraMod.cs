using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Player = UmbraMenu.Cheats.Player;
using World = UmbraMenu.Cheats.World;
using Render = UmbraMenu.Cheats.Render;

// TODO: Move cheat bools to UmbraMod class or change client to allow set of "embedded" classes
namespace UmbraMenu
{
    public class UmbraMod : MonoBehaviour
    {
        #region Player Variables
        // Note: These have to be static
        public static CharacterMaster LocalPlayer;
        public static CharacterBody LocalPlayerBody;
        public static Inventory LocalPlayerInv;
        public static HealthComponent LocalHealth;
        public static SkillLocator LocalSkills;
        public static NetworkUser LocalNetworkUser;
        public static CharacterMotor LocalMotor;
        #endregion

        // TODO: Maybe make these all dictionaries like unlockables????
        #region Game Element Lists
        public readonly Dictionary<string, UnlockableDef> unlockables;
        public List<GameObject> bodyPrefabs;
        public List<EquipmentIndex> equipment;
        public List<ItemIndex> items;
        public List<BuffDef> buffs;
        public List<SpawnCard> spawnCards;
        public List<ItemIndex> bossItems;
        public List<ItemIndex> unreleasedItems;
        public List<EquipmentIndex> unreleasedEquipment;
        #endregion
        
        #region Cheat instances

        #region Player Category
        public Player.Main mainPlayerInstance = Player.Main.instance;
        public Cheats.Player.Inventory inventoryInstance = Player.Inventory.instance;
        public Player.Movement movementInstance = Player.Movement.instance;
        public Player.Stats statsInstance = Player.Stats.instance;
        #endregion
        
        #region World Category
        public World.Teleporter teleporterInstance = World.Teleporter.instance;
        #endregion
        
        #region Render Category
        public Render.Main mainRenderInstance = Render.Main.instance;
        #endregion
        
        #endregion

        #region Misc Variabled used in some features
        public bool characterCollected;
        public Scene currentScene;
        #endregion

        #region Events and their Handlers
        public delegate void StartEventHandler(object sender, EventArgs e);
        public event StartEventHandler OnStart;

        public delegate void UpdateEventHandler(object sender, EventArgs e);
        public event UpdateEventHandler OnUpdate;

        public delegate void FixedUpdateEventHandler(object sender, EventArgs e);
        public event FixedUpdateEventHandler OnFixedUpdate;

        public delegate void GUIUpdateEventHandler(object sender, EventArgs e);
        public event GUIUpdateEventHandler OnGUIUpdate;

        public delegate void CharacterUpdateEventHandler(object sender, EventArgs e);
        public event CharacterUpdateEventHandler OnCharacterUpdate;
        #endregion

        static UmbraMod() { }

        private UmbraMod()
        {
            try
            {
                unlockables = typeof(UnlockableCatalog).GetField<Dictionary<string, UnlockableDef>>("nameToDefTable");
                // TODO: Just use ...Catalog and reflections to get this info
                bodyPrefabs = Utility.GetBodyPrefabs();
                equipment = Utility.GetEquipment();
                items = Utility.GetItems();
                buffs = Utility.GetBuffs();
                spawnCards = Utility.GetSpawnCards();

                OnGUIUpdate += ESPRoutine;

                //OnStart += ResolutionCheckRoutine;
                
                OnUpdate += CharacterRoutine;
                OnUpdate += SkillsRoutine;
                OnUpdate += AimBotRoutine;
                OnUpdate += GodRoutine;
                OnUpdate += EquipCooldownRoutine;
                OnUpdate += FlightRoutine;
                OnUpdate += SprintRoutine;
                OnUpdate += JumpPackRoutine;

                OnFixedUpdate += UpdateCurrentSceneRoutine;
                OnFixedUpdate += UpdateHurtboxesRoutine;
                OnFixedUpdate += UpdateNearestChestRoutine;

                SceneManager.activeSceneChanged += OnSceneLoadedRoutine;
            }
            catch
            {
                Debug.Log("Error while loading UmbraMod");
            }
        }

        [CanBeNull] public static UmbraMod instance;

        #region Main Unity Functions
        public void OnGUI()
        {
            try { OnGUIUpdate?.Invoke(this, EventArgs.Empty); } catch (Exception e) { Loader.client.Handler.Logger?.Proxy.LogError(e); }
        }

        public void Start()
        {
            try { OnStart?.Invoke(this, EventArgs.Empty); } catch (Exception e) { Loader.client.Handler.Logger?.Proxy.LogError(e); }
        }

        public void Update()
        {
            try { OnUpdate?.Invoke(this, EventArgs.Empty); } catch (Exception e) { Loader.client.Handler.Logger?.Proxy.LogError(e); }
        }

        public void FixedUpdate()
        {
            try { OnFixedUpdate?.Invoke(this, EventArgs.Empty); } catch (Exception e) { Loader.client.Handler.Logger?.Proxy.LogError(e); }
        }
        #endregion

        #region Misc Functions

        public void GetCharacter()
        {
            var temp = characterCollected;
            try
            {
                if (InGameCheck())
                {
                    LocalNetworkUser = null;

                    foreach (NetworkUser readOnlyInstance in NetworkUser.readOnlyInstancesList)   
                    {
                        if (readOnlyInstance.isLocalPlayer)
                        {
                            LocalNetworkUser = readOnlyInstance;
                            LocalPlayer = LocalNetworkUser.master;
                            LocalPlayerInv = LocalPlayer.GetComponent<Inventory>(); //gets player inventory
                            LocalHealth = LocalPlayer.GetBody().GetComponent<HealthComponent>(); //gets players local health numbers
                            LocalSkills = LocalPlayer.GetBody().GetComponent<SkillLocator>(); //gets current for local character skills
                            LocalPlayerBody = LocalPlayer.GetBody().GetComponent<CharacterBody>(); //gets all stats for local character
                            LocalMotor = LocalPlayer.GetBody().GetComponent<CharacterMotor>();
                            var flag = LocalHealth.alive || LocalPlayer.isActiveAndEnabled || LocalPlayerBody.isActiveAndEnabled;
                            characterCollected = flag;
                        }
                    }
                }
            }
            catch
            {
                characterCollected = false;
            }

            try { if (temp != characterCollected) OnCharacterUpdate?.Invoke(this, EventArgs.Empty); } catch { }
        }

        public bool InGameCheck()
        {
            var inGame = currentScene.name != "title" && currentScene.name != "lobby" && currentScene.name != "" && currentScene.name != " ";
            return inGame;
        }
        #endregion

        #region Routines
        #region OnGUIUpdate Routines
        private void ESPRoutine(object sender, EventArgs e)
        {
            if (!characterCollected) return;
            if (Render.renderInteractables)
            {
                Render.RenderInteractables();
                Render.renderInteractables = true;
            }
            else
            {
                Render.renderInteractables = false;
            }

            if (Render.renderMobs)
            {
                Render.RenderMobs();
                Render.renderMobs = true;
            }
            else
            {
                Render.renderMobs = false;
            }

            if (Render.renderMods)
            {
                Render.RenderActiveMods();
                Render.renderMods = true;
            }
            else
            {
                Render.renderMods = false;
            }

            if (!Chests.onChestsEnable)
            {
                Render.RenderClosestChest();
            }
        }
        #endregion

        #region OnStart Routines
        #endregion

        #region OnUpdate Routines
        private void CharacterRoutine(object sender, EventArgs e)
        {
            GetCharacter();
        }

        private void EquipCooldownRoutine(object sender, EventArgs e)
        {
            if (Items.noEquipmentCD)
            {
                Items.NoEquipmentCooldown();
            }
        }

        private void SkillsRoutine(object sender, EventArgs e)
        {
            if (!characterCollected) return;
            if (Main.SkillToggle)
            {
                LocalSkills.ApplyAmmoPack();
            }
        }

        private void AimBotRoutine(object sender, EventArgs e)
        {
            if (Main.AimBotToggle)
            {
                Main.AimBot();
            }

        }

        private void GodRoutine(object sender, EventArgs e)
        {
            if (!characterCollected) return;
            if (Main.GodToggle)
            {
                Main.EnableGodMode();
            }
        }

        private void SprintRoutine(object sender, EventArgs e)
        {
            if (Movement.alwaysSprintToggle)
            {
                Movement.AlwaysSprint();
            }
        }

        private void FlightRoutine(object sender, EventArgs e)
        {
            if (Movement.flightToggle)
            {
                Movement.Flight();
            }
        }

        private void JumpPackRoutine(object sender, EventArgs e)
        {
            if (Movement.jumpPackToggle)
            {
                Movement.JumpPack();
            }
        }
        #endregion

        #region OnFixedUpdate Routines
        private void UpdateCurrentSceneRoutine(object sender, EventArgs e)
        {
            currentScene = SceneManager.GetActiveScene();
        }

        private void UpdateHurtboxesRoutine(object sender, EventArgs e)
        {
            if (!InGameCheck()) return;
            if (Render.renderMobs)
            {
                Render.hurtBoxes = Utility.GetHurtBoxes();
            }
        }

        private void UpdateNearestChestRoutine(object sender, EventArgs e)
        {
            try
            {
                if (InGameCheck() && !Chests.onChestsEnable)
                {
                    Chests.isClosestChestEquip = Chests.CheckClosestChestEquip();
                }
            }
            catch (Exception exc)
            {
                Loader.client.Handler.Logger?.Proxy.LogError(exc);
            }
        }
        #endregion

        #region Misc Routines
        public void OnSceneLoadedRoutine(Scene s1, Scene s2)
        {
            if (s2 == null) return;
            var inGame = s2.name != "title" && s2.name != "lobby" && s2.name != "" && s2.name != " ";
            if (inGame)
            {
                if (Render.renderInteractables)
                {
                    Render.DumpInteractables(null);
                }
            }
            else
            {
                if (Screen.height < 1080)
                {
                    Render.renderMods = false;
                }
            }
        }
        #endregion
        #endregion
    }
}