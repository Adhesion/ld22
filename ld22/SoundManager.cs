using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ld22
{
    public class SoundManager
    {
        protected Song[] songs;
        protected SoundEffectInstance currentSong;

        protected SoundEffect playerFireS;
        protected SoundEffect enemyFireS;
        protected SoundEffect playerHitS;
        protected SoundEffect enemyHitS;
        protected SoundEffect playerDeathS;
        protected SoundEffect enemyDeathS;
        protected SoundEffect bossDeathS;
        protected SoundEffect eggS;

        public SoundManager(ContentManager Content)
        {
            songs = new Song[6];
            songs[0] = Content.Load<Song>("intro");
            songs[1] = Content.Load<Song>("disco");
            songs[2] = Content.Load<Song>("trip");
            songs[3] = Content.Load<Song>("bosssong");
            songs[4] = Content.Load<Song>("end");

            playerFireS = Content.Load<SoundEffect>("playerfire");
            enemyFireS = Content.Load<SoundEffect>("enemyfire");
            playerHitS = Content.Load<SoundEffect>("playerhit");
            enemyHitS = Content.Load<SoundEffect>("enemyhit");
            playerDeathS = Content.Load<SoundEffect>("playerdeath");
            enemyDeathS = Content.Load<SoundEffect>("enemydeath");
            bossDeathS = Content.Load<SoundEffect>("bossdeath");
            eggS = Content.Load<SoundEffect>("eggsound");
        }

        public void setLevel(int i)
        {
            MediaPlayer.Stop();
            int index = i;
            if (index >= 4)
            {
                index -= 2;
            }
            else if (index >= 2)
            {
                index--;
            }
            
            //Console.WriteLine("SoundManager::setlevel(): level " + i + ", " + " index " + index);
            MediaPlayer.Play(songs[index]);
            MediaPlayer.IsRepeating = true;
            //currentSong = songs[index].Play(1.0f, 0.0f, 0.0f, true);
        }

        public void stop()
        {
            MediaPlayer.Stop();
        }

        public void playerFire()
        {
            SoundEffectInstance i = playerFireS.Play();
            i.Volume = 0.3f;
        }

        public void enemyFire()
        {
            SoundEffectInstance i = enemyFireS.Play();
            i.Volume = 0.2f;
        }

        public void playerHit()
        {
            SoundEffectInstance i = playerHitS.Play();
            i.Volume = 0.45f;
        }

        public void enemyHit()
        {
            SoundEffectInstance i = enemyHitS.Play();
            i.Volume = 0.25f;
        }

        public void playerDeath()
        {
            SoundEffectInstance i = playerDeathS.Play();
        }

        public void enemyDeath()
        {
            SoundEffectInstance i = enemyDeathS.Play();
            i.Volume = 0.5f;
        }

        public void bossDeath()
        {
            SoundEffectInstance i = bossDeathS.Play();
            i.Volume = 0.6f;
        }

        public void egg()
        {
            SoundEffectInstance i = eggS.Play();
            i.Volume = 0.35f;
        }
    }
}
