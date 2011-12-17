using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class CharacterManager
    {
        protected Player player;
        protected List<Character> enemies;
        protected List<Bullet> enemyBullets;
        protected List<Bullet> playerBullets;

        protected List<Character> removedEnemies;
        protected List<Bullet> removedEnemyBullets;
        protected List<Bullet> removedPlayerBullets;

        protected Texture2D bulletSprite;
        protected Texture2D enemySprite;

        protected LevelManager levelManager;

        public CharacterManager(Player p)
        {
            player = p;

            enemies = new List<Character>();
            enemyBullets = new List<Bullet>();
            playerBullets = new List<Bullet>();

            removedEnemies = new List<Character>();
            removedEnemyBullets = new List<Bullet>();
            removedPlayerBullets = new List<Bullet>();
        }

        public void setBulletSprite(Texture2D sprite)
        {
            bulletSprite = sprite;
        }

        public void setEnemySprite(Texture2D sprite)
        {
            enemySprite = sprite;
        }

        public void setLevelManager(LevelManager l)
        {
            levelManager = l;
        }

        public void render(SpriteBatch batch)
        {
            player.render(batch);
            foreach(Character c in enemies)
            {
                c.render(batch);
            }
            foreach(Character c in enemyBullets)
            {
                c.render(batch);
            }
            foreach(Character c in playerBullets)
            {
                c.render(batch);
            }
        }

        public void clear()
        {
            enemies.Clear();
            enemyBullets.Clear();
            playerBullets.Clear();

            removedEnemies.Clear();
            removedEnemyBullets.Clear();
            removedPlayerBullets.Clear();
        }

        public void addBullet(Character c)
        {
            Vector2 vel = c.getVel() * 0.3f;
            List<Bullet> l;
            bool flip;
            int d;

            if (c is Player)
            {
                vel -= new Vector2(0.0f, 7.5f);
                l = playerBullets;
                flip = false;
                d = 10;
            }
            else
            {
                vel += new Vector2(0.0f, 5.0f);
                l = enemyBullets;
                flip = true;
                d = 5;
            }

            Bullet b = new Bullet(bulletSprite, c.getPos(), vel, 1, flip, d);
            l.Add(b);
        }

        public void addEnemy()
        {
            Vector2 pos = new Vector2(Game1.random.Next(0, 640), Game1.random.Next(0, 100));
            Character e = new Enemy1(enemySprite, pos, new Vector2(0.0f, 0.0f), 30);
            e.setCharacterManager(this);
            e.setLevelManager(levelManager);
            enemies.Add(e);
        }

        public void update(GameTime gameTime)
        {
            player.update(gameTime);
            foreach(Character enemy in enemies)
            {
                enemy.update(gameTime);
                if (!enemy.isAlive())
                {
                    removedEnemies.Add(enemy);
                    // add bigger explosion?
                }
            }
            foreach (Bullet bullet in enemyBullets)
            {
                bullet.update(gameTime);
                if (!bullet.isAlive())
                {
                    removedEnemyBullets.Add(bullet);
                }
            }
            foreach (Bullet bullet in playerBullets)
            {
                bullet.update(gameTime);
                if (!bullet.isAlive())
                {
                    removedPlayerBullets.Add(bullet);
                }
            }

            // check player shots
            foreach(Bullet bullet in playerBullets)
            {
                foreach(Character enemy in enemies)
                {
                    if (bullet.getBoundingBox().Intersects(enemy.getBoundingBox()))
                    {
                        enemy.hit(bullet.getDamage());
                        bullet.kill();
                        // add sound/explosion effect here
                    }
                }
            }
            
            // check enemy shots
            foreach (Bullet bullet in enemyBullets)
            {
                if (bullet.getBoundingBox().Intersects(player.getBoundingBox()))
                {
                    player.hit(bullet.getDamage());
                    bullet.kill();
                    // add sound/explosion effect here
                }
            }

            foreach (Bullet b in removedPlayerBullets)
            {
                playerBullets.Remove(b);
            }
            foreach (Bullet b in removedEnemyBullets)
            {
                enemyBullets.Remove(b);
            }
            foreach (Character b in removedEnemies)
            {
                enemies.Remove(b);
            }
        }
    }
}
