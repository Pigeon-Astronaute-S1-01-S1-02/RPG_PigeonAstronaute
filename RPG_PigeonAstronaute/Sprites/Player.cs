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
        public Keys _lastTouche;

        public OrthographicCamera _camera;
        private Vector2 _cameraPosition, movementDirection = Vector2.Zero;

        private Vector2 _posTile;

        public Rectangle _rectangleSize
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, 0 + _sprite.TextureRegion.Width, 0 + 0 + _sprite.TextureRegion.Height); }
        }

        public Player(Game1 game, ContentManager content, MapSpawn mapSpawn, string nomSpriteSheet, Vector2 position, Vector2 size, float scale, uint vitesse, int health, int dgt, int def)
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
            _health = health;
            _dgt = dgt;
            _def = def;
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

            _posTile = new Vector2(_position.X / _mapSpawn._map.TileWidth, _position.Y / _mapSpawn._map.TileHeight);
            Vector2 _tilePos = GetTilePos(_posTile);

            //Vector2 _tilePos = GetTilePos(_position.X, _position.Y, _mapSpawn._map);
            Vector2 _chestTilePos = GetTilePos(_posTile);

            if (IsPresssingKey(_kbState, _touches[(int)Touches.Up], _touches[(int)Touches.Down], _touches[(int)Touches.Left], _touches[(int)Touches.Right], _touches[(int)Touches.Attack], _touches[(int)Touches.Open]))
            {
                if (_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _position.X > 0 + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animationsMovement[7];
                    _lastTouche = _touches[(int)Touches.Left];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X -= walkSpeed;
                        movementDirection -= Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _position.X + _sprite.TextureRegion.Width < Game1.Screen.X + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animationsMovement[6];
                    _lastTouche = _touches[(int)Touches.Right];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X += walkSpeed;
                        movementDirection += Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _position.Y > +_sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animationsMovement[4];
                    _lastTouche = _touches[(int)Touches.Up];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y -= walkSpeed;
                        movementDirection -= Vector2.UnitY;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _position.Y + _sprite.TextureRegion.Height < Game1.Screen.Y + _sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animationsMovement[5];
                    _lastTouche = _touches[(int)Touches.Down];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y += walkSpeed;
                        movementDirection += Vector2.UnitY;
                    }
                }
                else if (movementDirection != Vector2.Zero)
                    movementDirection.Normalize();

                if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _lastTouche == _touches[(int)Touches.Up])
                    _currentAnimation = _animationsAttack[0];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _lastTouche == _touches[(int)Touches.Down])
                    _currentAnimation = _animationsAttack[1];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _lastTouche == _touches[(int)Touches.Right])
                    _currentAnimation = _animationsAttack[2];
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Attack]) && _lastTouche == _touches[(int)Touches.Left])
                    _currentAnimation = _animationsAttack[3];

/*                if (OneShot(_touches[(int)Touches.Open]))
                {
                    if (_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _position.Y >= _sprite.TextureRegion.Height && IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, "Porte"))
                        _position.Y -= _sprite.TextureRegion.Height;
                    else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _position.Y <= _mapSpawn._map.HeightInPixels - _sprite.TextureRegion.Height / 2 && IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, "Porte"))
                        _position.Y += _sprite.TextureRegion.Height / 2;
                    else if (_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _position.X >= _sprite.TextureRegion.Width && IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, "Porte"))
                        _position.X -= _sprite.TextureRegion.Width;
                    else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _position.Y <= _mapSpawn._map.WidthInPixels - _sprite.TextureRegion.Width / 2 && IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, "Porte"))
                        _position.X += _sprite.TextureRegion.Width / 2;
                    else if (IsCollision((ushort)_chestTilePos.X, (ushort)_chestTilePos.Y, _mapSpawn._map, "Coffre"))
                        Console.WriteLine("Chest Opened");
                    else Console.WriteLine("Opened");
                }*/
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
