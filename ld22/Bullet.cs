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

        public Bullet(Texture2D s, Vector2 p, Vector2 v, int _hp, LevelManager l, Color c, int d) :
            base(s, p, v, _hp, l)
        {
            friction = 1.0f;
            lifetime = 200;
            damage = d;

            col = c;

            bounded = false;
        }

        public int getDamage()
        {
            return damage;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            lifetime--;
            if (lifetime <= 0)
            {
                alive = false;
            }
        }
    }
}
