using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using RPG_PigeonAstronaute.Controls;
using RPG_PigeonAstronaute.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_PigeonAstronaute.Screens
{
    public enum Directions { Up = 1, Down = 0, Left = 2, Right = 3 };

    public class Mouvement
    {
        private readonly PathFinding pathFinding;
        private IList<Vector2> path;
        public bool IsDone;
        private int index;

        public Mouvement()
        {
            pathFinding = new PathFinding();
        }

        public void StartPathFinding(Ennemi _ennemi, Vector2 goalTilePos, IList<TiledMapTile> collisions)
        {
            path = pathFinding.FindPath(_ennemi._posTile, goalTilePos, collisions);
            index = 0;
            IsDone = false;
        }

        public void UpdatePF(Ennemi _ennemi, Vector2 goalTilePos, IList<TiledMapTile> collisions)
        {
            if (!IsDone)
                return;
            GoToNextTile(_ennemi);
        }

        public void GoToNextTile(Ennemi _ennemi)
        {
            if (index < path.Count)
            {
                var tile = path[index];
                var dirVector = tile - _ennemi._posTile;

            }
        }

        public void Move(Directions dir, ref Vector2 posPerso, TiledMap _tiledMap, float walkSpeed, int hauteur, int largeur, Sprite _mc, List<string> animations, params string[] _layersName)
        {
            if (dir == Directions.Left)
                posPerso.X -= walkSpeed;
            else if (dir == Directions.Right)
                posPerso.X += walkSpeed;
            else if (dir == Directions.Up)
                posPerso.Y -= walkSpeed;
            else if (dir == Directions.Down)
                posPerso.Y += walkSpeed;
        }

        public void Move(ref Vector2 posPerso, ref string animation, TiledMap _tiledMap, float walkSpeed, int hauteur, int largeur, Sprite _mc, List<string> animations, params string[] _layersName)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                ushort tx = (ushort)(posPerso.X / _tiledMap.TileWidth - 0.5);
                ushort ty = (ushort)(posPerso.Y / _tiledMap.TileHeight + 1);
                animation = animations[3];
                if (!IsCollision(tx, ty, _tiledMap, _layersName) && posPerso.X > 0 + _mc.TextureRegion.Width / 2)
                    posPerso.X -= walkSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                ushort tx = (ushort)(posPerso.X / _tiledMap.TileWidth + 0.5);
                ushort ty = (ushort)(posPerso.Y / _tiledMap.TileHeight + 1);
                animation = animations[2];
                if (!IsCollision(tx, ty, _tiledMap, _layersName) && posPerso.X < _tiledMap.Width * largeur + _mc.TextureRegion.Width / 2)
                    posPerso.X += walkSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                ushort tx = (ushort)(posPerso.X / _tiledMap.TileWidth);
                ushort ty = (ushort)(posPerso.Y / _tiledMap.TileHeight + 0.5);
                animation = animations[0];
                if (!IsCollision(tx, ty, _tiledMap, _layersName) && posPerso.Y > 0 + _mc.TextureRegion.Height / 2)
                    posPerso.Y -= walkSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                ushort tx = (ushort)(posPerso.X / _tiledMap.TileWidth);
                ushort ty = (ushort)(posPerso.Y / _tiledMap.TileHeight + 1.5);
                animation = animations[1];
                if (!IsCollision(tx, ty, _tiledMap, _layersName) && posPerso.Y < _tiledMap.Height * hauteur + _mc.TextureRegion.Height / 2)
                    posPerso.Y += walkSpeed;
            }
            else
            {
                animation = animations[5];
            }
        }

        public bool IsCollision(ushort x, ushort y, TiledMap _tiledMap, params string[] _layersName)
        {
            bool col = false;

            foreach (string _layer in _layersName)
            {
                TiledMapTileLayer _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>(_layer);
                TiledMapTile? tile;
                if (_mapLayer.TryGetTile(x, y, out tile) == false)
                    col = false;
                if (!tile.Value.IsBlank)
                    return true;
            }
            return col;
        }

        public Vector2 ConvertDirectionToVector(Directions direction)
        {
            switch (direction)
            {
                case Directions.Left:
                    return new Vector2(-1, 0);
                case Directions.Up:
                    return new Vector2(0, -1);
                case Directions.Right:
                    return new Vector2(1, 0);
                case Directions.Down:
                    return new Vector2(0, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
