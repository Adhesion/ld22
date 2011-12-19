using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Bullet : Character
    {
        protected int damage;
        protected int lifetime;
        protected int lifetimeThreshold;

        protected bool grow;
        protected float growScale;

        protected bool stay;

        public Bullet(Texture2D s, Vector2 p, Vector2 v, int _hp, LevelManager l, Color c, int d) :
            base(s, p, v, _hp, l)
        {
            friction = 1.0f;
            lifetimeThreshold = 20;
            lifetime = 90;

            damage = d;

            col = c;

            bounded = false;
            grow = false;
            stay = false;
            growScale = 0.0f;
        }

        public bool isStay()
        {
            return stay;
        }

        public void setStay(bool s)
        {
            stay = s;
        }

        public int getDamage()
        {
            return damage;
        }

        public int getLifetime()
        {
            return lifetime;
        }

        public int getLifetimeThreshold()
        {
            return lifetimeThreshold;
        }

        public void setLifetime(int l)
        {
            lifetime = l;
        }

        public void setGrow(bool g, float s)
        {
            grow = g;
            growScale = s;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            lifetime--;
            if (lifetime <= 0)
            {
                alive = false;
            }
            if (grow)
            {
                scale += growScale;
                setBoxScale(boxScale + new Vector2(growScale, growScale));
            }
        }

        public override void render(SpriteBatch batch)
        {
            if (lifetime <= lifetimeThreshold)
            {
                col.A = (byte)((float)lifetime / 25.0f * 255.0f);
            }
            base.render(batch);
        }
    }
}
