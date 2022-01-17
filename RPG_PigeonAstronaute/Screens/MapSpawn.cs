using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using RPG_PigeonAstronaute.Sprites;
using System.Collections.Generic;

namespace RPG_PigeonAstronaute.Screens
{
    public class MapSpawn : Map
    {
        private readonly Game1 _game;
        private SpriteBatch _spriteBatch;
        private Vector2 _mcPosition;
        private AnimatedSprite _mc;
        private string animation;
        public int vitesse;
        private Mouvement mouvement;
        public TiledMap map;
        public TiledMapRenderer renduMap;
        private TiledMapTileLayer _tpPoints;
        private OrthographicCamera _camera;
        private List<string> animations;

        public MapSpawn(Game1 game) : base(game)
        {
            _game = game;
            _mapName = "MapEtage";
            collisionLayers = new string[] { "Mur", "Porte" };
            _renduMap = renduMap;
            animations = new List<string>() {
                "IdleNorth", "IdleSouth", "IdleEast", "IdleWest",
                "WalkNorth", "WalkSouth", "WalkEast", "WalkWest",
                "StrikeNorth", "StrikeSouth", "StrikeEast", "StrikeWest",
                "Dead"};
        }

        public override void Initialize()
        {
            _mcPosition = new Vector2(300, 300);
            vitesse = 100;
            animation = animations[5];
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            var viewportadapter = new BoxingViewportAdapter(_game.Window, GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportadapter);
            mouvement = new Mouvement();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("PersoBase.sf", new JsonContentLoader());
            map = Content.Load<TiledMap>(_mapName);
            _renduMap = new TiledMapRenderer(GraphicsDevice, map);
            _mc = new AnimatedSprite(spriteSheet);
        }

        public override void Update(GameTime gameTime)
        {
            _renduMap.Update(gameTime);
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * vitesse;
            mouvement.Move(ref _mcPosition, ref animation, map, walkSpeed, 600, 800, _mc, animations, "Mur", "Porte");
            _camera.LookAt(_mcPosition);
            _mc.Play(animation);
            _mc.Update(deltaSeconds);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _renduMap.Draw(_camera.GetViewMatrix());
            _spriteBatch.Begin();
            _spriteBatch.Draw(_mc, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), 0, new Vector2((float)1.5, (float)1.5));
            _spriteBatch.End();
            _renduMap.Draw(11, _camera.GetViewMatrix());
            _renduMap.Draw(15, _camera.GetViewMatrix());
        }
    }
}
