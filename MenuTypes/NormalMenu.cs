﻿using RewiredConsts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UmbraMenu
{
    public class NormalMenu : IMenu
    {
        public float delay = 0, widthSize = 350;
        public int id { get; set; }
        public string title { get; set; }
        public bool enabled { get; set; }
        public Rect rect { get; set; }
        public bool ifDragged { get; set; }
        public int numberOfButtons { get; set; }
        public TogglableButton activatingButton { get; set; }
        public bool highlighted = false;
        public List<IButton> buttons { get; set; }

        public NormalMenu(int id, Rect rect, string title, List<IButton> buttons)
        {
            this.id = id;
            this.rect = rect;
            this.title = title;
            this.buttons = buttons;
        }

        public void SetWindow()
        {
            rect = GUI.Window(id, rect, new GUI.WindowFunction(SetBackground), "", new GUIStyle());
        }

        public void Draw()
        {
            if (enabled)
            {
                GUI.Box(new Rect(rect.x + 0f, rect.y + 0f, widthSize + 10, 50f + 45 * numberOfButtons), "", Styles.MainBgStyle);
                GUI.Label(new Rect(rect.x + 5f, rect.y + 5f, widthSize + 5, 85f), title, Styles.TitleStyle);
                DrawAllButtons();
            }
        }

        private void DrawAllButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw();
            }
        }

        private void SetBackground(int windowID)
        {
            GUI.Box(new Rect(0f, 0f, widthSize + 10, 50f + 45 * numberOfButtons), "", Styles.CornerStyle);
            if (Event.current.type == EventType.MouseDrag)
            {
                delay += Time.deltaTime;
                if (delay > 0.3f)
                {
                    ifDragged = true;
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                delay = 0;
                if (!ifDragged)
                {
                    enabled = !enabled;
                    if (activatingButton != null)
                    {
                        activatingButton.Enabled = !activatingButton.Enabled;
                    }
                    UmbraMenu.GetCharacter();
                }
                ifDragged = false;
            }
            GUI.DragWindow();
        }
    }
}
