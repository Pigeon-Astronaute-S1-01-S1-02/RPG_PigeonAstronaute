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

    public class Player : ModelePerso
    {
        public bool isOpeningChest = false;

        public OrthographicCamera _camera;
        private Vector2 _cameraPosition, movementDirection = Vector2.Zero;

        public Player(Game1 game, ContentManager content, MapSpawn mapSpawn, string nomSpriteSheet, Vector2 position, Vector2 size, float scale, uint vitesse)
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

        public void LoadContent()
        {
            _spriteSheetMovement = _game.Content.Load<SpriteSheet>(_nomSpriteSheet, new JsonContentLoader());
            _sprite = new AnimatedSprite(_spriteSheetMovement);

            var _vpAdatpter = new BoxingViewportAdapter(_game.Window, _game.GraphicsDevice, 500, 400);
            _camera = new OrthographicCamera(_vpAdatpter);
        }

        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitesse;
            _oldKbState = _kbState;
            _kbState = Keyboard.GetState();
            Vector2 _tilePos = GetTilePos(_position.X, _position.Y, _mapSpawn._map);

            if (IsPresssingKey(_kbState, _touches[(int)Touches.Up], _touches[(int)Touches.Down], _touches[(int)Touches.Left], _touches[(int)Touches.Right], _touches[(int)Touches.Attack], _touches[(int)Touches.Open]))
            {
                if (_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _position.X > 0 + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animationsMovement[7];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X -= walkSpeed;
                        movementDirection -= Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _position.X + _sprite.TextureRegion.Width < Game1.Screen.X + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animationsMovement[6];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X += walkSpeed;
                        movementDirection += Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _position.Y > +_sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animationsMovement[4];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y -= walkSpeed;
                        movementDirection -= Vector2.UnitY;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _position.Y + _sprite.TextureRegion.Height < Game1.Screen.Y + _sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animationsMovement[5];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y += walkSpeed;
                        movementDirection += Vector2.UnitY;
                    }
                }
                else if (movementDirection != Vector2.Zero)
                    movementDirection.Normalize();

                if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Up]))
                    _currentAnimation = _animationsMovement[8];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Down]))
                    _currentAnimation = _animationsMovement[9];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Left]))
                    _currentAnimation = _animationsMovement[10];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Right]))
                    _currentAnimation = _animationsMovement[11];
            }
            else
            {
                if (!_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Left]))
                    _currentAnimation = _animationsMovement[3];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Right]))
                    _currentAnimation = _animationsMovement[2];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Up]))
                    _currentAnimation = _animationsMovement[0];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Down]))
                    _currentAnimation = _animationsMovement[1];
            }




            _cameraPosition += _vitesse * movementDirection * deltaSeconds;
            _camera.LookAt(_position+new Vector2(0,70));
            _sprite.Play(_currentAnimation);
            _sprite.Update(deltaSeconds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.spriteBatch.Draw(_sprite, _position, 0, new Vector2(_scale, _scale));
        }
    }
}
