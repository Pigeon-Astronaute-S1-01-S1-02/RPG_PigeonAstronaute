using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using RPG_PigeonAstronaute.States;

namespace RPG_PigeonAstronaute.Sprites
{
    public class Player : Game
    {
        private Vector2 _persoPosition;
        private Vector2 _winSize;
        private AnimatedSprite _perso;
        private string _spriteSheetName;
        private int _vitessePerso;

        private int _health;
        private int _def;
        private int _dmg;

        private Game1 _game;
        public SpriteBatch _spriteBatch;

        public Player(string spriteSheetName, Vector2 persoPos, int vitesse, int health, int def, int dmg)
        {
            _persoPosition = persoPos;
            _vitessePerso = vitesse;
            _health = health;
            _def = def;
            _dmg = dmg;
            _spriteSheetName = spriteSheetName;

            _persoPosition = new Vector2(_persoPosition.X, _persoPosition.Y);
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // spritesheet
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>(_spriteSheetName, new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
        }

        public override void Update(GameTime gameTime)
        {

            _winSize = new Vector2(_game.graphics.PreferredBackBufferWidth, _game.graphics.PreferredBackBufferHeight);
            // TODO: Add your update logic here

            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;

            string animation = "IdleSouth";

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                animation = "WalkWest";
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                animation = "WalkEast";
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && _persoPosition.Y > +_perso.TextureRegion.Height / 2)
            {
                animation = "WalkNorth";
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && _persoPosition.Y + _perso.TextureRegion.Height < _winSize.Y + _perso.TextureRegion.Height / 2)
            {
                animation = "WalkSouth";
            }
            else
                animation = "IdleSouth";

            _perso.Play(animation);
            _perso.Update(deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_perso, _persoPosition);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
