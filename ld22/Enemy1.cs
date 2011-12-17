using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Enemy1 : Character
    {
        protected int aiCounter;
        protected int aiCounterMax;

        float maxVel;

        public Enemy1(Texture2D s, Vector2 p, Vector2 v, int _hp) :
            base(s, p, v, _hp)
        {
            fireCounterMax = 30;
            aiCounter = 0;
            aiCounterMax = 40;
            maxVel = 4.0f;

            setBoxScale(new Vector2(0.9f, 0.9f));
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.update(gameTime);

            if (aiCounter == 0)
            {
                runAI();
            }

            aiCounter = (aiCounter + 1) % aiCounterMax;
        }

        protected virtual void runAI()
        {
            float val = (float)Game1.random.NextDouble();
            setFiring(val >= 0.7f);
            if (val <= 0.5f)
            {
                float velX = ((float)Game1.random.NextDouble() * 2 * maxVel) - maxVel;
                float velY = ((float)Game1.random.NextDouble() * 2 * maxVel) - maxVel;
                addVel(new Vector2(velX, velY));
            }
        }
    }
}
