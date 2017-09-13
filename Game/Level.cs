using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Game
{
    class Level
    {
        Ball ball;

        Texture2D wallTexture;
        Texture2D exitTexture;
        Texture2D lavaTexture;
        Texture2D goldTexture;
        SpriteFont font;

        SoundEffect goldSound;
        SoundEffect storeSound;
        SoundEffect gameSound;
        SoundEffectInstance gameSoundInstance;
        SoundEffect lavaSound;
        SoundEffectInstance lavaSoundInstance;

        Rectangle lavaRect = new Rectangle(0, 1280, 2880, 5000);
        Rectangle exitRect;

        public int ScreenWidth = 800;
        public int ScreenHeight = 800;
        public static int ScrollX;
        static int ScrollY=480;
        public int Height;
        public int Lenght;
        public static int level = 1;
        int maxLevel = 5;
        public string status;
        public int money = 120;
        public int levelMoney = 0;
        string lastname;

        List<StorePoint> Store;
        static List<Wall> walls;
        List<Wall> monets;

        public Level(ContentManager Content)
        {
            CreatedLevel();
        }

        public void ScrollApeak(int dy)
        {
            if (ScrollY + dy >= 0 && ScrollY + dy <= Height-ScreenHeight)
                ScrollY += dy;
        }

        public void ScrollHorizontally(int dx)
        {
            if (ScrollX + dx >= 0 && ScrollX + dx <= Lenght - ScreenWidth)
                ScrollX += dx;
        }

        public static Rectangle GetScreenRect(Rectangle rect)
        {
            Rectangle ScreenRect = rect;
            ScreenRect.Offset(-ScrollX, -ScrollY);
            return ScreenRect;
        }

        public static bool CrossWall(Rectangle rect)
        {
            foreach (Wall wall in walls)
            {
                if (wall.rect.Intersects(rect))
                    return true;
            }
            return false;
        }

        public void Restart()
        {
            lavaSoundInstance.Stop(true);
            gameSoundInstance.Stop(true);
            if (Game1.game == "gameMenu" || Game1.game == "lose")
                level -= 1;
            if (Game1.game == "afterLevelMenu")
                level -= 2;
            Game1.game = "game";
            NextLevel();
        }

        void Lose()
        {
            lavaSoundInstance.Stop();
            gameSoundInstance.Stop();
            Game1.game = "lose";
        }

        public void NextLevel()
        {
            status = "game";
            level++;
            exitRect = new Rectangle(0, 0, 0, 0);
            levelMoney = 0;
            CreatedLevel();
            ball.rect = new Rectangle(350, Height - 160, 50, 50);
            ball.goStart(false, true);
            if (level == 2 || level == 4)
                ball.goStart(true, true);
            ScrollX = 0;
            ScrollY = 0;
            if (level == 1)
                ScrollY = 1200;
            if (level == 3)
                ScrollY = 480;
            if (level == 5)
                ScrollY = 480;
            lavaRect = new Rectangle(0,0,0,0);
            if (level == 1 || level == 3 || level == 5)
                lavaRect = new Rectangle(0, Height, 2880, 5000);
            if (level == 2 || level == 4)
                lavaRect = new Rectangle(-1000, 0, 1000, 800);
        }

        public void NewGame(ContentManager Content)
        {
            ball.boost = false;
            ball.timeForBoost = 0;
            ball.color = "white";
            ball.texture = Content.Load<Texture2D>("Textures/Ball/ball_white");
            level = 0;
            money = 0;
            levelMoney = 0;
            NextLevel();
            status = "game";
            ball.maxJumpSpeed = 15;
        }

        public void OpenStore(ContentManager Content)
        {
            int oldlevel = level;
            status = "store";
            level = -1;
            exitRect = new Rectangle();
            CreatedLevel();
            level = oldlevel;
            lavaRect = new Rectangle(0, 0, 0, 0);
            ScrollX = 0;
            ScrollY = 0;
            Store = new List<StorePoint>();
            string[] lines = File.ReadAllLines("content/Store/store.txt");
            int n = 0;
            int x = 100;
            int y = 400;
            int numberOfPoints = lines.Length / 3;
            Height = 800;
            if (numberOfPoints * 300 + 200 > 800)
                Lenght = numberOfPoints * 300;
            else
                Lenght = 800;
            while (n <= lines.Length-3)
            {
                StorePoint point = new StorePoint(new Rectangle(x,y,100,100), lines[n+1], lines[n], Convert.ToInt32(lines[n+2]), Content.Load<Texture2D>("Store/"+lines[n+1]),Content.Load<Texture2D>("Store/Gold"), Content.Load<SpriteFont>("Store/nameFont"), Content.Load<SpriteFont>("Store/categoryFont"));
                Store.Add(point);
                x += 300;
                n += 3;
            }
        }

        public void ballOnTheBegin()
        {
            ball.rect = new Rectangle(350, Height - 200, 50, 50);
        }

        void CreatedLevel()
        {
            walls = new List<Wall>();
            monets = new List<Wall>();
            string[] lines = File.ReadAllLines("content/Levels/level" + level + ".txt");
            int x = 0;
            int y = 0;
            Height = 80 * lines.Length;
            Lenght = 80 * lines[0].Length;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    if (c == '1')
                    {
                        Rectangle rect = new Rectangle(x, y, 80, 80);
                        Wall wall = new Wall(rect, wallTexture);
                        walls.Add(wall);
                    }
                    if (c == '2')
                    {
                        exitRect = new Rectangle(x, y, 80, 80);
                    }
                    if (c == 'g')
                    {
                        Rectangle rect = new Rectangle(x+10, y+10, 60, 60);
                        Wall gold = new Wall(rect, goldTexture);
                        monets.Add(gold);
                    }
                    x += 80;
                }
                x = 0;
                y += 80;
            }
        }

        public void LoadSave()
        {
            string[] Save = File.ReadAllLines("Content/Save/save.txt");
            money = Convert.ToInt32(Save[0]);
            ball.color = Save[1];
            level = Convert.ToInt32(Save[2]);
            ball.boost = Convert.ToBoolean(Save[3]);
            ball.timeForBoost = Convert.ToInt32(Save[4]);
            ball.Speed = Convert.ToInt32(Save[5]);
            ball.maxJumpSpeed = Convert.ToInt32(Save[6]);
        }

        public void Save()
        {
            string[] Save = new string[7];
            Save[0] = money.ToString();
            Save[1] = ball.color;
            Save[2] = level.ToString();
            Save[3] = ball.boost.ToString();
            Save[4] = ball.timeForBoost.ToString();
            Save[5] = ball.Speed.ToString();
            Save[6] = ball.maxJumpSpeed.ToString();
            File.WriteAllLines(@"Content/Save/save.txt", Save);
        }

        public void BallStart()
        {
            ball.goStart(false, true);
        }

        public void LoadContent(ContentManager Content)
        {
            wallTexture = Content.Load<Texture2D>("Textures/wall");
            exitTexture = Content.Load<Texture2D>("Textures/exit");
            lavaTexture = Content.Load<Texture2D>("Textures/lava");
            goldTexture = Content.Load<Texture2D>("Textures/gold");
            goldSound = Content.Load<SoundEffect>("Sound/gold");
            storeSound = Content.Load<SoundEffect>("Sound/store");
            lavaSound = Content.Load<SoundEffect>("Sound/lava");
            gameSound = Content.Load<SoundEffect>("Sound/game");
            lavaSoundInstance = lavaSound.CreateInstance();
            gameSoundInstance = gameSound.CreateInstance();
            OpenStore(Content);
            CreatedLevel();
            font = Content.Load<SpriteFont>("Fonts/menu");
            Rectangle rect = new Rectangle(350, Height - 200, 50, 50);
            ball = new Ball(rect, Content);
        }

        public void Update(GameTime gameTime, ContentManager Content)
        {
            KeyboardState state = Keyboard.GetState();

            if (status == "game")
            {
                lavaSoundInstance.Play();
                gameSoundInstance.Volume = 0.2f;
                gameSoundInstance.Play();
            }

            if (state.IsKeyDown(Keys.Left) && ((level == 1 || level == 3 || level == 5) || status == "store"))
                ball.goStart(true, false);
            if (state.IsKeyDown(Keys.Right) && ((level == 1 || level == 3 || level == 5) || status == "store"))
                ball.goStart(true, true);
            if (state.IsKeyUp(Keys.Right) && state.IsKeyUp(Keys.Left) && ((level == 1 || level == 3 || level == 5) || status == "store"))
                ball.goStart(false, true);

            if (state.IsKeyDown(Keys.Up))
                ball.ballJump(true);
            if (state.IsKeyUp(Keys.Up))
                ball.ballJump(false);

            if (level == 1 || level == 3 || level == 5)
                lavaRect.Offset(0, -1);
            if (level == 2 || level == 4)
            {
                lavaRect.Offset(5 * gameTime.ElapsedGameTime.Milliseconds / 10, 0);
            }

            Rectangle ballScreenRect = GetScreenRect(ball.rect);

            if (ballScreenRect.Right > ScreenWidth / 2)
                ScrollHorizontally(ball.Speed * gameTime.ElapsedGameTime.Milliseconds / 10);
            if (ballScreenRect.Left < ScreenWidth / 2)
                ScrollHorizontally(-ball.Speed * gameTime.ElapsedGameTime.Milliseconds / 10);

            if (ballScreenRect.Top < ScreenHeight / 2)
                ScrollApeak((int)-Math.Abs(ball.jumpSpeed));
            if (ballScreenRect.Bottom > ScreenHeight / 2)
                ScrollApeak((int)Math.Abs(ball.jumpSpeed));

            if (lavaRect.Intersects(ball.rect))
                Lose();

            if (exitRect.Intersects(ball.rect))
            {
                lavaSoundInstance.Stop(true);
                gameSoundInstance.Stop(true);
                level++;
                if (level > maxLevel)
                { Game1.game = "win"; level -= 1; }
                else
                    Game1.game = "afterLevelMenu";
                //money += levelMoney; 
                Save();
                //money -= levelMoney;
            }

            for (int i = 0; i <= monets.Count-1; i++)
            {
                monets[i].Update(gameTime);
                if (monets[i].rect.Intersects(ball.rect))
                {
                    levelMoney += 10;
                    goldSound.Play();
                    monets.Remove(monets[i]);
                }
            }
            if (status == "store")
            {
                foreach (StorePoint point in Store)
                {
                    if (point.rect.Intersects(ball.rect) && point.price <= money && lastname != point.name && ball.color != point.name)
                    {
                        if (point.category == "Color of ball")
                        {
                            ball.color = point.name;
                            ball.texture = Content.Load<Texture2D>("Textures/Ball/ball_" + point.name);
                        }
                        if (point.category == "Upgrade")
                        {
                            if (point.name == "Boost")
                                ball.boost = true;
                            if (point.name == "Bigger jump")
                                ball.maxJumpSpeed = 18;
                        }
                        if (point.category == "Textures")
                        {
                            wallTexture = Content.Load<Texture2D>("Store/"+point.name);
                        }
                        storeSound.Play();
                        money -= point.price;
                        lastname = point.name;
                    }
                }
            }

            if (state.IsKeyDown(Keys.LeftShift) && ball.boost && ball.timeForBoost <= 3000 && ball.go)
            {
                ball.timeForBoost += gameTime.ElapsedGameTime.Milliseconds;
                ball.Speed = 10;
            }
            if (state.IsKeyUp(Keys.LeftShift))
                ball.Speed = 5;
            if (ball.timeForBoost > 3000)
            {
                ball.boost = false;
                ball.timeForBoost = 0;
                ball.Speed = 5;
            }

            if (state.IsKeyDown(Keys.Escape) && status != "store")
            {
                Game1.game = "gameMenu";
                lavaSoundInstance.Pause();
                gameSoundInstance.Pause();
            }
            if (state.IsKeyDown(Keys.Escape) && status == "store")
                Game1.game = "mainMenu";

            ball.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(exitTexture, GetScreenRect(exitRect), Color.White);
            foreach (Wall wall in walls)
            {
                wall.Draw(spriteBatch);
            }
            if (status == "game")
            {
                foreach (Wall gold in monets)
                {
                    gold.Draw(spriteBatch);
                }
            }
            if (status == "store")
            {
                foreach (StorePoint point in Store)
                {
                    point.Draw(spriteBatch);
                }
            }
            spriteBatch.Draw(lavaTexture, GetScreenRect(lavaRect), Color.White);
            if (status != "store")
                spriteBatch.DrawString(font, "Money: " + (money+levelMoney).ToString(), new Vector2(100, 100), Color.Black);
            else
                spriteBatch.DrawString(font, "Money: " + money.ToString(), new Vector2(100, 100), Color.Black);
            ball.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
