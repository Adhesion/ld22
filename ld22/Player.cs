﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Player : Character
    {
        protected int numLives;
        protected int bombCooldown;
        protected int numEggs;

        public Player(Texture2D _sprite, Vector2 _pos, Vector2 _vel, int _hp, LevelManager l) :
            base(_sprite, _pos, _vel, _hp, l)
        {
            setBoxScale(new Vector2(0.25f, 0.25f));
            fireCounterMax = 10;
            numLives = 3;
            bombCooldown = 0;
            numEggs = 0;
        }

        public int getLives()
        {
            return numLives;
        }

        public void incLives()
        {
            numLives++;
        }

        public void decLives()
        {
            numLives--;
        }

        public void setLives(int i)
        {
            numLives = i;
        }

        public int getEggs()
        {
            return numEggs;
        }

        public void incEggs()
        {
            numEggs++;
        }

        public void resetEggs()
        {
            numEggs = 0;
        }

        public override void fireBullet()
        {
            //Console.WriteLine("player fired at " + pos);
            characterManager.addBullet(this, Color.Yellow, 7);
        }

        public void fireBomb()
        {
            if (bombCooldown == 0)
            {
                characterManager.addBomb(this, Color.Yellow, 2);
                bombCooldown = 600;
            }
        }

        public int getBombCooldown()
        {
            return bombCooldown;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            if (bombCooldown > 0)
            {
                bombCooldown--;
            }
        }
    }
}
