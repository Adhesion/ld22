using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Player : Character
    {
        public Player(Texture2D _sprite, Vector2 _pos, Vector2 _vel, int _hp, LevelManager l) :
            base(_sprite, _pos, _vel, _hp, l)
        {
            setBoxScale(new Vector2(0.1f, 0.1f));
        }

        public override void fireBullet()
        {
            Console.WriteLine("player fired at " + pos);
            characterManager.addBullet(this, Color.Yellow, 10);
        }
    }
}
