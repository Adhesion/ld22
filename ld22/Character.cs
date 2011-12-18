using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Character
    {
        protected Texture2D sprite;

        protected Vector2 pos;
        protected Vector2 vel;
        protected Vector2 center;

        protected float rotation;

        protected float scale;

        protected float friction;

        protected int hp;
        protected int hpMax;
        protected bool alive;

        protected bool firing;
        protected int fireCounter;
        protected int fireCounterMax;

        protected CharacterManager characterManager;
        protected LevelManager levelManager;

        protected Rectangle boundingBox;
        protected Vector2 boxScale;

        protected SpriteEffects effect;
        protected Color col;

        protected bool bounded;

        public Character(Texture2D _sprite, Vector2 _pos, Vector2 _vel, int _hp, LevelManager l)
        {
            sprite = _sprite;
            pos = _pos;
            vel = _vel;

            center = new Vector2(sprite.Width / 2.0f, sprite.Height / 2.0f);

            hp = _hp;
            hpMax = hp;
            alive = true;

            firing = false;
            fireCounter = 0;
            fireCounterMax = 15;

            scale = 1.0f;

            setBoxScale(new Vector2(1.0f, 1.0f));
            updateBBoxPos();
            boundingBox.X = (int)pos.X;
            boundingBox.Y = (int)pos.Y;

            friction = 0.9f;

            rotation = 0.0f;

            col = Color.White;
            effect = SpriteEffects.None;

            bounded = true;
            levelManager = l;
        }

        public void setCharacterManager(CharacterManager c)
        {
            characterManager = c;
        }

        public void setBoxScale(Vector2 bs)
        {
            boxScale = bs;
            boundingBox.Width = (int)((float)sprite.Width * bs.X);
            boundingBox.Height = (int)((float)sprite.Height * bs.Y);
        }

        private void updateBBoxPos()
        {
            boundingBox.X = (int)pos.X - (int)(center.X * boxScale.X);
            boundingBox.Y = (int)pos.Y - (int)(center.Y * boxScale.Y);
        }

        public Rectangle getBoundingBox()
        {
            return boundingBox;
        }

        public Vector2 getVel()
        {
            return vel;
        }

        public void setVel(Vector2 v)
        {
            vel = v;
        }

        public void addVel(Vector2 v)
        {
            vel += v;
        }

        public float getRotation()
        {
            return rotation;
        }

        public virtual void setRotation(float r)
        {
            rotation = r;
        }

        public virtual void rotate(float r)
        {
            rotation += r;
        }

        public Vector2 getPos()
        {
            return pos;
        }

        public void setPos(Vector2 p)
        {
            pos = p;
        }

        public void setScale(float s)
        {
            scale = s;
        }

        public int getHP()
        {
            return hp;
        }

        public void incHP(int _hp)
        {
            hp += _hp;
            if (hp > hpMax)
            {
                hp = hpMax;
            }
        }

        //overridden by enemy
        public virtual int getType()
        {
            return 0;
        }

        public virtual void render(SpriteBatch batch)
        {
            batch.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            batch.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            batch.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            batch.Draw(sprite,
                pos,
                null,
                col,
                rotation,
                center,
                scale,
                effect,
                0.0f);
            //batch.Draw(Game1.instance.boxSprite, getBoundingBox(), Color.White);
        }

        public virtual void update(GameTime gameTime)
        {
            pos += vel;

            vel *= friction;

            updateBBoxPos();

            if (alive && firing)
            {
                if (fireCounter == 0)
                {
                    fireBullet();
                }
                fireCounter = (fireCounter + 1) % fireCounterMax;
            }

            if (bounded)
            {
                if (pos.X < levelManager.getCurrentLevelArea().Left)
                    pos.X = levelManager.getCurrentLevelArea().Left;
                else if (pos.X > levelManager.getCurrentLevelArea().Right)
                    pos.X = levelManager.getCurrentLevelArea().Right;
                if (pos.Y > levelManager.getCurrentLevelArea().Bottom)
                    pos.Y = levelManager.getCurrentLevelArea().Bottom;
                else if (pos.Y < levelManager.getCurrentLevelArea().Top)
                    pos.Y = levelManager.getCurrentLevelArea().Top;
            }
        }

        public virtual void fireBullet()
        {
            // player and enemy override this
        }

        public bool isAlive()
        {
            return alive;
        }

        public void revive()
        {
            alive = true;
            hp = hpMax;
        }

        public void kill()
        {
            alive = false;
        }

        public void hit(int damage)
        {
            hp -= damage;
            Console.WriteLine("Character hit hp now " + hp);
            if (hp <= 0)
            {
                alive = false;
                Console.WriteLine("Character dead");
            }
        }

        public void setFiring(bool f)
        {
            firing = f;
        }
    }
}
