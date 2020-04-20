﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Console = RoR2.Console;

namespace RoRCheats
{
    public class Main : MonoBehaviour
    {
        public const string
            NAME = "RoRCheats",
            GUID = "com.acher0ns." + NAME,
            VERSION = "1.0";

        #region Player Variables
        public static CharacterMaster LocalPlayer;
        public static CharacterBody LocalPlayerBody;
        public static Inventory LocalPlayerInv;
        public static HealthComponent LocalHealth;
        public static SkillLocator LocalSkills;
        public static NetworkUser LocalNetworkUser;
        public static CharacterMotor LocalMotor;
        #endregion

        #region Menu Checks
        public static bool _isMenuOpen = false;
        public static bool _ifDragged = false;
        public static bool _CharacterCollected = false;
        public static bool _isStatMenuOpen = false;
        public static bool _isTeleMenuOpen = false;
        public static bool _ItemToggle = false;
        public static bool _CharacterToggle = false;
        public static bool _isLobbyMenuOpen = false;
        public static bool _isEditStatsOpen = false;
        public static bool _isItemSpawnMenuOpen = false;
        public static bool _isPlayerMod = false;
        public static bool _isEquipmentSpawnMenuOpen = false;
        public static bool _isItemManagerOpen = false;
        public static bool enableRespawnButton = false;
        #endregion

        #region Button Styles / Toggles
        public static GUIStyle MainBgStyle, StatBgSytle, TeleBgStyle, OnStyle, OffStyle, LabelStyle, TitleStyle, BtnStyle, ItemBtnStyle, CornerStyle, DisplayStyle, BgStyle; //make new BgStyle for stats menu
        public static GUIStyle BtnStyle1, BtnStyle2, BtnStyle3;
        public static bool skillToggle, renderInteractables, damageToggle, critToggle, attackSpeedToggle, armorToggle, regenToggle, moveSpeedToggle, MouseToggle, NoclipToggle, listItems, equipmentCooldown, listBuffs, dropMenu, ShowUnlockAll;
        public static float delay = 0, widthSize = 400;
        #endregion

        #region UI Rects
        public static Rect mainRect;
        public static Rect statRect;
        public static Rect teleRect;
        public static Rect lobbyRect;
        public static Rect itemSpawnerRect;
        public static Rect equipmentSpawnerRect;
        public static Rect characterRect;
        public static Rect playerModRect;
        public static Rect itemManagerRect;
        #endregion

        public static Texture2D Image = null, ontexture, onpresstexture, offtexture, offpresstexture, cornertexture, backtexture, btntexture, btnpresstexture, btntexturelabel;
        public static Texture2D NewTexture2D { get { return new Texture2D(1, 1); } }

        #region Stats intervals / toggles
        public static int itemsToRoll = 5;
        public static int damagePerLvl = 10;
        public static int CritPerLvl = 1;
        public static float attackSpeed = 1;
        public static float armor = 0;
        public static float movespeed = 7;
        public static int jumpCount = 1;
        public static bool isDropItems = false;
        public static bool isDropItemForAll = false;
        public static bool alwaysSprint = false;
        public static bool aimBot = false;
        public static int allItemsQuantity = 5;
        public static ulong xpToGive = 100;
        public static uint moneyToGive = 100;
        public static uint coinsToGive = 100;
        #endregion

        public static int PlayerModBtnY, MainMulY, StatMulY, TeleMulY, LobbyMulY, itemSpawnerMulY, equipmentSpawnerMulY, CharacterMulY, PlayerModMulY, ItemManagerMulY, ItemManagerBtnY;

        public static Dictionary<String, Int32> nameToIndexMap = new Dictionary<String, Int32>();
        public static string[] Players = new string[16];

        public static Rect rect = new Rect(10, 10, 20, 20);

        public static int btnY, mulY;
        private Vector2 scrollViewVector = Vector2.zero;
        public Rect dropDownRect = new Rect(10, 10, 20, 20);

        private void OnGUI()
        {

            #region Original Code
            /*rect = GUI.Window(0, rect, new GUI.WindowFunction(SetBG), "", new GUIStyle());
            if (_isMenuOpen)
            {
                Cursor.visible = true;
                DrawMenu();
            }
            else
            {
                Cursor.visible = false;
            }
            if (_CharacterCollected)
            {
                ESPRoutine();
            }
            if (listItems)
            {
                ListItemsRoutine();
            }
            if (dropMenu)
            {
                DropMenuRoutine();
            }
            if (listBuffs)
            {
                ListBuffsRoutine();
            }*/
            #endregion

            #region GenerateMenus

            mainRect = GUI.Window(0, mainRect, new GUI.WindowFunction(SetMainBG), "", new GUIStyle());
            if (_isMenuOpen)
            {
                DrawAllMenus();
            }
            if (_isStatMenuOpen)
            {
                statRect = GUI.Window(1, statRect, new GUI.WindowFunction(SetStatsBG), "", new GUIStyle());
                DrawMenu.DrawStatsMenu(statRect.x, statRect.y, widthSize, StatMulY, MainBgStyle, LabelStyle);
            }
            if (_isTeleMenuOpen)
            {
                teleRect = GUI.Window(2, teleRect, new GUI.WindowFunction(SetTeleBG), "", new GUIStyle());
                DrawMenu.DrawTeleMenu(teleRect.x, teleRect.y, widthSize, TeleMulY, MainBgStyle, BtnStyle, LabelStyle);
                //Debug.Log("X : " + teleRect.x + " Y : " + teleRect.y);
            }
            if (_isLobbyMenuOpen)
            {
                lobbyRect = GUI.Window(3, lobbyRect, new GUI.WindowFunction(SetLobbyBG), "", new GUIStyle());
                DrawMenu.DrawManagmentMenu(lobbyRect.x, lobbyRect.y, widthSize, LobbyMulY, MainBgStyle, BtnStyle, LabelStyle);
                //Debug.Log("X : " + lobbyRect.x + " Y : " + lobbyRect.y);
            }
            /*if (_isItemSpawnMenuOpen)
            {
                itemSpawnerRect = GUI.Window(4, itemSpawnerRect, new GUI.WindowFunction(SetSpawnerBG), "", new GUIStyle());
                DrawMenu.DrawSpawnMenu(itemSpawnerRect.x, itemSpawnerRect.y, widthSize, itemSpawnerMulY, MainBgStyle, BtnStyle, LabelStyle);
                //Debug.Log("X : " + itemSpawnerRect.x + " Y : " + itemSpawnerRect.y);
            }*/

            if (_isPlayerMod)
            {
                playerModRect = GUI.Window(5, playerModRect, new GUI.WindowFunction(SetPlayerModBG), "", new GUIStyle());
                DrawMenu.DrawPlayerModMenu(playerModRect.x, playerModRect.y, widthSize, PlayerModMulY, MainBgStyle, BtnStyle, OnStyle, OffStyle, LabelStyle);
                //Debug.Log("X : " + playerModRect.x + " Y : " + playerModRect.y);
            }
            /*if (_isEquipmentSpawnMenuOpen)
            {
                equipmentSpawnerRect = GUI.Window(6, equipmentSpawnerRect, new GUI.WindowFunction(SetEquipmentBG), "", new GUIStyle());
                DrawMenu.DrawEquipmentMenu(equipmentSpawnerRect.x, equipmentSpawnerRect.y, widthSize, equipmentSpawnerMulY, MainBgStyle, BtnStyle, LabelStyle, OffStyle);
                //Debug.Log("X : " + equipmentSpawnerRect.x + " Y : " + equipmentSpawnerRect.y);
            }*/
            if (_CharacterToggle)
            {
                characterRect = GUI.Window(7, characterRect, new GUI.WindowFunction(SetCharacterBG), "", new GUIStyle());
                DrawMenu.CharacterWindowMethod(characterRect.x, characterRect.y, widthSize, CharacterMulY, MainBgStyle, BtnStyle, LabelStyle);
                //Debug.Log("X : " + characterRect.x + " Y : " + characterRect.y);
            }
            if (_isItemManagerOpen)
            {
                itemManagerRect = GUI.Window(8, itemManagerRect, new GUI.WindowFunction(SetItemManagerBG), "", new GUIStyle());
                DrawMenu.DrawItemManagementMenu(itemManagerRect.x, itemManagerRect.y, widthSize, ItemManagerMulY, MainBgStyle, BtnStyle, OnStyle, OffStyle, LabelStyle);
                //Debug.Log("X : " + itemManagerRect.x + " Y : " + itemManagerRect.y);
            }
            if (_CharacterCollected)
            {
                ESPRoutine();
            }
            #endregion
        }

        public void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            #region CondenseMenuValues

            mainRect = new Rect(10, 10, 20, 20); //start position
            statRect = new Rect(1626, 457, 20, 20); //start position
            teleRect = new Rect(10, 661, 20, 20); //start position
            lobbyRect = new Rect(10, 421, 20, 20); //start position
            itemSpawnerRect = new Rect(1503, 10, 20, 20); //start position
            equipmentSpawnerRect = new Rect(1503, 10, 20, 20); //start position
            characterRect = new Rect(1503, 10, 20, 20); //start position
            playerModRect = new Rect(424, 381, 20, 20); //start position
            itemManagerRect = new Rect(424, 10, 20, 20);

            /*mainRect = new Rect(10, 10, 20, 20); //start position
            statRect = new Rect(1626, 457, 20, 20); //start position
            teleRect = new Rect(426, 558, 20, 20); //start position
            lobbyRect = new Rect(10, 558, 20, 20); //start position
            itemSpawnerRect = new Rect(1503, 10, 20, 20); //start position
            equipmentSpawnerRect = new Rect(1503, 10, 20, 20); //start position
            characterRect = new Rect(1503, 10, 20, 20); //start position
            playerModRect = new Rect(426, 10, 20, 20); //start position
            itemManagerRect = new Rect(426, 10, 20, 20);*/

            #endregion

            #region Styles

            if (MainBgStyle == null)
            {
                MainBgStyle = new GUIStyle();
                MainBgStyle.normal.background = BackTexture;
                MainBgStyle.onNormal.background = BackTexture;
                MainBgStyle.active.background = BackTexture;
                MainBgStyle.onActive.background = BackTexture;
                MainBgStyle.normal.textColor = Color.white;
                MainBgStyle.onNormal.textColor = Color.white;
                MainBgStyle.active.textColor = Color.white;
                MainBgStyle.onActive.textColor = Color.white;
                MainBgStyle.fontSize = 18;
                MainBgStyle.fontStyle = FontStyle.Normal;
                MainBgStyle.alignment = TextAnchor.UpperCenter;
            }

            if (CornerStyle == null)
            {
                CornerStyle = new GUIStyle();
                CornerStyle.normal.background = BtnTexture;
                CornerStyle.onNormal.background = BtnTexture;
                CornerStyle.active.background = BtnTexture;
                CornerStyle.onActive.background = BtnTexture;
                CornerStyle.normal.textColor = Color.white;
                CornerStyle.onNormal.textColor = Color.white;
                CornerStyle.active.textColor = Color.white;
                CornerStyle.onActive.textColor = Color.white;
                CornerStyle.fontSize = 18;
                CornerStyle.fontStyle = FontStyle.Normal;
                CornerStyle.alignment = TextAnchor.UpperCenter;
            }

            if (LabelStyle == null)
            {
                LabelStyle = new GUIStyle();
                LabelStyle.normal.textColor = Color.white;
                LabelStyle.onNormal.textColor = Color.white;
                LabelStyle.active.textColor = Color.white;
                LabelStyle.onActive.textColor = Color.white;
                LabelStyle.fontSize = 18;
                LabelStyle.fontStyle = FontStyle.Normal;
                LabelStyle.alignment = TextAnchor.UpperCenter;
            }
            if (TitleStyle == null)
            {
                TitleStyle = new GUIStyle();
                TitleStyle.normal.textColor = Color.white;
                TitleStyle.onNormal.textColor = Color.white;
                TitleStyle.active.textColor = Color.white;
                TitleStyle.onActive.textColor = Color.white;
                TitleStyle.fontSize = 18;
                TitleStyle.fontStyle = FontStyle.Normal;
                TitleStyle.alignment = TextAnchor.UpperCenter;
            }

            if (OffStyle == null)
            {
                OffStyle = new GUIStyle();
                OffStyle.normal.background = OffTexture;
                OffStyle.onNormal.background = OffTexture;
                OffStyle.active.background = OffPressTexture;
                OffStyle.onActive.background = OffPressTexture;
                OffStyle.normal.textColor = Color.white;
                OffStyle.onNormal.textColor = Color.white;
                OffStyle.active.textColor = Color.white;
                OffStyle.onActive.textColor = Color.white;
                OffStyle.fontSize = 18;
                OffStyle.fontStyle = FontStyle.Normal;
                OffStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (OnStyle == null)
            {
                OnStyle = new GUIStyle();
                OnStyle.normal.background = OnTexture;
                OnStyle.onNormal.background = OnTexture;
                OnStyle.active.background = OnPressTexture;
                OnStyle.onActive.background = OnPressTexture;
                OnStyle.normal.textColor = Color.white;
                OnStyle.onNormal.textColor = Color.white;
                OnStyle.active.textColor = Color.white;
                OnStyle.onActive.textColor = Color.white;
                OnStyle.fontSize = 18;
                OnStyle.fontStyle = FontStyle.Normal;
                OnStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (BtnStyle == null)
            {
                BtnStyle = new GUIStyle();
                BtnStyle.normal.background = BtnTexture;
                BtnStyle.onNormal.background = BtnTexture;
                BtnStyle.active.background = BtnPressTexture;
                BtnStyle.onActive.background = BtnPressTexture;
                BtnStyle.normal.textColor = Color.white;
                BtnStyle.onNormal.textColor = Color.white;
                BtnStyle.active.textColor = Color.white;
                BtnStyle.onActive.textColor = Color.white;
                BtnStyle.fontSize = 18;
                BtnStyle.fontStyle = FontStyle.Normal;
                BtnStyle.alignment = TextAnchor.MiddleCenter;
            }
            if (ItemBtnStyle == null)
            {
                ItemBtnStyle = new GUIStyle();
                ItemBtnStyle.normal.background = BtnTexture;
                ItemBtnStyle.onNormal.background = BtnTexture;
                ItemBtnStyle.active.background = BtnPressTexture;
                ItemBtnStyle.onActive.background = BtnPressTexture;
                ItemBtnStyle.normal.textColor = Color.white;
                ItemBtnStyle.onNormal.textColor = Color.white;
                ItemBtnStyle.active.textColor = Color.white;
                ItemBtnStyle.onActive.textColor = Color.white;
                ItemBtnStyle.fontSize = 18;
                ItemBtnStyle.fontStyle = FontStyle.Normal;
                ItemBtnStyle.alignment = TextAnchor.MiddleCenter;
            }
            #endregion

            #region Original Styles
            /*if (BgStyle == null)
            {
                BgStyle = new GUIStyle();
                BgStyle.normal.background = BackTexture;
                BgStyle.onNormal.background = BackTexture;
                BgStyle.active.background = BackTexture;
                BgStyle.onActive.background = BackTexture;
                BgStyle.normal.textColor = Color.white;
                BgStyle.onNormal.textColor = Color.white;
                BgStyle.active.textColor = Color.white;
                BgStyle.fontSize = 18;
                BgStyle.fontStyle = FontStyle.Normal;
                BgStyle.onActive.textColor = Color.white;
                BgStyle.alignment = TextAnchor.UpperCenter;
            }

            if (CornerStyle == null)
            {
                CornerStyle = new GUIStyle();
                CornerStyle.normal.background = BtnTexture;
                CornerStyle.onNormal.background = BtnTexture;
                CornerStyle.active.background = BtnTexture;
                CornerStyle.onActive.background = BtnTexture;
                CornerStyle.normal.textColor = Color.white;
                CornerStyle.onNormal.textColor = Color.white;
                CornerStyle.active.textColor = Color.white;
                CornerStyle.onActive.textColor = Color.white;
                CornerStyle.fontSize = 18;
                CornerStyle.fontStyle = FontStyle.Normal;
                CornerStyle.alignment = TextAnchor.UpperCenter;
            }

            if (LabelStyle == null)
            {
                LabelStyle = new GUIStyle();
                LabelStyle.normal.textColor = Color.white;
                LabelStyle.onNormal.textColor = Color.white;
                LabelStyle.active.textColor = Color.white;
                LabelStyle.onActive.textColor = Color.white;
                LabelStyle.fontSize = 18;
                LabelStyle.fontStyle = FontStyle.Normal;
                LabelStyle.alignment = TextAnchor.UpperCenter;
            }

            if (OffStyle == null)
            {
                OffStyle = new GUIStyle();
                OffStyle.normal.background = OffTexture;
                OffStyle.onNormal.background = OffTexture;
                OffStyle.active.background = OffPressTexture;
                OffStyle.onActive.background = OffPressTexture;
                OffStyle.normal.textColor = Color.white;
                OffStyle.onNormal.textColor = Color.white;
                OffStyle.active.textColor = Color.white;
                OffStyle.onActive.textColor = Color.white;
                OffStyle.fontSize = 18;
                OffStyle.fontStyle = FontStyle.Normal;
                OffStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (OnStyle == null)
            {
                OnStyle = new GUIStyle();
                OnStyle.normal.background = OnTexture;
                OnStyle.onNormal.background = OnTexture;
                OnStyle.active.background = OnPressTexture;
                OnStyle.onActive.background = OnPressTexture;
                OnStyle.normal.textColor = Color.white;
                OnStyle.onNormal.textColor = Color.white;
                OnStyle.active.textColor = Color.white;
                OnStyle.onActive.textColor = Color.white;
                OnStyle.fontSize = 18;
                OnStyle.fontStyle = FontStyle.Normal;
                OnStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (BtnStyle == null)
            {
                BtnStyle = new GUIStyle();
                BtnStyle.normal.background = BtnTexture;
                BtnStyle.onNormal.background = BtnTexture;
                BtnStyle.active.background = BtnPressTexture;
                BtnStyle.onActive.background = BtnPressTexture;
                BtnStyle.normal.textColor = Color.white;
                BtnStyle.onNormal.textColor = Color.white;
                BtnStyle.active.textColor = Color.white;
                BtnStyle.onActive.textColor = Color.white;
                BtnStyle.fontSize = 18;
                BtnStyle.fontStyle = FontStyle.Normal;
                BtnStyle.alignment = TextAnchor.MiddleCenter;
            }*/
            #endregion
        }

        public void Update()
        {
            try
            {
                CharacterRoutine();
                CheckInputs();
                StatsRoutine();
                EquipCooldownRoutine();
                ModStatsRoutine();
                NoClipRoutine();
                SprintRoutine();
                AimBotRoutine();
            }
            catch (NullReferenceException)
            {

            }
        }

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _isMenuOpen = !_isMenuOpen;
                GetCharacter();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                GiveMoney();
            }
        }

        #region Routines
        private void CharacterRoutine()
        {
            if (!_CharacterCollected)
            {
                GetCharacter();
            }
        }

        private void ESPRoutine()
        {
            if (renderInteractables)
            {
                RenderInteractables();
            }
        }

        private void EquipCooldownRoutine()
        {
            if (equipmentCooldown)
            {
                ReduceEquipmentCooldown();
            }
        }

        private void StatsRoutine()
        {
            if (_CharacterCollected)
            {
                if (skillToggle)
                {
                    LocalSkills.ApplyAmmoPack();
                }
            }
        }
        private void AimBotRoutine()
        {
            if (aimBot)
                AimBot();
        }
        private void SprintRoutine()
        {
            if (alwaysSprint)
                AlwaysSprint();
        }

        private void NoClipRoutine()
        {
            if (NoclipToggle)
            {
                DoNoclip();
            }
        }

        private void ModStatsRoutine()
        {
            if (_CharacterCollected)
            {
                if (damageToggle)
                {
                    LevelPlayersDamage();
                }
                if (critToggle)
                {
                    LevelPlayersCrit();
                }
                if (attackSpeedToggle)
                {
                    SetplayersAttackSpeed();
                }
                if (armorToggle)
                {
                    SetplayersArmor();
                }
                if (moveSpeedToggle)
                {
                    SetplayersMoveSpeed();
                }
                LocalPlayerBody.RecalculateStats();

            }
        }
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "title")
            {
                RESETMENU();
            }
            else if (!LocalHealth.alive && scene.name != "title")
            {
                enableRespawnButton = true;
            }
        }

        #region SetBG

        public static void SetBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * mulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                    _isMenuOpen = !_isMenuOpen;
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetMainBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * MainMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isMenuOpen = !_isMenuOpen;
                    GetCharacter();
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetCharacterBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * CharacterMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _CharacterToggle = !_CharacterToggle;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetEquipmentBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * equipmentSpawnerMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isEquipmentSpawnMenuOpen = !_isEquipmentSpawnMenuOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetStatsBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * StatMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isStatMenuOpen = !_isStatMenuOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetTeleBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * TeleMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isTeleMenuOpen = !_isTeleMenuOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetLobbyBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * LobbyMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isLobbyMenuOpen = !_isLobbyMenuOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetEditStatBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * MainMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isEditStatsOpen = !_isEditStatsOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetSpawnerBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * MainMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isItemSpawnMenuOpen = !_isItemSpawnMenuOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetPlayerModBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * PlayerModMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isPlayerMod = !_isPlayerMod;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        public static void SetItemManagerBG(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * ItemManagerMulY), "", CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    _ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!_ifDragged)
                {
                    _isItemManagerOpen = !_isItemManagerOpen;
                }
                _ifDragged = false;
            }
            GUI.DragWindow();
        }

        #endregion SetBG

        public static void DrawAllMenus()
        {
            GUI.Box(new Rect(mainRect.x + 0f, mainRect.y + 0f, widthSize + 10, 50f + 45 * MainMulY), "", MainBgStyle);
            GUI.Label(new Rect(mainRect.x + 5f, mainRect.y + 5f, widthSize + 5, 95f), "RoRCheats" + "\n" + VERSION, TitleStyle);

            if (!_CharacterCollected)
            {
                DrawMenu.DrawNotCollectedMenu(LabelStyle, OnStyle, OffStyle);
            }
            if (_CharacterCollected)
            {
                DrawMenu.DrawMainMenu(mainRect.x, mainRect.y, widthSize, MainMulY, MainBgStyle, OnStyle, OffStyle, BtnStyle);
            }
        }

        // Textures

        #region Textures

        public static Texture2D BtnTexture
        {
            get
            {
                if (btntexture == null)
                {
                    btntexture = NewTexture2D;
                    btntexture.SetPixel(0, 0, new Color32(3, 155, 229, 255));
                    //byte[] FileData = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/BepInEx/plugins/RoRCheats/Resources/Images/ButtonStyle.png");
                    //btntexture.LoadImage(FileData);
                    btntexture.Apply();
                }
                return btntexture;
            }
        }

        public static Texture2D BtnTextureLabel
        {
            get
            {
                if (BtnTextureLabel == null)
                {
                    btntexture = NewTexture2D;
                    btntexture.SetPixel(0, 0, new Color32(255, 0, 0, 255));
                    btntexture.Apply();
                }
                return BtnTextureLabel;
            }
        }

        public static Texture2D BtnPressTexture
        {
            get
            {
                if (btnpresstexture == null)
                {
                    btnpresstexture = NewTexture2D;
                    btnpresstexture.SetPixel(0, 0, new Color32(2, 119, 189, 255));
                    btnpresstexture.Apply();
                }
                return btnpresstexture;
            }
        }

        public static Texture2D OnPressTexture
        {
            get
            {
                if (onpresstexture == null)
                {
                    onpresstexture = NewTexture2D;
                    onpresstexture.SetPixel(0, 0, new Color32(62, 119, 64, 255));
                    onpresstexture.Apply();
                }
                return onpresstexture;
            }
        }

        public static Texture2D OnTexture
        {
            get
            {
                if (ontexture == null)
                {
                    ontexture = NewTexture2D;
                    ontexture.SetPixel(0, 0, new Color32(79, 153, 82, 255));
                    ontexture.Apply();
                }
                return ontexture;
            }
        }

        public static Texture2D OffPressTexture
        {
            get
            {
                if (offpresstexture == null)
                {
                    offpresstexture = NewTexture2D;
                    offpresstexture.SetPixel(0, 0, new Color32(79, 79, 79, 255));
                    offpresstexture.Apply();
                }
                return offpresstexture;
            }
        }

        public static Texture2D OffTexture
        {
            get
            {
                if (offtexture == null)
                {
                    offtexture = NewTexture2D;
                    offtexture.SetPixel(0, 0, new Color32(99, 99, 99, 255));
                    //byte[] FileData = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/BepInEx/plugins/RoRCheats/Resources/Images/OffStyle.png");
                    //offtexture.LoadImage(FileData);
                    offtexture.Apply();
                }
                return offtexture;
            }
        }
        public static Texture2D BackTexture
        {
            get
            {
                if (backtexture == null)
                {
                    backtexture = NewTexture2D;
                    backtexture.SetPixel(0, 0, new Color32(42, 42, 42, 200));
                    backtexture.Apply();
                }
                return backtexture;
            }
        }

        public static Texture2D CornerTexture
        {
            get
            {
                if (cornertexture == null)
                {
                    cornertexture = NewTexture2D;
                    //ToHtmlStringRGBA  new Color(33, 150, 243, 1)
                    cornertexture.SetPixel(0, 0, new Color32(42, 42, 42, 0));

                    cornertexture.Apply();
                }
                return cornertexture;
            }
        }

        #endregion Textures
        
        private void ListItemsRoutine()
        {
            string[] unreleasedItems = { "AACannon", "PlasmaCore", "LevelBonus", "CooldownOnCrit", "PlantOnHit", "MageAttunement", "BoostHp", "BoostDamage", "CritHeal", "BurnNearby", "CrippleWardOnLevel", "ExtraLifeConsumed", "Ghost", "HealthDecay", "DrizzlePlayerHelper", "MonsoonPlayerHelper", "TempestOnKill", "Count" };
            string[] unreleasedEquipment = { "SoulJar", "AffixYellow", "AffixGold", "GhostGun", "OrbitalLaser", "Enigma", "LunarPotion", "SoulCorruptor", "Count" };
            int index = 1;
            if (listItems)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(dropDownRect.x, dropDownRect.y, widthSize + 10, Enum.GetNames(typeof(ItemIndex)).Length * 25 - 700), scrollViewVector, new Rect(0, 0, widthSize, (Enum.GetNames(typeof(ItemIndex)).Length + Enum.GetNames(typeof(EquipmentIndex)).Length) * 50));
                GUI.Box(new Rect(2 * rect.x, 2 * rect.y, widthSize + 10, Enum.GetNames(typeof(ItemIndex)).Length * 25), "", CornerStyle);
                foreach (string item in Enum.GetNames(typeof(ItemIndex)))
                {
                    bool unreleasedullItem = unreleasedItems.Any(item.Contains);
                    if (!unreleasedullItem)
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Item: {item}", BtnStyle))
                        {
                            ItemIndex itemName = (ItemIndex)Enum.Parse(typeof(ItemIndex), item);
                            GiveCustomItem(itemName);
                        }
                        index++;
                    }
                }
                index -= 1;
                foreach (string equipment in Enum.GetNames(typeof(EquipmentIndex)))
                {
                    bool unreleasedullEquipment = unreleasedEquipment.Any(equipment.Contains);
                    if (!unreleasedullEquipment)
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Equipment: {equipment}", BtnStyle))
                        {
                            EquipmentIndex EquipmentName = (EquipmentIndex)Enum.Parse(typeof(EquipmentIndex), equipment);
                            GiveCustomEquipment(EquipmentName);
                        }
                        index++;
                    }
                    else if (equipment == "OrbitalLaser")
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Equipment: {equipment} (Unreleased)", BtnStyle))
                        {
                            EquipmentIndex EquipmentName = (EquipmentIndex)Enum.Parse(typeof(EquipmentIndex), equipment);
                            GiveCustomEquipment(EquipmentName);
                        }
                        index++;
                    }
                }
                GUI.EndScrollView();
            }
            else
            {
                GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), Enum.GetNames(typeof(ItemIndex))[index]);
            }
        }

        private void DropMenuRoutine()
        {
            string[] unreleasedItems = { "AACannon", "PlasmaCore", "LevelBonus", "CooldownOnCrit", "PlantOnHit", "MageAttunement", "BoostHp", "BoostDamage", "CritHeal", "BurnNearby", "CrippleWardOnLevel", "ExtraLifeConsumed", "Ghost", "HealthDecay", "DrizzlePlayerHelper", "MonsoonPlayerHelper", "TempestOnKill", "Count" };
            string[] unreleasedEquipment = { "SoulJar", "AffixYellow", "AffixGold", "GhostGun", "OrbitalLaser", "Enigma", "LunarPotion", "SoulCorruptor", "Count" };
            int index = 1;
            if (dropMenu)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(dropDownRect.x, dropDownRect.y, widthSize + 10, Enum.GetNames(typeof(ItemIndex)).Length * 25 - 700), scrollViewVector, new Rect(0, 0, widthSize, (Enum.GetNames(typeof(ItemIndex)).Length + Enum.GetNames(typeof(EquipmentIndex)).Length) * 50));
                GUI.Box(new Rect(2 * rect.x, 2 * rect.y, widthSize + 10, Enum.GetNames(typeof(ItemIndex)).Length * 25), "", CornerStyle);
                foreach (string item in Enum.GetNames(typeof(ItemIndex)))
                {
                    bool unreleasedullItem = unreleasedItems.Any(item.Contains);
                    if (!unreleasedullItem)
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Item: {item}", BtnStyle))
                        {
                            ItemIndex itemName = (ItemIndex)Enum.Parse(typeof(ItemIndex), item);
                            DropItemMethod(itemName);
                        }
                        index++;
                    }
                }
                index -= 1;
                foreach (string equipment in Enum.GetNames(typeof(EquipmentIndex)))
                {
                    bool unreleasedullEquipment = unreleasedEquipment.Any(equipment.Contains);
                    if (!unreleasedullEquipment)
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Equipment: {equipment}", BtnStyle))
                        {
                            EquipmentIndex EquipmentName = (EquipmentIndex)Enum.Parse(typeof(EquipmentIndex), equipment);
                            DropEquipmentMethod(EquipmentName);
                        }
                        index++;
                    }
                    else if (equipment == "OrbitalLaser")
                    {
                        if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Equipment: {equipment} (Unreleased)", BtnStyle))
                        {
                            EquipmentIndex EquipmentName = (EquipmentIndex)Enum.Parse(typeof(EquipmentIndex), equipment);
                            DropEquipmentMethod(EquipmentName);
                        }
                        index++;
                    }
                }
                GUI.EndScrollView();
            }
            else
            {
                GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), Enum.GetNames(typeof(ItemIndex))[index]);
            }
        }

        private void ListBuffsRoutine()
        {
            int index = 1;
            if (listBuffs)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(dropDownRect.x, dropDownRect.y, widthSize + 10, Enum.GetNames(typeof(BuffIndex)).Length * 25 - 500), scrollViewVector, new Rect(0, 0, widthSize, Enum.GetNames(typeof(BuffIndex)).Length * 50));
                GUI.Box(new Rect(2 * rect.x, 2 * rect.y, widthSize + 10, Enum.GetNames(typeof(BuffIndex)).Length * 25), "", CornerStyle);
                foreach (string buff in Enum.GetNames(typeof(BuffIndex)))
                {
                    //bool unreleasedullItem = unreleasedItems.Any(item.Contains);
                    if (GUI.Button(new Rect(dropDownRect.x + 5, dropDownRect.y + 5 + 45 * index, widthSize, 40), $"Buff: {buff}", BtnStyle))
                    {
                        BuffIndex buffName = (BuffIndex)Enum.Parse(typeof(BuffIndex), buff);
                        GiveBuff(buffName);
                    }
                    index++;
                }
                GUI.EndScrollView();
            }
            else
            {
                GUI.Label(new Rect((dropDownRect.x - 95), dropDownRect.y, 300, 25), Enum.GetNames(typeof(BuffIndex))[index]);
            }
        }

        #endregion

        /*
        public static void DrawMenu()
        {


            GUI.Box(new Rect(rect.x + 0f, rect.y + 0f, widthSize + 10, 50f + 45 * mulY), "", BgStyle);
            GUI.Label(new Rect(rect.x + 0f, rect.y + 5f, widthSize + 10, 95f), "Umbra menu\nv 0.04", LabelStyle);


            if (_CharacterCollected)
            {
                #region Toggle Buttons
                // we dont have a god toggle bool, because we can just ref localhealth
                if (LocalHealth.godMode)
                {
                    if (GUI.Button(BtnRect(1, false), "God mode: ON", OnStyle))
                    {
                        LocalHealth.godMode = false;
                    }
                }
                else if (GUI.Button(BtnRect(1, false), "God mode: OFF", OffStyle))
                {
                    LocalHealth.godMode = true;
                }

                if (skillToggle)
                {
                    if (GUI.Button(BtnRect(2, false), "Infinite Skills: ON", OnStyle))
                    {
                        skillToggle = false;
                    }
                }
                else if (GUI.Button(BtnRect(2, false), "Infinite Skills: OFF", OffStyle))
                {
                    skillToggle = true;
                }

                if (renderInteractables)
                {
                    if (GUI.Button(BtnRect(3, false), "Interactables ESP: ON", OnStyle))
                    {
                        renderInteractables = false;
                    }
                }
                else if (GUI.Button(BtnRect(3, false), "Interactables ESP: OFF", OffStyle))
                {
                    renderInteractables = true;
                }

                if (GUI.Button(BtnRect(4, true), "Give Money: " + moneyToGive.ToString(), BtnStyle))
                {
                    GiveMoney();
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 80, rect.y + btnY, 40, 40), "-", OffStyle))
                {
                    if (moneyToGive > 100)
                        moneyToGive -= 100;
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 35, rect.y + btnY, 40, 40), "+", OffStyle))
                {
                    if (moneyToGive >= 100)
                        moneyToGive += 100;
                }
                if (GUI.Button(BtnRect(5, true), "Give Lunar Coins: " + coinsToGive.ToString(), BtnStyle))
                {
                    GiveLunarCoins();
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 80, rect.y + btnY, 40, 40), "-", OffStyle))
                {
                    if (coinsToGive > 10)
                        coinsToGive -= 10;
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 35, rect.y + btnY, 40, 40), "+", OffStyle))
                {
                    if (coinsToGive >= 10)
                        coinsToGive += 10;
                }
                if (GUI.Button(BtnRect(6, true), "Give Experience: " + xpToGive.ToString(), BtnStyle))
                {
                    giveXP();
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 80, rect.y + btnY, 40, 40), "-", OffStyle))
                {
                    if (xpToGive > 100)
                        xpToGive -= 100;
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 35, rect.y + btnY, 40, 40), "+", OffStyle))
                {
                    if (xpToGive >= 100)
                        xpToGive += 100;
                }
                if (GUI.Button(BtnRect(7, true), "Roll Items: " + itemsToRoll.ToString(), BtnStyle))
                {
                    RollItems(itemsToRoll.ToString());
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 80, rect.y + btnY, 40, 40), "-", OffStyle))
                {
                    if (itemsToRoll > 5)
                        itemsToRoll -= 5;
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 35, rect.y + btnY, 40, 40), "+", OffStyle))
                {
                    if (itemsToRoll >= 5)
                        itemsToRoll += 5;
                }
                if (GUI.Button(BtnRect(8, true), "Give All Items: " + allItemsQuantity.ToString(), BtnStyle))
                {
                    GiveAllItems();
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 80, rect.y + btnY, 40, 40), "-", OffStyle))
                {
                    if (allItemsQuantity > 1)
                        allItemsQuantity -= 1;
                }
                if (GUI.Button(new Rect(rect.x + widthSize - 35, rect.y + btnY, 40, 40), "+", OffStyle))
                {
                    if (allItemsQuantity >= 1)
                        allItemsQuantity += 1;
                }
                if (GUI.Button(BtnRect(9, false), "Stack Inventory", BtnStyle))
                {
                    StackInventory();
                }
                if (GUI.Button(BtnRect(10, false), "Clear Inventory", BtnStyle))
                {
                    ClearInventory();
                }
                if (equipmentCooldown)
                {
                    if (GUI.Button(BtnRect(11, false), "Equipment Cooldown: None", OnStyle))
                    {
                        equipmentCooldown = false;
                    }
                }
                else if (GUI.Button(BtnRect(11, false), "Equipment Cooldown: Normal", OffStyle))
                {
                    equipmentCooldown = true;
                    ReduceEquipmentCooldown();
                }
                if (listItems)
                {
                    if (GUI.Button(BtnRect(12, false), "Hide Items List", OnStyle))
                    {
                        listItems = false;
                    }
                }
                else if (GUI.Button(BtnRect(12, false), "Show Item List", OffStyle))
                {
                    listItems = true;
                }
                if (dropMenu)
                {
                    if (GUI.Button(BtnRect(13, false), "Hide Drop Items Menu", OnStyle))
                    {
                        dropMenu = false;
                    }
                }
                else if (GUI.Button(BtnRect(13, false), "Show Drop Items Menu", OffStyle))
                {
                    dropMenu = true;
                }
                if (listBuffs)
                {
                    if (GUI.Button(BtnRect(14, false), "Hide Buffs List", OnStyle))
                    {
                        listBuffs = false;
                    }
                }
                else if (GUI.Button(BtnRect(14, false), "Show Buffs List", OffStyle))
                {
                    listBuffs = true;
                }
                if (GUI.Button(BtnRect(15, false), "Remove All Buffs", BtnStyle))
                {
                    RemoveAllBuffs();
                }
                #endregion
            }
            else
            {
                GUI.Box(new Rect(rect.x, rect.y + 95f * mulY, widthSize + 10, 50f), "", BgStyle);
                GUI.Label(new Rect(rect.x, rect.y + 95f * mulY, widthSize + 10, 50f), "<color=yellow>Note: Buttons Will Appear in Match Only</color>", LabelStyle);
            }
        }*/

        // Rect for buttons
        // It automatically auto position buttons. There is no need to change it
        public static Rect BtnRect(int y, bool multiplyBtn)
        {
            mulY = y;
            if (multiplyBtn)
            {
                btnY = 5 + 45 * y;
                return new Rect(rect.x + 5, rect.y + 5 + 45 * y, widthSize - 90, 40);
            }
            return new Rect(rect.x + 5, rect.y + 5 + 45 * y, widthSize, 40);
        }

        // Original Textures
        #region Original Textures
        /*
        public static Texture2D BtnTexture
        {
            get
            {
                if (btntexture == null)
                {
                    btntexture = NewTexture2D;
                    btntexture.SetPixel(0, 0, new Color32(3, 155, 229, 255));
                    btntexture.Apply();
                }
                return btntexture;
            }
        }

        public static Texture2D BtnPressTexture
        {
            get
            {
                if (btnpresstexture == null)
                {
                    btnpresstexture = NewTexture2D;
                    btnpresstexture.SetPixel(0, 0, new Color32(2, 119, 189, 255));
                    btnpresstexture.Apply();
                }
                return btnpresstexture;
            }
        }

        public static Texture2D OnPressTexture
        {
            get
            {
                if (onpresstexture == null)
                {
                    onpresstexture = NewTexture2D;
                    onpresstexture.SetPixel(0, 0, new Color32(62, 119, 64, 255));
                    onpresstexture.Apply();
                }
                return onpresstexture;
            }
        }

        public static Texture2D OnTexture
        {
            get
            {
                if (ontexture == null)
                {
                    ontexture = NewTexture2D;
                    ontexture.SetPixel(0, 0, new Color32(79, 153, 82, 255));
                    ontexture.Apply();
                }
                return ontexture;
            }
        }

        public static Texture2D OffPressTexture
        {
            get
            {
                if (offpresstexture == null)
                {
                    offpresstexture = NewTexture2D;
                    offpresstexture.SetPixel(0, 0, new Color32(79, 79, 79, 255));
                    offpresstexture.Apply();
                }
                return offpresstexture;
            }
        }

        public static Texture2D OffTexture
        {
            get
            {
                if (offtexture == null)
                {
                    offtexture = NewTexture2D;
                    offtexture.SetPixel(0, 0, new Color32(99, 99, 99, 255));
                    offtexture.Apply();
                }
                return offtexture;
            }
        }

        public static Texture2D BackTexture
        {
            get
            {
                if (backtexture == null)
                {
                    backtexture = NewTexture2D;
                    //ToHtmlStringRGBA  new Color(33, 150, 243, 1)
                    backtexture.SetPixel(0, 0, new Color32(42, 42, 42, 200));
                    backtexture.Apply();
                }
                return backtexture;
            }
        }

        public static Texture2D CornerTexture
        {
            get
            {
                if (cornertexture == null)
                {
                    cornertexture = NewTexture2D;
                    //ToHtmlStringRGBA  new Color(33, 150, 243, 1)
                    cornertexture.SetPixel(0, 0, new Color32(42, 42, 42, 0));
                    cornertexture.Apply();
                }
                return cornertexture;
            }
        }*/
        #endregion

        // try and setup our character, if we hit an error we set it to false
        //TODO: Find a way to stop it from checking whilst in main menu/lobby menu
        private static void GetCharacter()
        {
            try
            {
                LocalNetworkUser = null;
                foreach (NetworkUser readOnlyInstance in NetworkUser.readOnlyInstancesList)
                {
                    //localplayer == you!
                    if (readOnlyInstance.isLocalPlayer)
                    {
                        LocalNetworkUser = readOnlyInstance;
                        LocalPlayer = LocalNetworkUser.master;
                        LocalPlayerInv = LocalPlayer.GetComponent<Inventory>();
                        LocalHealth = LocalPlayer.GetBody().GetComponent<HealthComponent>();
                        LocalSkills = LocalPlayer.GetBody().GetComponent<SkillLocator>();
                        LocalPlayerBody = LocalPlayer.GetBody().GetComponent<CharacterBody>();
                        if (LocalHealth.alive) _CharacterCollected = true;
                        else _CharacterCollected = false;
                    }
                }
            }
            catch (Exception e)
            {
                _CharacterCollected = false;
            }
        }

        private static void RenderInteractables()
        {
            foreach (TeleporterInteraction teleporterInteraction in FindObjectsOfType<TeleporterInteraction>())
            {
                float distanceToObject = Vector3.Distance(Camera.main.transform.position, teleporterInteraction.transform.position);
                Vector3 Position = Camera.main.WorldToScreenPoint(teleporterInteraction.transform.position);
                var BoundingVector = new Vector3(Position.x, Position.y, Position.z);
                if (BoundingVector.z > 0.01)
                {
                    GUI.color =
                        teleporterInteraction.isIdle ? Color.red :
                        teleporterInteraction.isIdleToCharging || teleporterInteraction.isCharging ? Color.yellow :
                        teleporterInteraction.isCharged ? Color.green : Color.yellow;
                    int distance = (int)distanceToObject;
                    String friendlyName = "Teleporter";
                    string status = "" + (
                        teleporterInteraction.isIdle ? "Idle" :
                        teleporterInteraction.isCharging ? "Charging" :
                        teleporterInteraction.isCharged ? "Charged" :
                        teleporterInteraction.isActiveAndEnabled ? "Idle" :
                        teleporterInteraction.isIdleToCharging ? "Idle-Charging" :
                        teleporterInteraction.isInFinalSequence ? "Final-Sequence" :
                        "???");
                    string boxText = $"{friendlyName}\n{status}\n{distance}m";
                    GUI.Label(new Rect(BoundingVector.x - 50f, (float)Screen.height - BoundingVector.y, 100f, 50f), boxText);
                }
            }

            foreach (PurchaseInteraction purchaseInteraction in PurchaseInteraction.FindObjectsOfType(typeof(PurchaseInteraction)))
            {
                if (purchaseInteraction.available)
                {
                    float distanceToObject = Vector3.Distance(Camera.main.transform.position, purchaseInteraction.transform.position);
                    Vector3 Position = Camera.main.WorldToScreenPoint(purchaseInteraction.transform.position);
                    var BoundingVector = new Vector3(Position.x, Position.y, Position.z);
                    if (BoundingVector.z > 0.01)
                    {
                        int distance = (int)distanceToObject;
                        GUI.color = Color.green;
                        String friendlyName = purchaseInteraction.GetDisplayName();
                        int cost = purchaseInteraction.cost;
                        string boxText = $"{friendlyName}\n${cost}\n{distance}m";
                        GUI.Label(new Rect(BoundingVector.x - 50f, (float)Screen.height - BoundingVector.y, 100f, 50f), boxText);
                    }
                }
            }
        }

        #region Teleporter
        public static void InstaTeleporter()
        {
            if (TeleporterInteraction.instance)
            {
                TeleporterInteraction.instance.remainingChargeTimer = 0;
                Debug.Log("RoRCheats : Skipping Stage");
            }
        }

        public static void skipStage()
        {
            Run.instance.AdvanceStage(Run.instance.nextStageScene);
            Debug.Log("RoRCheats : Skipped Stage");
        }

        public static void addMountain()
        {
            TeleporterInteraction.instance.AddShrineStack();
        }

        public static void SpawnPortals(string portal)
        {
            if (TeleporterInteraction.instance)
            {
                if (portal.Equals("gold"))
                {
                    Debug.Log("RoRCheats : Spawned Gold Portal");
                    TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal = true;
                }
                else if (portal.Equals("newt"))
                {
                    Debug.Log("RoRCheats : Spawned Shop Portal");
                    TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal = true;
                }
                else if (portal.Equals("blue"))
                {
                    Debug.Log("RoRCheats : Spawned Celestal Portal");
                    TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal = true;
                }
                else if (portal.Equals("all"))
                {
                    Debug.Log("RoRCheats : Spawned All Portals");
                    TeleporterInteraction.instance.shouldAttemptToSpawnGoldshoresPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal = true;
                    TeleporterInteraction.instance.shouldAttemptToSpawnMSPortal = true;
                }
                else
                {
                    Debug.LogError("Selection was " + portal + " please contact mod developer.");
                }
            }
        }
        #endregion

        #region Player Modifiers
        private static void GiveBuff(BuffIndex buff)
        {
            LocalPlayerBody.AddBuff(buff);
        }

        private static void RemoveAllBuffs()
        {
            foreach (string buff in Enum.GetNames(typeof(BuffIndex)))
            {
                BuffIndex buffName = (BuffIndex)Enum.Parse(typeof(BuffIndex), buff);
                LocalPlayerBody.RemoveBuff(buffName);
            }
        }

        // self explanatory
        public static void giveXP()
        {
            LocalPlayer.GiveExperience(xpToGive);
        }
        public static void GiveMoney()
        {
            LocalPlayer.GiveMoney(moneyToGive);
        }
        //uh, duh.
        public static void GiveLunarCoins()
        {
            LocalNetworkUser.AwardLunarCoins(coinsToGive);
        }
        public static void LevelPlayersCrit()
        {
            try
            {
                LocalPlayerBody.levelCrit = (float)CritPerLvl;
            }
            catch (NullReferenceException)
            {
            }
        }

        public static void LevelPlayersDamage()
        {
            try
            {
                LocalPlayerBody.levelDamage = (float)damagePerLvl;
            }
            catch (NullReferenceException)
            {
            }
        }
        public static void SetplayersAttackSpeed()
        {
            try
            {
                LocalPlayerBody.baseAttackSpeed = attackSpeed;
            }
            catch (NullReferenceException)
            {
            }
        }
        public static void SetplayersArmor()
        {
            try
            {
                LocalPlayerBody.baseArmor = armor;
            }
            catch (NullReferenceException)
            {
            }
        }
        public static void SetplayersMoveSpeed()
        {
            try
            {
                LocalPlayerBody.baseMoveSpeed = movespeed;
            }
            catch (NullReferenceException)
            {
            }
        }

        public static void AimBot()
        {
            if (CursorIsVisible())
                return;
            var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            var controller = localUser.cachedMasterController;
            if (!controller)
                return;
            var body = controller.master.GetBody();
            if (!body)
                return;
            var inputBank = body.GetComponent<RoR2.InputBankTest>();
            var aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var bullseyeSearch = new RoR2.BullseyeSearch();
            var team = body.GetComponent<RoR2.TeamComponent>();
            bullseyeSearch.teamMaskFilter = RoR2.TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(team.teamIndex);
            bullseyeSearch.filterByLoS = true;
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.sortMode = RoR2.BullseyeSearch.SortMode.Distance;
            bullseyeSearch.maxDistanceFilter = float.MaxValue;
            bullseyeSearch.maxAngleFilter = 20f;// ;// float.MaxValue;
            bullseyeSearch.RefreshCandidates();
            var hurtBox = bullseyeSearch.GetResults().FirstOrDefault();
            if (hurtBox)
            {
                Vector3 direction = hurtBox.transform.position - aimRay.origin;
                inputBank.aimDirection = direction;
            }
        }

        public static void AlwaysSprint()
        {
            var localUser = RoR2.LocalUserManager.GetFirstLocalUser();
            if (localUser == null || localUser.cachedMasterController == null || localUser.cachedMasterController.master == null) return;
            var controller = localUser.cachedMasterController;
            var body = controller.master.GetBody();
            if (body && !body.isSprinting && !localUser.inputPlayer.GetButton("Sprint"))
                //controller.SetFieldValue("sprintInputPressReceived", true);
                body.isSprinting = true;
        }

        public static void Respawn()
        {
            LocalPlayer.RespawnExtraLife();
        }
        #endregion

        #region NoClip
        public static void DoNoclip()
        {
            try
            {
                var forwardDirection = LocalPlayerBody.GetComponent<InputBankTest>().moveVector.normalized;
                var aimDirection = LocalPlayerBody.GetComponent<InputBankTest>().aimDirection.normalized;
                var isForward = Vector3.Dot(forwardDirection, aimDirection) > 0f;

                var isSprinting = LocalNetworkUser.inputPlayer.GetButton("Sprint");
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                var isStrafing = LocalNetworkUser.inputPlayer.GetAxis("MoveVertical") != 0f;

                if (isSprinting)
                {
                    LocalPlayerBody.characterMotor.velocity = forwardDirection * 100f;
                    if (isStrafing)
                    {
                        if (isForward)
                        {
                            LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * 100f;
                        }
                        else
                        {
                            LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * -100f;
                        }
                    }
                }
                else
                {
                    LocalPlayerBody.characterMotor.velocity = forwardDirection * 50;
                    if (isStrafing)
                    {
                        if (isForward)
                        {
                            LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * 50;
                        }
                        else
                        {
                            LocalPlayerBody.characterMotor.velocity.y = aimDirection.y * -50;
                        }
                    }
                }
            }
            catch (NullReferenceException) { }
        }
        #endregion

        #region Item Managers
        const Int16 HandleId = 99;
        class DropItemPacket : MessageBase
        {
            public GameObject Player;
            public ItemIndex ItemIndex;
            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Player);
                writer.Write((UInt16)ItemIndex);
            }

            public override void Deserialize(NetworkReader reader)
            {
                Player = reader.ReadGameObject();
                ItemIndex = (ItemIndex)reader.ReadUInt16();
            }
        }
        class DropEquipmentPacket : MessageBase
        {
            public GameObject Player;
            public EquipmentIndex EquipmentIndex;
            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Player);
                writer.Write((UInt16)EquipmentIndex);
            }

            public override void Deserialize(NetworkReader reader)
            {
                Player = reader.ReadGameObject();
                EquipmentIndex = (EquipmentIndex)reader.ReadUInt16();
            }
        }
        static void SendDropItem(GameObject player, ItemIndex itemIndex)
        {
            NetworkServer.SendToAll(HandleId, new DropItemPacket
            {
                Player = player,
                ItemIndex = itemIndex
            });
        }
        static void SendDropEquipment(GameObject player, EquipmentIndex equipmentIndex)
        {
            NetworkServer.SendToAll(HandleId, new DropEquipmentPacket
            {
                Player = player,
                EquipmentIndex = equipmentIndex
            });
        }
        [RoR2.Networking.NetworkMessageHandler(msgType = HandleId, client = true)]
        static void HandleDropItem(NetworkMessage netMsg)
        {
            var dropItem = netMsg.ReadMessage<DropItemPacket>();
            var body = dropItem.Player.GetComponent<CharacterBody>();
            /*if (isDropItems)
                body.inventory.RemoveItem(dropItem.ItemIndex, 1);
            if (isDropItemForAll && !isDropItems)*/
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(dropItem.ItemIndex), body.transform.position + Vector3.up * 1.5f, Vector3.up * 20f + body.transform.forward * 2f);
        }
        
        public static void DropItemMethod(ItemIndex itemIndex)
        {
            var user = RoR2.LocalUserManager.GetFirstLocalUser();
            var networkClient = NetworkClient.allClients.FirstOrDefault();
            if (networkClient != null)
            {
                networkClient.RegisterHandlerSafe(HandleId, HandleDropItem);
            }
            SendDropItem(user.cachedBody.gameObject, itemIndex);
        }

        public static void DropEquipmentMethod(EquipmentIndex equipmentIndex)
        {
            var user = RoR2.LocalUserManager.GetFirstLocalUser();
            var networkClient = NetworkClient.allClients.FirstOrDefault();
            if (networkClient != null)
            {
                networkClient.RegisterHandlerSafe(HandleId, HandleDropItem);
            }
            SendDropEquipment(user.cachedBody.gameObject, equipmentIndex);
        }

        //clears inventory, duh.
        public static void ClearInventory()
        {
            if (LocalPlayerInv)
            {
                for (ItemIndex itemIndex = ItemIndex.Syringe; itemIndex < (ItemIndex)78; itemIndex++)
                {

                    //If an item exists, delete the whole stack of it
                    LocalPlayerInv.ResetItem(itemIndex); int itemCount = LocalPlayerInv.GetItemCount(itemIndex);
                    if (itemCount > 0)
                        LocalPlayerInv.RemoveItem(itemIndex, itemCount); LocalPlayerInv.ResetItem(itemIndex);

                }
                LocalPlayerInv.SetEquipmentIndex(EquipmentIndex.None);
            }
        }

        // random items
        public static void RollItems(string ammount)
        {
            try
            {
                int num;
                TextSerialization.TryParseInvariant(ammount, out num);
                if (num > 0)
                {
                    WeightedSelection<List<PickupIndex>> weightedSelection = new WeightedSelection<List<PickupIndex>>(8);
                    weightedSelection.AddChoice(Run.instance.availableTier1DropList, 80f);
                    weightedSelection.AddChoice(Run.instance.availableTier2DropList, 19f);
                    weightedSelection.AddChoice(Run.instance.availableTier3DropList, 1f);
                    for (int i = 0; i < num; i++)
                    {
                        List<PickupIndex> list = weightedSelection.Evaluate(UnityEngine.Random.value);
                        LocalPlayerInv.GiveItem(list[UnityEngine.Random.Range(0, list.Count)].itemIndex, 1);
                    }
                }
            }
            catch (ArgumentException)
            {
            }
        }

        //TODO:
        //Seems like after giving all items and removing all items, something breaks.
        //throws ArgumentException: Destination array was not long enough. Look into later.
        public static void GiveAllItems()
        {
            if (LocalPlayerInv)
            {
                for (ItemIndex itemIndex = ItemIndex.Syringe; itemIndex < (ItemIndex)78; itemIndex++)
                {
                    //plantonhit kills you when you pick it up
                    if (itemIndex == ItemIndex.PlantOnHit)
                        continue;
                    //ResetItem sets quantity to 1, RemoveItem removes the last one.
                    LocalPlayerInv.GiveItem(itemIndex, allItemsQuantity);
                }
            }
        }

        public static void StackInventory()
        {
            //Does the same thing as the shrine of order. Orders all your items into stacks of several random items.
            LocalPlayerInv.ShrineRestackInventory(Run.instance.runRNG);
        }

        public static void GiveCustomItem(ItemIndex itemIndex)
        {
            if (LocalPlayerInv)
            {
                LocalPlayerInv.GiveItem(itemIndex, 1);
            }
        }

        public static void GiveCustomEquipment(EquipmentIndex equipmentIndex)
        {
            if (LocalPlayerInv)
            {
                LocalPlayerInv.SetEquipmentIndex(equipmentIndex);
            }
        }

        public static void ReduceEquipmentCooldown()
        {
            EquipmentState equipment = LocalPlayerInv.GetEquipment((uint)LocalPlayerInv.activeEquipmentSlot);

            if (equipment.chargeFinishTime != Run.FixedTimeStamp.zero)
            {
                LocalPlayerInv.SetEquipment(new EquipmentState(equipment.equipmentIndex, Run.FixedTimeStamp.zero, equipment.charges), (uint)LocalPlayerInv.activeEquipmentSlot);
            }
        }
        #endregion

        #region Util
        public static Boolean CursorIsVisible()
        {
            foreach (var mpeventSystem in RoR2.UI.MPEventSystem.readOnlyInstancesList)
                if (mpeventSystem.isCursorVisible)
                    return true;
            return false;
        }
        public static void banPlayer(NetworkUser PlayerName, NetworkUser LocalNetworkUser)
        {
            Console.instance.RunClientCmd(LocalNetworkUser, "ban_steam", new string[] { PlayerName.Network_id.steamId.ToString() });
        }

        public static void kickPlayer(NetworkUser PlayerName, NetworkUser LocalNetworkUser)
        {
            Console.instance.RunClientCmd(LocalNetworkUser, "kick_steam", new string[] { PlayerName.Network_id.steamId.ToString() });
        }

        public static NetworkUser GetNetUserFromString(string playerString)
        {
            if (playerString != "")
            {
                if (int.TryParse(playerString, out int result))
                {
                    if (result < NetworkUser.readOnlyInstancesList.Count && result >= 0)
                    {
                        return NetworkUser.readOnlyInstancesList[result];
                    }
                    return null;
                }
                else
                {
                    foreach (NetworkUser n in NetworkUser.readOnlyInstancesList)
                    {
                        if (n.userName.Equals(playerString, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return n;
                        }
                    }
                    return null;
                }
            }
            return null;
        }

        public static void GetPlayers(string[] Players)
        {
            NetworkUser n;
            for (int i = 0; i < NetworkUser.readOnlyInstancesList.Count; i++)
            {
                n = NetworkUser.readOnlyInstancesList[i];

                Players[i] = n.userName;
            }
        }

        public static void RESETMENU()
        {
            _ifDragged = false;
            _CharacterCollected = false;
            _isStatMenuOpen = false;
            _isTeleMenuOpen = false;
            _ItemToggle = false;
            _CharacterToggle = false;
            _isLobbyMenuOpen = false;
            _isEditStatsOpen = false;
            _isItemSpawnMenuOpen = false;
            _isPlayerMod = false;
            _isEquipmentSpawnMenuOpen = false;
            _isItemManagerOpen = false;
            damageToggle = false;
            critToggle = false;
            skillToggle = false;
            renderInteractables = false;
            attackSpeedToggle = false;
            armorToggle = false;
            regenToggle = false;
            moveSpeedToggle = false;
            NoclipToggle = false;
            isDropItemForAll = false;
            alwaysSprint = false;
            aimBot = false;
        }

        //TODO: FIX
        private static readonly Dictionary<string, UnlockableDef> nameToDefTable = new Dictionary<string, UnlockableDef>();
        public static void UnlockAll()
        {
            var unlockables = nameToDefTable;
            foreach (var unlockable in unlockables)
                Run.instance.GrantUnlockToAllParticipatingPlayers(unlockable.Key);
            //foreach (var networkUser in NetworkUser.readOnlyInstancesList)
                //networkUser.AwardLunarCoins(100);
            var achievementManager = AchievementManager.GetUserAchievementManager(LocalUserManager.GetFirstLocalUser());
            foreach (var achievement in AchievementManager.allAchievementDefs)
                achievementManager.GrantAchievement(achievement);
            var profile = LocalUserManager.GetFirstLocalUser().userProfile;
            foreach (var survivor in SurvivorCatalog.allSurvivorDefs)
            {
                if (profile.statSheet.GetStatValueDouble(RoR2.Stats.PerBodyStatDef.totalTimeAlive, survivor.bodyPrefab.name) == 0.0)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.totalTimeAlive.FindStatDef(survivor.bodyPrefab.name), "0.1");
            }
            for (int i = 0; i < 150; i++)
            {
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex((ItemIndex)i));
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex((EquipmentIndex)i));
            }
        }
        #endregion
    }
}
