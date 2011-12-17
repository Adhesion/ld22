using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class TileBackground : Character
    {
        protected Rectangle rect;

        public TileBackground(Texture2D _sprite, Vector2 _pos, Vector2 _vel, int _hp, LevelManager l) :
            base(_sprite, _pos, _vel, _hp, l)
        {
            rect = l.getCurrentLevelArea();
            rect.Inflate(10000, 10000);
            pos.X -= 5000;
            pos.Y -= 5000;
            center.X += 5000;
            center.Y += 5000;
            rotation = (float)Game1.random.NextDouble() * (float)Math.PI * 2.0f;
        }

        public override void render(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            batch.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Point;
            batch.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Point;
            batch.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Point;
            batch.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            batch.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            batch.Draw(sprite,
                pos,
                rect,
                col,
                rotation,
                center,
                15.0f,
                effect,
                0.0f);
        }
    }
}
