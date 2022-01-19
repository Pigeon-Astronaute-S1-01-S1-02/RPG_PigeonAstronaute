using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using RPG_PigeonAstronaute.Screens;
using System.Collections.Generic;

namespace RPG_PigeonAstronaute.Sprites
{
    public abstract class ModelePerso:Component
    {
        //Gestion du clavier
        public enum Touches { Up, Down, Left, Right, Open, Attack };
        public List<Keys> _touches = new List<Keys>() { Keys.Z, Keys.S, Keys.Q, Keys.D, Keys.E, Keys.V };
        public KeyboardState _kbState, _oldKbState;

        //Récupérer le jeu, son contenu, et la map + les obstacles
        protected Game1 _game;
        protected ContentManager _content;
        protected MapSpawn _mapSpawn;
        protected string[] _collisionLayers;

        //Nom et fichier spritesheet
        protected string _nomSpriteSheet { get; set; }
        protected SpriteSheet _spriteSheetMovement;

        //l'animation + le nom de chaque animation
        protected AnimatedSprite _sprite;
        public string _currentAnimation;
        protected List<string> _animationsMovement = new List<string>() {
                "IdleNorth", "IdleSouth", "IdleEast", "IdleWest",
                "WalkNorth", "WalkSouth", "WalkEast", "WalkWest", "Dead"};
        protected List<string> _animationsAttack = new List<string>() {
                "StrikeNorth", "StrikeSouth", "StrikeEast", "StrikeWest"};

        //Dimension
        public Vector2 _position;
        protected Vector2 _size;
        protected float _scale { get; set; }
        //Stats
        public uint _vitesse;
        public int _dgt;
        public int _health;
        public int _def;

        public ModelePerso(){}

        public void LoadContent()
        {
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
        public Vector2 GetTilePos(Vector2 _tilePos)
        {
            if (_kbState.IsKeyDown(_touches[(int)Touches.Left]))
                return new Vector2((ushort)(_tilePos.X - 1), (ushort)_tilePos.Y);
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Right]))
                return new Vector2((ushort)(_tilePos.X + 1), (ushort)_tilePos.Y);
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Up]))
                return new Vector2((ushort)_tilePos.X, (ushort)(_tilePos.Y - 1));
            else if (_kbState.IsKeyDown(_touches[(int)Touches.Down]))
                return new Vector2((ushort)_tilePos.X, (ushort)(_tilePos.Y + 1));
            else return new Vector2(-1, -1);
        }
        public Vector2 GetTileCoordinates(Vector2 _tilePos, TiledMap _tiledMap)
        {
            return new Vector2(_tilePos.X / _tiledMap.Width, _tilePos.Y / _tiledMap.Height);
        }

        public bool IsPresssingKey(KeyboardState kbstate, params Keys[] keys)
        {
            foreach (Keys key in keys)
                if (kbstate.IsKeyDown(key))
                    return true;
            return false;
        }
        public bool OneShot(Keys key)
        {
            return IsPresssingKey(_kbState, key) && !IsPresssingKey(_oldKbState, key);
        }
    }
}
