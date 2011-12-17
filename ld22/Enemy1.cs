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
        protected int faceCounter;
        protected int faceCounterMax;

        protected float destRotation;

        float maxVel;

        public Enemy1(Texture2D s, Vector2 p, Vector2 v, int _hp, LevelManager l) :
            base(s, p, v, _hp, l)
        {
            fireCounterMax = 30;
            aiCounter = 0;
            aiCounterMax = 40;
            faceCounter = 0;
            faceCounterMax = Game1.random.Next(10, 15);
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
            if (faceCounter == 0)
            {
                facePlayer();
            }

            aiCounter = (aiCounter + 1) % aiCounterMax;
            faceCounter = (faceCounter + 1) % faceCounterMax;
            rotation += (destRotation - rotation) / 10.0f;
        }

        public override void fireBullet()
        {
            characterManager.addBullet(this, Color.OrangeRed, 10);
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

        protected void facePlayer()
        {
            float r;
            Player player = characterManager.getPlayer();
            Vector2 playerVec = (player.getPos() - pos) + (player.getVel() * 50.0f);
            r = (float)Math.Atan((double)(playerVec.Y / playerVec.X)) + (float)Math.PI / 2;
            if (playerVec.X < -0.01f)
                r = (float)Math.PI + r;
            else
                r = (2.0f * (float)Math.PI) + r;
            //rotate((rotation - r)/20.0f);
            //setRotation(r + (((float)Game1.random.NextDouble() * (float)Math.PI / 16) - (float)Math.PI / 8));
            setRotation(r);
            //rotation = r;
            //destRotation = r;
        }

        public override void setRotation(float r)
        {
            destRotation = r;
        }

        public override void rotate(float r)
        {
            destRotation += r;
        }
    }
}
