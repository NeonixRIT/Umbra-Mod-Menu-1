using UnityEngine;

namespace UmbraMenu
{
    public static class Styles
    {
        private static GUIStyle defaultStyle, chestStyle, newtStyle, equipmentStyle, lunarStyle, voidStyle, chanceStyle, bloodStyle, combatStyle, mountainStyle, woodsStyle, teleporterStyle;
        private static GUIStyle renderMobsStyle, renderSecretsStyle, selectedChestStyle;

        private static GUIStyle CreateGUIStyle(Texture2D normalBackground, Texture2D activeBackground, Color color, int fontSize, FontStyle font, TextAnchor textAlignment, bool wrapWords = false)
        {
            var guiStyle = new GUIStyle
            {
                normal =
                {
                    background = normalBackground,
                    textColor = color
                },
                onNormal =
                {
                    background = normalBackground,
                    textColor = color
                },
                active =
                {
                    background = activeBackground,
                    textColor = color
                },
                onActive =
                {
                    background = activeBackground,
                    textColor = color
                },
                fontSize = fontSize,
                fontStyle = font,
                alignment = textAlignment,
                wordWrap = wrapWords
            };
            return guiStyle;
        }

        #region Styles
        public static GUIStyle RenderMobsStyle =>
            renderMobsStyle ??= CreateGUIStyle(null, null, Color.red, 14, FontStyle.Normal, TextAnchor.MiddleLeft);


        public static GUIStyle SelectedChestStyle =>
            selectedChestStyle ??= CreateGUIStyle(null, null,
                Color.HSVToRGB(0.5256f, 0.9286f, 0.9333f), 14, FontStyle.Normal, TextAnchor.MiddleRight);


        public static GUIStyle RenderSecretsStyle =>
            renderSecretsStyle ??= CreateGUIStyle(null, null,
                Color.HSVToRGB(0.5065f, 1.0000f, 1.0000f), 14, FontStyle.Normal, TextAnchor.MiddleLeft);

        public static GUIStyle DefaultStyle =>
            defaultStyle ??= CreateGUIStyle(null, null, new Color32(180, 200, 220, 255), 12,
                FontStyle.Normal, TextAnchor.MiddleCenter);

        public static GUIStyle ChestStyle =>
            chestStyle ??= CreateGUIStyle(null, null, new Color32(40, 110, 255, 255), 12,
                FontStyle.Normal, TextAnchor.MiddleCenter);

        public static GUIStyle NewtStyle =>
            newtStyle ??= CreateGUIStyle(null, null, new Color32(70, 130, 220, 255), 12,
                FontStyle.BoldAndItalic, TextAnchor.MiddleCenter);

        public static GUIStyle EquipmentStyle =>
            equipmentStyle ??= CreateGUIStyle(null, null, new Color32(200, 80, 0, 255), 12,
                FontStyle.Normal, TextAnchor.MiddleCenter);

        public static GUIStyle LunarStyle =>
            lunarStyle ??= CreateGUIStyle(null, null, new Color32(120, 175, 225, 255), 12,
                FontStyle.Italic, TextAnchor.MiddleCenter);

        public static GUIStyle VoidStyle =>
            voidStyle ??= CreateGUIStyle(null, null, new Color32(250, 80, 160, 255), 12,
                FontStyle.Italic, TextAnchor.MiddleCenter);

        public static GUIStyle ChanceStyle =>
            chanceStyle ??= CreateGUIStyle(null, null, new Color32(255, 255, 90, 255), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter);

        public static GUIStyle BloodStyle =>
            bloodStyle ??= CreateGUIStyle(null, null, new Color32(230, 110, 100, 255), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter);

        public static GUIStyle CombatStyle =>
            combatStyle ??= combatStyle = CreateGUIStyle(null, null, new Color32(230, 165, 240, 255), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter);

        public static GUIStyle MountainStyle =>
            mountainStyle ??= CreateGUIStyle(null, null, new Color32(125, 230, 255, 255), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter);

        public static GUIStyle WoodsStyle =>
            woodsStyle ??= CreateGUIStyle(null, null, new Color32(170, 225, 100, 255), 12,
                FontStyle.Bold, TextAnchor.MiddleCenter);

        public static GUIStyle TeleporterStyle =>
            teleporterStyle ??= CreateGUIStyle(null, null, new Color32(125, 40, 70, 255),
                12, FontStyle.Bold, TextAnchor.MiddleCenter);

        #endregion
    }
}
