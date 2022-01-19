using System;
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
        private Texture2D _background;
        public MenuState(Game1 game, ContentManager content)
          : base(game, content)
        {
            game.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _background = _content.Load<Texture2D>("Background");
            _components = new List<Component>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Text = "1 Player",
                    Position = new Vector2(Game1.Screen.X / 2, 600),
                    Click = new EventHandler(Button_1Player_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Highscores",
                    Position = new Vector2(Game1.Screen.X / 2, 680),
                    //Click = new EventHandler(Button_Highscores_Clicked),
                    Layer = 0.1f
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Quit",
                    Position = new Vector2(Game1.Screen.X / 2, 760),
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
            spriteBatch.Draw(_background, new Rectangle(0, 0, 1920, 1080), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
