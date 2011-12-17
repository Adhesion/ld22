using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class LevelManager
    {
        Rectangle currentLevelArea;
        int currentLevel;
        int levelSize;

        Texture2D[] starTex;
        Texture2D[] backTex;

        CharacterManager characterManager;

        protected IDictionary<float, List<Character>> backgrounds;

        public LevelManager(Texture2D[] _starTex, Texture2D[] _backTex )
        {
            backgrounds = new Dictionary<float, List<Character>>();
            currentLevelArea = new Rectangle(-500, -500, 1000, 1000);
            currentLevel = 0;

            starTex = _starTex;
            backTex = _backTex;
        }

        public void setCharacterManager(CharacterManager c)
        {
            characterManager = c;
        }

        public Rectangle getCurrentLevelArea()
        {
            return currentLevelArea;
        }

        public void initLevel(int level)
        {
            characterManager.clear();
            Console.WriteLine("Init level " + level);
            currentLevel = level;

            if (currentLevel >= 1 && currentLevel <= 4)
            {
                levelSize = 2500 + (currentLevel * 400);
                currentLevelArea = new Rectangle(-levelSize / 2, -levelSize / 2, levelSize, levelSize);
                spawnEnemies();
            }

            makeBackground();
        }

        public void spawnEnemies()
        {
            int numEggs = currentLevel + 5;
            for (int i = 0; i < numEggs; i++)
            {
                int ex = Game1.random.Next(levelSize / 6, (levelSize / 2) - 200);
                int ey = Game1.random.Next(levelSize / 6, (levelSize / 2) - 200);
                if (Game1.random.NextDouble() < 0.5)
                    ex *= -1;
                if (Game1.random.NextDouble() < 0.5)
                    ey *= -1;
                characterManager.addEgg(new Vector2(ex, ey));
                Console.WriteLine("spawned egg at " + ex + ", " + ey);
            }

            int x = (-levelSize / 2) + 200;
            int y = (-levelSize / 2) + 200;
            for (; x < (levelSize / 2) - 200; x += 100)
            {
                for (; y < (levelSize / 2) - 200; y += 100)
                {
                    Vector2 p = new Vector2(x, y);
                    float prob = 0.0f;
                    List<Character> eggs = characterManager.getEggs();
                    foreach (Character e in eggs)
                    {
                        float dist = Vector2.Distance(e.getPos(), p);
                        prob += (100.0f / dist);
                        //Console.WriteLine("at " + x + ", " + y + ", dist is " + dist + ", prob is " + prob);
                        if (prob > 1.0f)
                        {
                            characterManager.addEnemy(p);
                        }
                    }
                }
                y = (-levelSize / 2) - 200;
            }
        }

        public void makeBackground()
        {
            backgrounds.Clear();
            float zoom = 0.2f;
            for (int i = 0; i < 3; i++)
            {
                List<Character> starfield = new List<Character>();
                for (int j = 0; j < 200; j++)
                {
                    Vector2 pos = new Vector2(Game1.random.Next(currentLevelArea.Left, currentLevelArea.Right) * (2.0f / zoom),
                        Game1.random.Next(currentLevelArea.Top, currentLevelArea.Bottom) * (2.0f / zoom));
                    int st = Game1.random.Next(0, 3);
                    Texture2D starT = starTex[st];
                    Character star = new Character(starT, pos, new Vector2(0.0f, 0.0f), 1, this);
                    float rot = (float)(Game1.random.NextDouble() * 2.0f * Math.PI);
                    star.setRotation(rot);
                    starfield.Add(star);
                }

                backgrounds.Add(new KeyValuePair<float, List<Character>>(zoom, starfield));
                zoom *= 1.4f;
            }

            if (currentLevel >= 1 && currentLevel <= 4)
            {
                List<Character> back1 = new List<Character>();
                List<Character> back2 = new List<Character>();
                Vector2 p = new Vector2(currentLevelArea.X, currentLevelArea.Y);

                TileBackground tb1 = new TileBackground(backTex[currentLevel-1], p, new Vector2(0.0f, 0.0f), 1, this);
                float r = (float)(Game1.random.NextDouble() * 2.0f * Math.PI);
                tb1.setRotation(1.0f);
                back1.Add(tb1);
                TileBackground tb2 = new TileBackground(backTex[currentLevel - 1], p, new Vector2(0.0f, 0.0f), 1, this);
                r = (float)(Game1.random.NextDouble() * 2.0f * Math.PI);
                tb2.setRotation(2.2f);
                back2.Add(tb2);

                backgrounds.Add(new KeyValuePair<float, List<Character>>(0.5f, back1));
                backgrounds.Add(new KeyValuePair<float, List<Character>>(0.75f, back2));
            }

            if (currentLevel == 5)
            {
                // add earth/moon
            }
        }

        public void render(SpriteBatch batch, float zoomLevel)
        {
            List<Character> objects;
            if (backgrounds.TryGetValue(zoomLevel, out objects))
            {
                foreach (Character c in objects)
                {
                    c.render(batch);
                }
            }
        }

        public ICollection<float> getBackgroundLevels()
        {
            return backgrounds.Keys;
        }

        public bool checkWinCondition()
        {
            bool ret = false;
            if (currentLevel >= 1 && currentLevel <= 4)
            {
                ret = (characterManager.getEggNum() == 0);
            }
            return ret;
        }

        public void update(GameTime gameTime)
        {
            if (checkWinCondition() && currentLevel < 5)
            {
                initLevel(++currentLevel);
            }
        }
    }
}
