using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ld22
{
    class InputHandler
    {
        Player player;

        public InputHandler(Player p)
        {
            player = p;
        }

        public void update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (player.isAlive())
            {
                Vector2 vel = new Vector2(0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    vel.Y -= 0.5f;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    vel.Y += 0.5f;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    vel.X -= 0.5f;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    vel.X += 0.5f;
                }

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    player.rotate(-0.02f);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    player.rotate(0.02f);
                }

                vel = Vector2.Transform(vel, Matrix.CreateRotationZ(player.getRotation()));
                player.addVel(vel);
                player.setFiring(keyboardState.IsKeyDown(Keys.Space));
            }
        }
    }
}
