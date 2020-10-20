﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UmbraMenu
{
    public class Button : IButton
    {
        public Menu parentMenu;
        public int position { get; set; }
        public Rect rect;
        public string text;
        public bool enabled = false;
        public GUIStyle style = Styles.BtnStyle;
        public Action Action { get; set; }

        public Button(Menu parentMenu, int position, string text, Action Action) 
        {
            this.parentMenu = parentMenu;
            this.position = position;
            this.text = text;
            this.Action = Action;
        }

        public Button(ListMenu parentListMenu, int position, string text, Action Action)
        {
            this.position = position;
            this.text = text;
            this.Action = Action;
        }

        public void Draw()
        {
            if (parentMenu != null)
            {
                parentMenu.NumberOfButtons = position;
                int btnY = 5 + 45 * parentMenu.NumberOfButtons;
                rect = new Rect(parentMenu.GetRect().x + 5, parentMenu.GetRect().y + btnY, parentMenu.widthSize, 40);

                if (GUI.Button(rect, text, style))
                {
                    Action?.Invoke();
                    Draw();
                }
            }
        }
    }
}
