using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPG_PigeonAstronaute.Screens;
using System.Linq;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;

namespace RPG_PigeonAstronaute.States
{
    public class GameState : State
    {
        private Game1 _game;
        private MapSpawn mapSpawn;

        private AnimatedSprite _perso;

        private SpriteFont _font;

        private List<Sprite> _sprites;

        public int PlayerCount;

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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            mapSpawn.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mapSpawn.Draw(gameTime);
            mapSpawn._renduMap.Draw();
            spriteBatch.End();
        }
    }
}
