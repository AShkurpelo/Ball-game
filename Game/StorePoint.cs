using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game
{
    class StorePoint
    {
        public Rectangle rect;
        Texture2D texture;
        Texture2D goldTexture;
        public string name;
        public string category;
        public int price;
        SpriteFont nameFont;
        SpriteFont categoryFont;

        public StorePoint(Rectangle rect, string name, string category, int price, Texture2D texture,Texture2D goldTexture, SpriteFont nameFont, SpriteFont categoryFont)
        {
            this.rect = rect;
            this.name = name;
            this.category = category;
            this.price = price;
            this.texture = texture;
            this.goldTexture = goldTexture;
            this.nameFont = nameFont;
            this.categoryFont = categoryFont;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Level.GetScreenRect(rect), Color.White);
            spriteBatch.Draw(goldTexture, Level.GetScreenRect(new Rectangle(rect.Left + 25, rect.Top - 50, 20, 20)), Color.White);
            spriteBatch.DrawString(nameFont, price.ToString(), new Vector2(rect.Left + 50 - Level.ScrollX, rect.Top - 53), Color.White);
            spriteBatch.DrawString(nameFont, name, new Vector2(rect.Left +(rect.Width - name.Length*11)/2 - Level.ScrollX, rect.Top - 100), Color.White);
            spriteBatch.DrawString(categoryFont, category, new Vector2(rect.Left+(rect.Width - category.Length*13.6f)/2 - Level.ScrollX, rect.Top - 150), Color.White);
        }
    }
}
