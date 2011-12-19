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
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.Buttons.Back == ButtonState.Pressed ||
                keyboardState.IsKeyDown(Keys.Escape))
            {
                Game1.instance.Exit();
            }

            if (player.isAlive())
            {
                Vector2 vel = new Vector2(0.0f, 0.0f);
                float inVel = 0.65f;
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    vel.Y -= inVel;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    vel.Y += inVel;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    vel.X -= inVel;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    vel.X += inVel;
                }

                if (gamePadState.ThumbSticks.Left.X > 0.1f ||
                    gamePadState.ThumbSticks.Left.X < -0.1f)
                {
                    vel.X += inVel * gamePadState.ThumbSticks.Left.X;
                }
                if (gamePadState.ThumbSticks.Left.Y > 0.1f ||
                    gamePadState.ThumbSticks.Left.Y < -0.1f)
                {
                    vel.Y += inVel * -gamePadState.ThumbSticks.Left.Y;
                }

                float rotVal = 0.0225f;
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    player.rotate(-rotVal);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    player.rotate(rotVal);
                }

                rotVal = 0.025f;
                if (gamePadState.ThumbSticks.Right.X > 0.1f ||
                    gamePadState.ThumbSticks.Right.X < -0.1f)
                {
                    player.rotate(rotVal * gamePadState.ThumbSticks.Right.X);
                }
                if (gamePadState.ThumbSticks.Right.Y > 0.1f ||
                    gamePadState.ThumbSticks.Right.Y < -0.1f)
                {
                    player.rotate(-rotVal * gamePadState.ThumbSticks.Right.Y);
                }

                vel = Vector2.Transform(vel, Matrix.CreateRotationZ(player.getRotation()));
                player.addVel(vel);
                player.setFiring(keyboardState.IsKeyDown(Keys.Space) ||
                    (gamePadState.Triggers.Right > 0.01f));
                if (keyboardState.IsKeyDown(Keys.LeftControl) || gamePadState.Triggers.Left > 0.1f)
                {
                    player.fireBomb();
                }
            }
        }
    }
}
