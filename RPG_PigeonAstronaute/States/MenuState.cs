﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RPG_PigeonAstronaute.Controls;

namespace RPG_PigeonAstronaute.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        public MenuState(Game1 game, ContentManager content)
          : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                /*new Sprite(_content.Load<Texture2D>("Background/MainMenu"))
                {
                    Layer = 0f,
                    Position = new Vector2(Game1._game.Screen.X / 2, Game1.ScreenHeight / 2),
                },*/
                new Button(buttonTexture, buttonFont)
                {
                    Text = "1 Player",
                    Position = new Vector2(Game1.Screen.X / 2, 400),
                    Click = new EventHandler(Button_1Player_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Highscores",
                    Position = new Vector2(Game1.Screen.X / 2, 480),
                    //Click = new EventHandler(Button_Highscores_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Quit",
                    Position = new Vector2(Game1.Screen.X / 2, 520),
                    Click = new EventHandler(Button_Quit_Clicked),
                    Layer = 0.1f
                },
            };
        }

        private void Button_1Player_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new GameState(_game, _content));
        }


        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
