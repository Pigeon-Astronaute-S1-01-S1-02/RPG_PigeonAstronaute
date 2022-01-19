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
        public string[] collisionLayers { get; set; }

        public static Vector2 WindowSize { get; set; }
        public TiledMap _map;
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
            foreach (string _layerName in collisionLayers)
                _layer.Add(_map.GetLayer<TiledMapTileLayer>(_layerName));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            _game1.GraphicsDevice.Clear(Color.Black);
        }

        
    }
}
