using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class Camera
    {
        protected float zoom;
        protected Vector2 pos;
        protected Vector2 screenPos;

        protected Matrix transform;

        protected Player player;

        protected LevelManager levelManager;

        protected Rectangle camArea;

        public Camera(GraphicsDevice graphics, Vector2 _pos, Player _player, LevelManager _levelManager)
        {
            screenPos.X = graphics.Viewport.Width / 2;
            screenPos.Y = graphics.Viewport.Height / 2;
            pos = _pos;
            player = _player;
            zoom = 0.85f;
            levelManager = _levelManager;
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

        public void setPos(Vector2 p)
        {
            pos = p;
        }

        public void update(GameTime gameTime)
        {
            move(player.getPos() - pos);

            camArea = levelManager.getCurrentLevelArea();
            camArea.Inflate(-200, -200);

            if (pos.X < camArea.Left)
                pos.X = camArea.Left;
            else if (pos.X > camArea.Right)
                pos.X = camArea.Right;
            if (pos.Y > camArea.Bottom)
                pos.Y = camArea.Bottom;
            else if (pos.Y < camArea.Top)
                pos.Y = camArea.Top;

            /*if (pos.X - screenPos.X < levelManager.getCurrentLevelArea().Left)
                pos.X = levelManager.getCurrentLevelArea().Left + screenPos.X;
            else if (pos.X + screenPos.X > levelManager.getCurrentLevelArea().Right)
                pos.X = levelManager.getCurrentLevelArea().Right - screenPos.X;
            if (pos.Y + screenPos.Y > levelManager.getCurrentLevelArea().Bottom)
                pos.Y = levelManager.getCurrentLevelArea().Bottom - screenPos.Y;
            else if (pos.Y - screenPos.Y < levelManager.getCurrentLevelArea().Top)
                pos.Y = levelManager.getCurrentLevelArea().Top + screenPos.Y;*/

            updateTransform();
            //Console.WriteLine(transform);
        }

        private void updateTransform()
        {
            transform = Matrix.Identity *
                        Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0.0f)) *
                        Matrix.CreateRotationZ(-player.getRotation()) *
                        Matrix.CreateScale(zoom, zoom, 0) *
                        Matrix.CreateTranslation(new Vector3(screenPos.X, screenPos.Y, 0.0f));
            //transform = Matrix.CreateTranslation(new Vector3(screenPos - pos, 0.0f));
            //transform = Matrix.Identity;
        }

        public Matrix getTransform()
        {
            return transform;
        }

        public bool isOnCamera(Vector2 p)
        {
            float left = pos.X - 500;
            float right = pos.X + 500;
            float top = pos.Y - 500;
            float bottom = pos.Y + 500;
            return (left < p.X && p.X < right &&
                top < p.Y && p.Y < bottom);
        }
    }
}
