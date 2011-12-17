using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    class Camera
    {
        protected float zoom;
        protected Vector2 pos;
        protected Vector2 screenPos;

        protected Matrix transform;

        protected Player player;

        public Camera(GraphicsDevice graphics, Vector2 _pos, Player _player)
        {
            screenPos.X = graphics.Viewport.Width / 2;
            screenPos.Y = graphics.Viewport.Height / 2;
            pos = _pos;
            player = _player;
            zoom = 1.0f;
        }

        public float getZoom()
        {
            return zoom;
        }

        public void setZoom(float _zoom)
        {
            zoom = _zoom;
            updateTransform();
        }

        public void move(Vector2 m)
        {
            pos += m;
        }

        public void update(GameTime gameTime)
        {
            move(player.getPos() - pos);

            updateTransform();
            //Console.WriteLine(transform);
        }

        private void updateTransform()
        {
            transform = Matrix.Identity *
                        Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0.0f)) *
                        Matrix.CreateRotationZ(0.0f) *
                        Matrix.CreateScale(zoom, zoom, 0) *
                        Matrix.CreateTranslation(new Vector3(screenPos.X, screenPos.Y, 0.0f));
            //transform = Matrix.CreateTranslation(new Vector3(screenPos - pos, 0.0f));
            //transform = Matrix.Identity;
        }

        public Matrix getTransform()
        {
            return transform;
        }
    }
}
