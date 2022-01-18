using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;
using RPG_PigeonAstronaute.Screens;

namespace RPG_PigeonAstronaute.Sprites
{
    public class Ennemie:ModelePerso
    {
        public Ennemie(Game1 game, ContentManager content, MapSpawn mapSpawn, string nomSpriteSheet, Vector2 position, Vector2 size, float scale, uint vitesse)
        {
            _game = game;
            _content = content;
            _mapSpawn = mapSpawn;
            _nomSpriteSheet = nomSpriteSheet;
            _position = position;
            _size = size;
            _scale = scale;
            _vitesse = vitesse;
            _collisionLayers = _mapSpawn.collisionLayers;
            _currentAnimation = _animationsMovement[2];
        }

        public new void LoadContent()
        {
            _spriteSheetMovement = _game.Content.Load<SpriteSheet>(_nomSpriteSheet, new JsonContentLoader());
            _sprite = new AnimatedSprite(_spriteSheetMovement);

        }

        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitesse;
            _oldKbState = _kbState;
            _kbState = Keyboard.GetState();

            Vector2 _tilePos = GetTilePos(_position.X, _position.Y, _mapSpawn._map);
            _sprite.Play(_currentAnimation);
            _sprite.Update(deltaSeconds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.spriteBatch.Draw(_sprite, _position, 0, new Vector2(_scale, _scale));
        }

    }
}
