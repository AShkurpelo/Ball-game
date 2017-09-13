using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    class Wall
    {
        public Rectangle rect { get; set; }
        int currentTexture = 1;
        int timer;
        int picturesInTexture = 1;
        public Texture2D texture;

        public Wall(Rectangle Rect, Texture2D texture)
        {
            this.rect = Rect;
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            //if (texture.Width > 603)
            //    picturesInTexture = texture.Width / 483;
            //timer += gameTime.ElapsedGameTime.Milliseconds;
            //if (timer > 50)
            //{
            //    currentTexture = (currentTexture + 1) % picturesInTexture;
            //    timer = 0;
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle ScreenRect = Level.GetScreenRect(rect);
            spriteBatch.Draw(texture, ScreenRect, new Rectangle((currentTexture-1) * texture.Width / picturesInTexture, 0, texture.Width / picturesInTexture, texture.Height), Color.White);
        }
    }
}
