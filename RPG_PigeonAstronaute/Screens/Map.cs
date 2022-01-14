using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;

namespace RPG_PigeonAstronaute.Screens
{
    public abstract class Map : GameScreen
    {
        public Game1 _game1 { get; set; }
        public string _mapName { get; set; }
        public string[] _layerNames { get; set; }

        public static Vector2 WindowSize { get; set; }
        private TiledMap _map;
        public TiledMapRenderer _renduMap;
        private List<TiledMapTileLayer> _layer = new List<TiledMapTileLayer>();

        public Map(Game1 game) : base(game)
        {
            _game1 = game;
        }

        public override void Initialize()
        {
            _game1.graphics.PreferredBackBufferWidth = (int)WindowSize.X;
            _game1.graphics.PreferredBackBufferHeight = (int)WindowSize.Y;
            _game1.graphics.ApplyChanges();
        }

        public override void LoadContent()
        {
            _map = Content.Load<TiledMap>(_mapName);
            _renduMap = new TiledMapRenderer(GraphicsDevice, _map);
            foreach (string _layerName in _layerNames)
                _layer.Add(_map.GetLayer<TiledMapTileLayer>(_layerName));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            _game1.GraphicsDevice.Clear(Color.Black);
            /*_game1.spriteBatch.Begin();
            _renduMap.Draw();
            _game1.spriteBatch.End();*/
        }

        public bool IsCollision(ushort x, ushort y, TiledMap _tiledMap, params string[] _layerName)
        {
            bool res = false;
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
    }
}
