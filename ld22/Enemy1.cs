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
        protected int type;

        protected bool chase;

        protected float destRotation;

        protected float maxVel;
        protected Color bulletColor;
        protected int bulletDamage;

        public Enemy1(Texture2D s, Vector2 p, Vector2 v, int _hp, LevelManager l, int t) :
            base(s, p, v, _hp, l)
        {
            fireCounterMax = 30;
            aiCounter = 0;
            aiCounterMax = 40;
            faceCounter = 0;
            faceCounterMax = Game1.random.Next(10, 15);
            maxVel = 4.0f;

            setBoxScale(new Vector2(0.9f, 0.9f));
            type = t;

            chase = false;
            bulletDamage = type + 4;

            switch (type)
            {
                case 0:
                    fireCounterMax = 30;
                    hp = 30;
                    bulletColor = Color.OrangeRed;
                    break;
                case 1:
                    fireCounterMax = 20;
                    hp = 40;
                    bulletColor = Color.OrangeRed;
                    break;
                case 2:
                    fireCounterMax = 12;
                    hp = 60;
                    bulletColor = Color.OrangeRed;
                    break;
                case 3:
                    fireCounterMax = 10;
                    hp = 100;
                    scale = 3.0f;
                    setBoxScale(new Vector2(1.5f, 1.5f));
                    maxVel = 3.0f;
                    bulletColor = Color.White;
                    break;
            }
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.update(gameTime);
            if (characterManager.getCam().isOnCamera(pos))
            {
                if (aiCounter == 0)
                {
                    runAI();
                }
                if (faceCounter == 0)
                {
                    facePlayer();
                }

                if (destRotation < 0)
                {
                    destRotation += (float)Math.PI * 2.0f;
                }
                if (destRotation > (float)Math.PI * 2.0f)
                {
                    destRotation -= (float)Math.PI * 2.0f;
                }

                aiCounter = (aiCounter + 1) % aiCounterMax;
                faceCounter = (faceCounter + 1) % faceCounterMax;
                rotation += (destRotation - rotation) / 10.0f;
            }
            else
            {
                // to make sure they don't get stuck in a firing state
                setFiring(false);
            }
        }

        public override int getType()
        {
            return type;
        }

        public bool getChase()
        {
            return chase;
        }

        public void setChase(bool c)
        {
            chase = c;
        }

        public override void fireBullet()
        {
            characterManager.addBullet(this, bulletColor, bulletDamage);
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
            Vector2 playerVec = (player.getPos() - pos) + (player.getVel() * 70.0f);
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

            if (chase)
            {
                float dist = Vector2.Distance(player.getPos(), pos);
                Vector2 followVel = playerVec / 40.0f * dist / 1000.0f;
                if (followVel.X > maxVel)
                {
                    followVel.X = maxVel;
                }
                if (followVel.Y > maxVel)
                {
                    followVel.Y = maxVel;
                }
                float velX = ((float)Game1.random.NextDouble() * 2 * maxVel) - maxVel;
                float velY = ((float)Game1.random.NextDouble() * 2 * maxVel) - maxVel;
                Vector2 randVel = new Vector2(velX, velY);
                addVel(followVel + randVel);
            }
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
