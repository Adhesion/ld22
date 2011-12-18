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

        public void setGrow(bool g)
        {
            grow = g;
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
                scale += 0.05f;
                setBoxScale(boxScale + new Vector2(0.05f, 0.05f));
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
