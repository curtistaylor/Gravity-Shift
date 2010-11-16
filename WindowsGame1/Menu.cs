﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace GravityShift
{
    public class Menu
    {
        #region Member Variables

        private SpriteFont kootenay;

        private Texture2D[] selMenuItems;
        private Texture2D[] unselMenuItems;
        private Texture2D[] menuItems;

        private bool onTitle;
        private bool onOptions;
        private bool onLoad;
        private bool onController;
        private bool onSound;

        private const int TITLE = 3;
        private const int LOAD = 4;
        private const int OPTIONS = 3;

        GamePadState pad_state;
        GamePadState prev_pad_state;
        KeyboardState key_state;
        KeyboardState prev_key_state;

        int current;

        #endregion

        #region Menu Art

        /* Title */
        private Texture2D newGameUnsel;
        private Texture2D newGameSel;
        private Texture2D loadGameUnsel;
        private Texture2D loadGameSel;
        private Texture2D optionsSel;
        private Texture2D optionsUnsel;

        /* Options Screen */
        private Texture2D controlUnsel;
        private Texture2D controlSel;
        private Texture2D soundUnsel;
        private Texture2D soundSel;

        /* Load Screen */
        private Texture2D backUnsel;
        private Texture2D backSel;
        private Texture2D oneUnsel;
        private Texture2D oneSel;
        private Texture2D twoUnsel;
        private Texture2D twoSel;
        private Texture2D threeUnsel;
        private Texture2D threeSel;

        #endregion

        /*
         * Menu Contructor
         *
         * Currently does not do anything
         */
        public Menu() {}

        /*
         * Load
         *
         * Similar to a loadContent function. This function loads and 
         * initializes the variable and art used in the class.
         *
         * ContentManager content: the Content file used in the game.
         */
        public void Load(ContentManager content)
        {
            current = 0;

            selMenuItems = new Texture2D[TITLE];
            unselMenuItems = new Texture2D[TITLE];
            menuItems = new Texture2D[TITLE];

            /* Title Screen */
            newGameSel = content.Load<Texture2D>("menu/NewGameSelected");
            newGameUnsel = content.Load<Texture2D>("menu/NewGameUnselected");
            loadGameSel = content.Load<Texture2D>("menu/LoadGameSelected");
            loadGameUnsel = content.Load<Texture2D>("menu/LoadGameUnselected");
            optionsSel = content.Load<Texture2D>("menu/OptionSelected");
            optionsUnsel = content.Load<Texture2D>("menu/OptionUnselected");
            
            /* Options Screen */
            controlUnsel = content.Load<Texture2D>("menu/ControllerUnselected");
            controlSel = content.Load<Texture2D>("menu/ControllerSelected");
            soundUnsel = content.Load<Texture2D>("menu/SoundUnselected");
            soundSel = content.Load<Texture2D>("menu/SoundSelected");
            backUnsel = content.Load<Texture2D>("menu/BackUnselected");
            backSel = content.Load<Texture2D>("menu/BackSelected");
            
            /* Level Select Screen */
            oneUnsel = content.Load<Texture2D>("menu/OneUnselected");
            oneSel = content.Load<Texture2D>("menu/OneSelected");
            twoUnsel = content.Load<Texture2D>("menu/TwoUnselected");
            twoSel = content.Load<Texture2D>("menu/TwoSelected");
            threeUnsel = content.Load<Texture2D>("menu/ThreeUnselected");
            threeSel = content.Load<Texture2D>("menu/ThreeSelected");
            
            /* Initialize the menu item arrays */
            selMenuItems[0] = newGameSel;
            selMenuItems[1] = loadGameSel;
            selMenuItems[2] = optionsSel;

            unselMenuItems[0] = newGameUnsel;
            unselMenuItems[1] = loadGameUnsel;
            unselMenuItems[2] = optionsUnsel;

            menuItems[0] = newGameSel;
            menuItems[1] = loadGameUnsel;
            menuItems[2] = optionsUnsel;

            /* Set which screen we start on */
            onTitle = true;
            onOptions = false;
            onLoad = false;
            onController = false;
            onSound = false;

            /* Pad state stuff */
            prev_pad_state = GamePad.GetState(PlayerIndex.One);

            /* Load the fonts */
            kootenay = content.Load<SpriteFont>("fonts/Kootenay");
        }

        /*
         * Update
         *
         * Updates the menu depending on what the user has selected.
         * It will handle the title, options, load and all other menu 
         * screens
         *
         * GameTime gameTime: The current game time variable
         */
        public void Update(GameTime gameTime)
        {
            /* Keyboard and GamePad states */
            key_state = Keyboard.GetState();
            pad_state = GamePad.GetState(PlayerIndex.One);

            /* If we are on the title screen */
            if (onTitle)
            {
                /* If the user hits up */
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                    key_state.IsKeyDown(Keys.Up) &&
                    prev_key_state.IsKeyUp(Keys.Up))
                {
                    /* If we are not on the first element already */
                    if (current > 0)
                    {
                        /* Decrement current and change the images */
                        current--;
                        for (int i = 0; i < TITLE; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }
                /* If the user hits the down button */
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                    key_state.IsKeyDown(Keys.Down) &&
                    prev_key_state.IsKeyUp(Keys.Down))
                {
                    /* If we are on the last element in the menu */
                    if (current < TITLE - 1)
                    {
                        /* Increment current and update graphics */
                        current++;
                        for (int i = 0; i < TITLE; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }

                /* If the user selects one of the menu items */
                if (pad_state.IsButtonDown(Buttons.A) &&
                    prev_pad_state.IsButtonUp(Buttons.A) ||
                    key_state.IsKeyDown(Keys.Enter) &&
                    prev_key_state.IsKeyUp(Keys.Enter))
                {
                    /* New Game */
                    if (current == 0)
                    {
                        /* Start the game */
                        GravityShiftMain.InMenu = false;
                        GravityShiftMain.InGame = true;
                    }
                    /* Load Game */
                    else if (current == 1)
                    {
                        /* Change to the load screen */
                        onTitle = false;
                        onLoad = true;

                        /* Initialize variables to the load menu items */
                        selMenuItems = new Texture2D[LOAD];
                        unselMenuItems = new Texture2D[LOAD];
                        menuItems = new Texture2D[LOAD];

                        selMenuItems[0] = oneSel;
                        selMenuItems[1] = twoSel;
                        selMenuItems[2] = threeSel;
                        selMenuItems[3] = backSel;

                        unselMenuItems[0] = oneUnsel;
                        unselMenuItems[1] = twoUnsel;
                        unselMenuItems[2] = threeUnsel;
                        unselMenuItems[3] = backUnsel;

                        menuItems[0] = oneSel;
                        menuItems[1] = twoUnsel;
                        menuItems[2] = threeUnsel;
                        menuItems[3] = backUnsel;

                        current = 0;
                    }
                    /* Options */
                    else if (current == 2)
                    {
                        /* Change to the options menu */
                        onTitle = false;
                        onOptions = true;

                        selMenuItems = new Texture2D[OPTIONS];
                        unselMenuItems = new Texture2D[OPTIONS];
                        menuItems = new Texture2D[OPTIONS];

                        selMenuItems[0] = controlSel;
                        selMenuItems[1] = soundSel;
                        selMenuItems[2] = backSel;

                        unselMenuItems[0] = controlUnsel;
                        unselMenuItems[1] = soundUnsel;
                        unselMenuItems[2] = backUnsel;

                        menuItems[0] = controlSel;
                        menuItems[1] = soundUnsel;
                        menuItems[2] = backUnsel;

                        current = 0;
                    }
                }
            }

            /* Options Menu*/
            else if (onOptions)
            {
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                    key_state.IsKeyDown(Keys.Up) &&
                    prev_key_state.IsKeyUp(Keys.Up))
                {
                    if (current > 0)
                    {
                        current--;
                        for (int i = 0; i < OPTIONS; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                    key_state.IsKeyDown(Keys.Down) &&
                    prev_key_state.IsKeyUp(Keys.Down))
                {
                    if (current < OPTIONS - 1)
                    {
                        current++;
                        for (int i = 0; i < OPTIONS; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }

                if (pad_state.IsButtonDown(Buttons.A) &&
                    prev_pad_state.IsButtonUp(Buttons.A) ||
                    key_state.IsKeyDown(Keys.Enter) &&
                    prev_key_state.IsKeyUp(Keys.Enter))
                {
                    /* Controller Settings */
                    if (current == 0)
                    {
                        onOptions = false;
                        onController = true;
                    }
                    /* Sound Settings */
                    else if (current == 1)
                    {
                        onOptions = false;
                        onSound = true;
                    }
                    /* Back */
                    else if (current == 2)
                    {
                        onOptions = false;
                        onTitle = true;

                        selMenuItems = new Texture2D[TITLE];
                        unselMenuItems = new Texture2D[TITLE];
                        menuItems = new Texture2D[TITLE];

                        selMenuItems[0] = newGameSel;
                        selMenuItems[1] = loadGameSel;
                        selMenuItems[2] = optionsSel;

                        unselMenuItems[0] = newGameUnsel;
                        unselMenuItems[1] = loadGameUnsel;
                        unselMenuItems[2] = optionsUnsel;

                        menuItems[0] = newGameSel;
                        menuItems[1] = loadGameUnsel;
                        menuItems[2] = optionsUnsel;

                        current = 0;
                    }
                }
            }
            /* Load Menu */
            else if (onLoad)
            {
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickUp) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickUp) ||
                    key_state.IsKeyDown(Keys.Up) &&
                    prev_key_state.IsKeyUp(Keys.Up))
                {
                    if (current > 0)
                    {
                        current--;
                        for (int i = 0; i < LOAD; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }
                if (pad_state.IsButtonDown(Buttons.LeftThumbstickDown) &&
                    prev_pad_state.IsButtonUp(Buttons.LeftThumbstickDown) ||
                    key_state.IsKeyDown(Keys.Down) &&
                    prev_key_state.IsKeyUp(Keys.Down))
                {
                    if (current < LOAD - 1)
                    {
                        current++;
                        for (int i = 0; i < LOAD; i++)
                            menuItems[i] = unselMenuItems[i];
                        menuItems[current] = selMenuItems[current];
                    }
                }

                if (pad_state.IsButtonDown(Buttons.A) &&
                    prev_pad_state.IsButtonUp(Buttons.A) ||
                    key_state.IsKeyDown(Keys.Enter) &&
                    prev_key_state.IsKeyUp(Keys.Enter))
                {
                    /* Level 1 */
                    if (current == 0)
                    {
                        /* TODO */
                    }
                    /* Level 2 */
                    else if (current == 1)
                    {
                        /* TODO */
                    }
                    /* Level 3 */
                    else if (current == 2)
                    {
                        /* TODO */
                    }
                    /* Back */
                    else if (current == 3)
                    {
                        /* Return back to the title screen */
                        onLoad = false;
                        onTitle = true;

                        selMenuItems = new Texture2D[TITLE];
                        unselMenuItems = new Texture2D[TITLE];
                        menuItems = new Texture2D[TITLE];

                        selMenuItems[0] = newGameSel;
                        selMenuItems[1] = loadGameSel;
                        selMenuItems[2] = optionsSel;

                        unselMenuItems[0] = newGameUnsel;
                        unselMenuItems[1] = loadGameUnsel;
                        unselMenuItems[2] = optionsUnsel;

                        menuItems[0] = newGameSel;
                        menuItems[1] = loadGameUnsel;
                        menuItems[2] = optionsUnsel;

                        current = 0;
                    }
                }
            }
            /* Controller Settings */
            else if (onController)
            {
                /* TODO */
            }
            /* Sound Settings */
            else if (onSound)
            {
                /* TODO */
            }

            /* Set the previous states to the current states */
            prev_pad_state = pad_state;
            prev_key_state = key_state;
        }

        /*
         * Draw
         *
         * This function will draw the current menu
         *
         * SpriteBatch spriteBatch: The current sprite batch used to draw
         * 
         * GraphicsDeviceManager graphics: The current graphics manager
         */
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();

            /* Draw the title of the game  and main background */
            spriteBatch.DrawString(kootenay, "Gravity Shift",
                                   new Vector2(150.0f, 0.0f), Color.Black);

            /* If on the title screen */
            if (onTitle)
            {
                /* Draw the title items */
                spriteBatch.Draw(menuItems[0], new Vector2(300.0f, 200.0f), Color.White);
                spriteBatch.Draw(menuItems[1], new Vector2(300.0f, 300.0f), Color.White);
                spriteBatch.Draw(menuItems[2], new Vector2(300.0f, 400.0f), Color.White);
            }

            /* If on the load screen */
            else if (onLoad)
            {
                spriteBatch.Draw(menuItems[0], new Vector2(100.0f, 100.0f), Color.White);
                spriteBatch.Draw(menuItems[1], new Vector2(100.0f, 300.0f), Color.White);
                spriteBatch.Draw(menuItems[2], new Vector2(100.0f, 500.0f), Color.White);
                spriteBatch.Draw(menuItems[3], new Vector2(700.0f, 700.0f), Color.White);          
            }

            /* If on the options menu */
            else if (onOptions)
            {
                spriteBatch.Draw(menuItems[0], new Vector2(300.0f, 200.0f), Color.White);
                spriteBatch.Draw(menuItems[1], new Vector2(300.0f, 300.0f), Color.White);
                spriteBatch.Draw(menuItems[2], new Vector2(300.0f, 400.0f), Color.White);
            }

            /* If on the controller settings screen */
            else if (onController)
            {
                /* TODO */
            }

            /* If on the sound settings screen */
            else if (onSound)
            {
                /* TODO */
            }

            spriteBatch.End();
        }
    }
}