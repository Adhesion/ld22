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

        Texture2D[] starTex;
        Texture2D[] backTex;

        protected IDictionary<float, List<Character>> backgrounds;

        public LevelManager(Texture2D[] _starTex, Texture2D[] _backTex )
        {
            backgrounds = new Dictionary<float, List<Character>>();
            currentLevelArea = new Rectangle(-500, -500, 1000, 1000);
            currentLevel = 0;

            starTex = _starTex;
            backTex = _backTex;
        }

        public Rectangle getCurrentLevelArea()
        {
            return currentLevelArea;
        }

        public void initLevel(int level)
        {
            currentLevel = level;
            makeBackground();
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
    }
}
