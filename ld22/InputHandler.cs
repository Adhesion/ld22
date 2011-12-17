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

            Vector2 vel = new Vector2(0.0f, 0.0f);
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                vel.Y -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                vel.Y += 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                vel.X -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                vel.X += 0.5f;
            }

            player.addVel(vel);
            player.setFiring(keyboardState.IsKeyDown(Keys.Space));
        }
    }
}
