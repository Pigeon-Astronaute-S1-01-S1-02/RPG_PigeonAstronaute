using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPG_PigeonAstronaute.Screens;
using RPG_PigeonAstronaute.Sprites;
using RPG_PigeonAstronaute.Controls;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using static RPG_PigeonAstronaute.Sprites.Perso;

namespace RPG_PigeonAstronaute.States
{
    public class GameState : State
    {
        public MapSpawn mapSpawn;
        private Vector2 _spawnPos = new Vector2(750, 750);

        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            _game = game;
            mapSpawn = new MapSpawn(_game);
        }

        public override void LoadContent()
        {
            mapSpawn.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            mapSpawn.Update(gameTime);
            mapSpawn._renduMap.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mapSpawn.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
