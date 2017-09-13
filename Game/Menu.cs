using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game
{
    class Menu
    {
        public List<MenuPoint> Items { get; set; }
        SpriteFont font;
        int currentPoint = 0;
        public KeyboardState oldstate;
        public KeyboardState state;
        SoundEffect menuItemsSwitchSound;
        SoundEffect menuItemSelectSound;

        public Menu()
        {
            Items = new List<MenuPoint>();
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/menu");
            menuItemsSwitchSound = Content.Load<SoundEffect>("Sound/menuSwitch");
            menuItemSelectSound = Content.Load<SoundEffect>("Sound/menuSelect");
        }

        public void Update()
        {
            state = Keyboard.GetState();
            int delta = 0;

            if (oldstate.IsKeyDown(Keys.Enter) && state.IsKeyUp(Keys.Enter))
            {
                menuItemSelectSound.Play();
                Items[currentPoint].PressButton();
            }

            if (state.IsKeyDown(Keys.Up) && oldstate.IsKeyUp(Keys.Up))
            { delta = -1; menuItemsSwitchSound.Play(); }
            if (state.IsKeyDown(Keys.Down) && oldstate.IsKeyUp(Keys.Down))
            { delta = 1; menuItemsSwitchSound.Play(); }
            currentPoint += delta;

            bool ok = false;
            while (!ok)
            {
                if (currentPoint < 0)
                    currentPoint = Items.Count - 1;
                else if (currentPoint > Items.Count - 1)
                    currentPoint = 0;
                else if (!Items[currentPoint].On && delta != 0)
                    currentPoint += delta;
                else ok = true;
            }

            oldstate = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int y = 200 + (200 / Items.Count);
            foreach (MenuPoint point in Items)
            {
                Color color = Color.Black;
                if (!point.On)
                    color = Color.Gray;
                if (point == Items[currentPoint])
                    color = Color.Red;
                spriteBatch.DrawString(font, point.Text, new Vector2(200,y), color);
                y += 100;
            }
            spriteBatch.End();
        }
    }
}
