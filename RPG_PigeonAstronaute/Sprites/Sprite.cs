using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using RPG_PigeonAstronaute.Screens;

namespace RPG_PigeonAstronaute.Sprites
{

    public class Sprite : Component
    {
        public enum Touches { Up, Down, Left, Right, Open, Attack };
        public Game1 _game;
        protected SpriteSheet _spriteTablo;
        protected AnimatedSprite _sprite;
        protected MapSpawn _mapSpawn;
        protected Vector2 _size;
        public Vector2 _position;
        public Rectangle Rectangle;
        protected List<string>_animations = new List<string>() {
                "IdleNorth", "IdleSouth", "IdleEast", "IdleWest",
                "WalkNorth", "WalkSouth", "WalkEast", "WalkWest",
                "StrikeNorth", "StrikeSouth", "StrikeEast", "StrikeWest",
                "Dead"};
        protected float _scale { get; set; }
        public int _vitesse = 100;
        protected string[] _collisionLayers;
        protected string _nomSpriteSheet { get; set; }
        protected string _currentAnimation;
        protected KeyboardState _kbState, _oldKbState;
        public List<Keys> _touches = new List<Keys>() { Keys.Z, Keys.S, Keys.Q, Keys.D, Keys.E, Keys.NumPad0 };

        public bool isOpeningChest = false;

        public OrthographicCamera _camera;
        private Vector2 _cameraPosition, movementDirection = Vector2.Zero;

        public Sprite(Game1 game, string nomSpriteSheet, Vector2 size, Vector2 position, float scale, MapSpawn mapSpawn)
        {
            _game = game;
            _size = size;
            _position = position;
            _scale = scale;
            _nomSpriteSheet = nomSpriteSheet;
            _currentAnimation = _animations[2];
            _mapSpawn = mapSpawn;
            _collisionLayers = _mapSpawn.collisionLayers;
        }

        public void LoadContent()
        {
            _spriteTablo = _game.Content.Load<SpriteSheet>(_nomSpriteSheet, new JsonContentLoader());
            _sprite = new AnimatedSprite(_spriteTablo);
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

            if (IsPresssingKey(_kbState, _touches[(int)Touches.Up], _touches[(int)Touches.Down], _touches[(int)Touches.Left], _touches[(int)Touches.Right]))
            {
                if (_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _position.X > 0 + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animations[7];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X -= walkSpeed;
                        movementDirection -= Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _position.X + _sprite.TextureRegion.Width < Game1.Screen.X + _sprite.TextureRegion.Width / 2)
                {
                    _currentAnimation = _animations[6];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.X += walkSpeed;
                        movementDirection += Vector2.UnitX;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _position.Y > +_sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animations[4];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y -= walkSpeed;
                        movementDirection -= Vector2.UnitY;
                    }
                }
                else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _position.Y + _sprite.TextureRegion.Height < Game1.Screen.Y + _sprite.TextureRegion.Height / 2)
                {
                    _currentAnimation = _animations[5];
                    if (!IsCollision((ushort)_tilePos.X, (ushort)_tilePos.Y, _mapSpawn._map, _collisionLayers))
                    {
                        _position.Y += walkSpeed;
                        movementDirection += Vector2.UnitY;
                    }
                }
                else if (movementDirection != Vector2.Zero)
                    movementDirection.Normalize();
            }
            else
            {
                if (!_kbState.IsKeyDown(_touches[(int)Touches.Left]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Left]))
                    _currentAnimation = _animations[3];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Right]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Right]))
                    _currentAnimation = _animations[2];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Up]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Up]))
                    _currentAnimation = _animations[0];
                else if (!_kbState.IsKeyDown(_touches[(int)Touches.Down]) && _oldKbState.IsKeyDown(_touches[(int)Touches.Down]))
                    _currentAnimation = _animations[1];
            }

            _cameraPosition += _vitesse * movementDirection * deltaSeconds;
            _camera.LookAt(_position);
            _sprite.Play(_currentAnimation);
            _sprite.Update(deltaSeconds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.spriteBatch.Draw(_sprite, _position);
        }

        public bool IsCollision(ushort x, ushort y, TiledMap _tiledMap, params string[] _layerName)
        {
            bool res = false;
            if (_layerName != null && _layerName.Length > 0)
                foreach (string _layer in _layerName)
                {
                    TiledMapTileLayer _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>(_layer);
                    TiledMapTile? tile;
                    if (_mapLayer.TryGetTile(x, y, out tile) == false)
                        res = false;
                    if (!tile.Value.IsBlank)
                        return true;
                    res = false;
                }
            return res;
        }

        public Vector2 GetTilePos(float x, float y, TiledMap _tiledMap)
        {
            if (_kbState.IsKeyDown(_touches[(int)Touches.Left]))
                return new Vector2((ushort)(x / _tiledMap.TileWidth - 1), (ushort)(y / _tiledMap.TileHeight));
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]))
                return new Vector2((ushort)(x / _tiledMap.TileWidth + 1), (ushort)(y / _tiledMap.TileHeight));
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Up]))
                return new Vector2((ushort)(x / _tiledMap.TileWidth), (ushort)(y / _tiledMap.TileHeight - 1));
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]))
                return new Vector2((ushort)(x / _tiledMap.TileWidth), (ushort)(y / _tiledMap.TileHeight + 1));
            else return new Vector2(-1, -1);
        }

        public bool IsPresssingKey(KeyboardState kbstate, params Keys[] keys)
        {
            foreach (Keys key in keys)
                if (kbstate.IsKeyDown(key))
                    return true;
            return false;
        }
    }
}
