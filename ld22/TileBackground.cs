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
        public TileBackground(Texture2D _sprite, Vector2 _pos, Vector2 _vel, int _hp, LevelManager l) :
            base(_sprite, _pos, _vel, _hp, l)
        {

        }

        public override void render(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.render(batch);
        }
    }
}
