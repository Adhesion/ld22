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
        protected List<Character> eggs;

        protected List<Character> removedEnemies;
        protected List<Bullet> removedEnemyBullets;
        protected List<Bullet> removedPlayerBullets;
        protected List<Character> removedEggs;
        protected List<Character> removeds;

        protected Texture2D bulletSprite;
        protected Texture2D[] enemySprites;
        protected Texture2D eggSprite;

        protected Camera cam;

        protected LevelManager levelManager;
        protected SoundManager soundManager;

        protected int deathTimer;
        protected int deathTimerMax;

        public CharacterManager(Player p)
        {
            player = p;

            enemies = new List<Character>();
            enemyBullets = new List<Bullet>();
            playerBullets = new List<Bullet>();
            eggs = new List<Character>();

            removedEnemies = new List<Character>();
            removedEnemyBullets = new List<Bullet>();
            removedPlayerBullets = new List<Bullet>();
            removedEggs = new List<Character>();

            deathTimerMax = 180;
            deathTimer = deathTimerMax;
        }

        public void setBulletSprite(Texture2D sprite)
        {
            bulletSprite = sprite;
        }

        public void setEnemySprites(Texture2D[] sprite)
        {
            enemySprites = sprite;
        }

        public void setEggSprite(Texture2D sprite)
        {
            eggSprite = sprite;
        }

        public void setLevelManager(LevelManager l)
        {
            levelManager = l;
        }

        public void setSoundManager(SoundManager s)
        {
            soundManager = s;
        }

        public Player getPlayer()
        {
            return player;
        }

        public Camera getCam()
        {
            return cam;
        }

        public void setCam(Camera c)
        {
            cam = c;
        }

        public void render(SpriteBatch batch)
        {
            if (player.isAlive())
            {
                player.render(batch);
            }
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
            foreach (Character c in eggs)
            {
                c.render(batch);
            }
        }

        public void clear()
        {
            enemies.Clear();
            enemyBullets.Clear();
            playerBullets.Clear();
            eggs.Clear();

            removedEnemies.Clear();
            removedEnemyBullets.Clear();
            removedPlayerBullets.Clear();
            removedEggs.Clear();
        }

        public void addBullet(Character c, Color col, int damage)
        {
            Vector2 vel = c.getVel() * 0.1f;
            vel += (new Vector2((float)Math.Cos(c.getRotation() - Math.PI / 2),
                (float)Math.Sin(c.getRotation() - Math.PI / 2))) * 4.0f;
            List<Bullet> l;

            if (c is Player)
            {
                vel *= 3.0f;
                l = playerBullets;
                soundManager.playerFire();
            }
            else
            {
                vel *= 1.5f;
                l = enemyBullets;
                if (enemyBullets.Count < 100)
                {
                    soundManager.enemyFire();
                }
            }

            Bullet b = new Bullet(bulletSprite, c.getPos(), vel, 1, levelManager, col, damage);
            b.setRotation(c.getRotation());
            l.Add(b);
        }

        public void addEnemy(Vector2 p, int type)
        {
            //Vector2 pos = new Vector2(Game1.random.Next(-250, 250), Game1.random.Next(-250, -100));
            Character e = new Enemy1(enemySprites[type], p, new Vector2(0.0f, 0.0f), 30, levelManager, type);
            e.setCharacterManager(this);
            enemies.Add(e);
        }

        public void addEgg(Vector2 pos)
        {
            Character e = new Character(eggSprite, pos, new Vector2(0.0f, 0.0f), 1, levelManager);
            eggs.Add(e);
        }

        public int getEggNum()
        {
            return eggs.Count;
        }

        public int getEnemyNum()
        {
            return enemies.Count;
        }

        public List<Character> getEggs()
        {
            return eggs;
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
                    if (enemy.getType() == 3)
                    {
                        soundManager.bossDeath();
                    }
                    else
                    {
                        soundManager.enemyDeath();
                    }
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
            foreach (Character egg in eggs)
            {
                egg.update(gameTime);
                if (!egg.isAlive())
                {
                    removedEggs.Add(egg);
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
                        soundManager.enemyHit();
                    }
                }
            }
            
            // check enemy shots
            if (player.isAlive())
            {
                foreach (Bullet bullet in enemyBullets)
                {
                    if (bullet.getBoundingBox().Intersects(player.getBoundingBox()))
                    {
                        player.hit(bullet.getDamage());
                        bullet.kill();
                        soundManager.playerHit();
                    }
                }
            }

            // check eggs
            foreach (Character egg in eggs)
            {
                if (egg.isAlive() && egg.getBoundingBox().Intersects(player.getBoundingBox()))
                {
                    egg.kill();
                    player.incHP(50);
                    soundManager.egg();
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
            foreach (Character e in removedEggs)
            {
                eggs.Remove(e);
            }

            // death handling
            if (!player.isAlive())
            {
                // after the wait
                if (deathTimer == 0)
                {
                    player.revive();
                    deathTimer = deathTimerMax;
                    player.setPos(new Vector2(0.0f, 0.0f));
                    cam.setPos(new Vector2(0.0f, 0.0f));
                    if (player.getLives() < 0)
                    {
                        levelManager.initLevel(0);
                        player.setLives(3);
                    }
                }
                // meaning, just died
                else
                {
                    if (deathTimer == deathTimerMax)
                    {
                        soundManager.playerDeath();
                        player.decLives();
                        if (player.getLives() < 0)
                        {
                            Game1.instance.setGameOver(true);
                        }
                    }
                    deathTimer--;
                }
            }
        }
    }
}
