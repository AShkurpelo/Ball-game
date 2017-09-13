using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        SoundEffect loseSound;
        SoundEffect winSound;
        SoundEffectInstance loseSoundInstance;
        SoundEffectInstance winSoundInstance;

        public static string game = "mainMenu";

        Level gameLevel;
        Menu mainMenu;
        Menu pauseMenu;
        Menu sureNewGameMenu;
        Menu afterLevelMenu;
        Menu loseMenu;
        Menu winMenu;
     
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Ball Game";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            mainMenu = new Menu();
            pauseMenu = new Menu();
            sureNewGameMenu = new Menu();
            afterLevelMenu = new Menu();
            loseMenu = new Menu();
            winMenu = new Menu();
            
            MenuPoint newGame = new MenuPoint("New Game");
            MenuPoint resumeGame = new MenuPoint("Resume");
            MenuPoint store = new MenuPoint("Store");
            MenuPoint back = new MenuPoint("Back");
            MenuPoint exitGame = new MenuPoint("Exit");
            MenuPoint exitLevel = new MenuPoint("Go to main menu");
            MenuPoint nextLevel = new MenuPoint("Next level");
            MenuPoint yes = new MenuPoint("Yes");
            MenuPoint Continue = new MenuPoint("Continue");
            MenuPoint restartLevel = new MenuPoint("Restart level");


            string[] check = File.ReadAllLines("Content/Save/save.txt");
            if (check[2] == "1")
                Continue.On = false;

            newGame.Press += new EventHandler(newGame_Press);
            resumeGame.Press += new EventHandler(resumeGame_Press);
            store.Press += new EventHandler(store_Press);
            back.Press += new EventHandler(back_Press);
            exitGame.Press += new EventHandler(exitGame_Press);
            exitLevel.Press += new EventHandler(exitLevel_Press);
            nextLevel.Press += new EventHandler(nextLevel_Press);
            yes.Press += new EventHandler(yes_Press);
            Continue.Press += new EventHandler(Continue_Press);
            restartLevel.Press += new EventHandler(restartLevel_Press);

            winMenu.Items.Add(exitLevel);

            loseMenu.Items.Add(restartLevel);
            loseMenu.Items.Add(exitLevel);

            afterLevelMenu.Items.Add(nextLevel);
            afterLevelMenu.Items.Add(restartLevel);
            afterLevelMenu.Items.Add(exitLevel);

            mainMenu.Items.Add(newGame);
            mainMenu.Items.Add(Continue);
            mainMenu.Items.Add(store);
            mainMenu.Items.Add(exitGame);

            pauseMenu.Items.Add(restartLevel);
            pauseMenu.Items.Add(resumeGame);
            pauseMenu.Items.Add(exitLevel);

            sureNewGameMenu.Items.Add(yes);
            sureNewGameMenu.Items.Add(back);

            base.Initialize();
        }
        void nextLevel_Press(object sender, EventArgs e)
        {
            gameLevel.money += gameLevel.levelMoney;
            gameLevel.levelMoney = 0;
            Level.level -= 1;
            gameLevel.NextLevel();
            game = "game";
        }

        void exitLevel_Press(object sender, EventArgs e)
        {
            winSoundInstance.Stop();
            loseSoundInstance.Stop();
            if (game == "afterLevelMenu")
                gameLevel.money += gameLevel.levelMoney;
            gameLevel.levelMoney = 0;
            gameLevel.BallStart();
            game = "mainMenu";
        }

        void Continue_Press(object sennder, EventArgs e)
        {
            Level.level = Level.level - 1;
            gameLevel.NextLevel();
            game = "game";
        }

        void restartLevel_Press(object sender, EventArgs e)
        {
            loseSoundInstance.Stop();
            gameLevel.Restart();
        }

        void yes_Press(object sender, EventArgs e)
        {
            mainMenu.Items[1].On = true;
            game = "game";
            gameLevel.NewGame(Content);
        }

        void back_Press(object sender, EventArgs e)
        {
            game = "mainMenu";
        }

        void store_Press(object sender, EventArgs e)
        {
            game = "game";
            gameLevel.OpenStore(Content);
            gameLevel.ballOnTheBegin();
        }

        void exitGame_Press(object sender, EventArgs e)
        {
            gameLevel.Save();
            this.Exit();
        }

        void newGame_Press(object sender, EventArgs e)
        {
            if (!mainMenu.Items[1].On)
            {
                mainMenu.Items[1].On = true;
                game = "game";
                gameLevel.NewGame(Content);
            }
            if (mainMenu.Items[1].On)
                game = "sureNewGameMenu";
        }

        void resumeGame_Press(object sender, EventArgs e)
        {
            game = "game";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pauseMenu.LoadContent(Content);
            mainMenu.LoadContent(Content);
            sureNewGameMenu.LoadContent(Content);
            afterLevelMenu.LoadContent(Content);
            loseMenu.LoadContent(Content);
            winMenu.LoadContent(Content);

            gameLevel = new Level(Content);
            gameLevel.LoadContent(Content);
            gameLevel.LoadSave();

            loseSound = Content.Load<SoundEffect>("Sound/lose");
            loseSoundInstance = loseSound.CreateInstance();
            winSound = Content.Load<SoundEffect>("Sound/win");
            winSoundInstance = winSound.CreateInstance();

            font = Content.Load<SpriteFont>("Fonts/messegeFont");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            if (game == "game")
                gameLevel.Update(gameTime, Content);
            if (game == "mainMenu")
                mainMenu.Update();
            if (game == "gameMenu")
                pauseMenu.Update();
            if (game == "sureNewGameMenu")
                sureNewGameMenu.Update();
            if (game == "afterLevelMenu")
                afterLevelMenu.Update();
            if (game == "lose")
            {
                loseMenu.Update();
                loseSoundInstance.Play();
            }
            if (game == "win")
            {
                winMenu.Update();
                winSoundInstance.Play();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (game == "game")
                gameLevel.Draw(spriteBatch);
            if (game == "gameMenu")
                pauseMenu.Draw(spriteBatch);
            if (game == "mainMenu")
                mainMenu.Draw(spriteBatch);
            if (game == "sureNewGameMenu")
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Are you sure?", new Vector2(230, 150), Color.White);
                spriteBatch.End();
                sureNewGameMenu.Draw(spriteBatch);
            }
            if (game == "afterLevelMenu")
                afterLevelMenu.Draw(spriteBatch);
            if (game == "lose")
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "You LOOOOOOOOSE!!!", new Vector2(150, 150), Color.DarkViolet);
                spriteBatch.End();
                loseMenu.Draw(spriteBatch);
            }
            if (game == "win")
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Game complited! You win!", new Vector2(100, 300), Color.Green);
                spriteBatch.End();
                winMenu.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}
