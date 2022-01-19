using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPG_PigeonAstronaute.Screens;
using RPG_PigeonAstronaute.Sprites;
using RPG_PigeonAstronaute.Controls;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using static RPG_PigeonAstronaute.Sprites.Player;
using MonoGame.Extended.Tiled;

//https://www.youtube.com/watch?v=iunc_x7iJvM

namespace RPG_PigeonAstronaute.States
{
    public class GameState : State
    {
        public MapSpawn mapSpawn;
        public Player _player;
        public Ennemi _ennemi;

        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            _game = game;
            mapSpawn = new MapSpawn(_game);
            _game.IsMouseVisible = false;
        }

        public override void LoadContent()
        {
            mapSpawn.LoadContent();
            _player = new Player(_game, _content, mapSpawn, "PersoBase.sf", new Vector2(500, 500), new Vector2(64, 64), 0.5f, 100);
            _player.LoadContent();
            _ennemi = new Ennemi(_game, _content, mapSpawn, "DemiBossMouvement.sf", new Vector2(550, 550), new Vector2(64, 64), 0.5f, 100);
            _ennemi.LoadContent();
            var p = new PathFinding();
            var path = p.FindPath(_ennemi._position, _player._position, GetTiles(mapSpawn._map, "Mur"));
        }

        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            mapSpawn.Update(gameTime);
            mapSpawn._renduMap.Update(gameTime);
            _player.Update(gameTime);
            _ennemi.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var transformMatrix = _player._camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            mapSpawn.Draw(gameTime);
            _ennemi.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            mapSpawn._renduMap.Draw(_player._camera.GetViewMatrix());
            spriteBatch.End();
        }

        public IList<TiledMapTile> GetTiles(TiledMap map, string layerName)
        {
            TiledMapTileLayer layer = map.GetLayer<TiledMapTileLayer>(layerName);
            return new List<TiledMapTile>(layer.Tiles);
        }
    }
}
