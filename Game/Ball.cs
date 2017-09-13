using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Game
{
    class Ball
    {
        public Rectangle rect;
        public Texture2D texture;
        public bool go;
        bool goRight;
        public int Speed = 5;
        public bool boost;
        public int timeForBoost;
        bool jump;
        public double jumpSpeed;
        public double maxJumpSpeed = 15;
        double g = 0.4;
        int ballSize;
        int cornerTime;
        public string color = "White";
        float corner = 0.0f;
        float cornerSpeed = 0.3f;

        public Ball(Rectangle rect, ContentManager Content)
        {
            this.rect = rect;
            string[] Save = File.ReadAllLines("Content/Save/save.txt");
            color = Save[1];
            boost = Convert.ToBoolean(Save[3]);
            timeForBoost = Convert.ToInt32(Save[4]);
            Speed = Convert.ToInt32(Save[5]);
            maxJumpSpeed = Convert.ToInt32(Save[6]);
            texture = Content.Load<Texture2D>("Textures/Ball/ball_" + color);
            ballSize = texture.Height;
        }

        public void goStart(bool goBall, bool right)
        {
            if (goBall)
            {
                go = true;
                goRight = right;
            }
            else
                go = false;
        }

        void Gravitation(GameTime gameTime)
        {
            jumpSpeed = jumpSpeed - g * gameTime.ElapsedGameTime.Milliseconds / 10;
            double dy = jumpSpeed * gameTime.ElapsedGameTime.Milliseconds / 10;
            Rectangle nextRect = rect;
            nextRect.Offset(0, -(int)dy);            
            bool stopFalling = Level.CrossWall(nextRect) && dy < 0;
            if (Level.GetScreenRect(nextRect).Top > 0 && Level.GetScreenRect(nextRect).Bottom < 800 && !Level.CrossWall(nextRect))
            {
                rect = nextRect;
            }
            if (Level.GetScreenRect(nextRect).Bottom > 800 || stopFalling)
            {
                jump = false;
                jumpSpeed = 0;
            }
        }

        public void ballJump(bool boolJump)
        {
            if (boolJump && jumpSpeed == 0)
            {
                jumpSpeed = maxJumpSpeed;
                jump = boolJump;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (go)
            {
                cornerTime += gameTime.ElapsedGameTime.Milliseconds;
                if (cornerTime > 1)
                {
                    if (goRight)
                        corner = (corner + cornerSpeed) % 6.28f;
                    if (!goRight)
                    {
                        corner = corner - cornerSpeed;
                    }
                    if (corner <= 0)
                        corner = 6.28f;
                    cornerTime = 0;
                }
                int dx = Speed * gameTime.ElapsedGameTime.Milliseconds / 10;
                if (!goRight)
                    dx = -dx;
                Rectangle nextRect = rect;
                nextRect.Offset(dx, 0);
                if (Level.GetScreenRect(nextRect).Left > 0 && Level.GetScreenRect(nextRect).Right < 800 && !Level.CrossWall(nextRect))
                    rect = nextRect;
            }
            Gravitation(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle ScreenRect = Level.GetScreenRect(rect);
            ScreenRect.Offset(rect.Width/2, rect.Width/2);
            spriteBatch.Draw(texture, ScreenRect, new Rectangle(0, 0, ballSize, ballSize), Color.White, corner, new Vector2(ballSize/2,ballSize/2), SpriteEffects.None, 0);
            
        }

    }
}
