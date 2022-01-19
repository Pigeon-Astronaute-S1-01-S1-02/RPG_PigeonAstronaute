using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG_PigeonAstronaute.Controls
{
    public class Button : Component
    {
        #region Fields
        private MouseState _currentMS, _previousMS;
        private SpriteFont _font;
        private bool _isHovering;
        private Texture2D _texture;
        #endregion

        #region Properties
        public EventHandler Click;
        public bool Clicked { get; set; }
        public float Layer { get; set; }
        public Vector2 Origin { get { return new Vector2(_texture.Width / 2, _texture.Height / 2); } }
        public Color PenColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height); } }
        public string Text;
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColor = Color.Black;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_isHovering)
                color = Color.Gray;

            spriteBatch.Draw(_texture, Position, null, color, 0f, Origin, 2f, SpriteEffects.None, Layer);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X - ((_font.MeasureString(Text).X*2)/2));
                var y = (Rectangle.Y + (Rectangle.Height/2)) - ((_font.MeasureString(Text).Y*2)/ 2);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, Layer + 0.01f);
            }
        }
        public override void Update(GameTime gameTime)
        {
            _previousMS = _currentMS;
            _currentMS = Mouse.GetState();
            var mouseRectangle = new Rectangle(_currentMS.X, _currentMS.Y, 1, 1);
            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                if (_currentMS.LeftButton == ButtonState.Released && _previousMS.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            }
        }
        #endregion
    }
}
