using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld22
{
    public class LevelManager
    {
        Rectangle currentLevelArea;
        int currentLevel;

        public LevelManager()
        {
            currentLevelArea = new Rectangle(-250, -250, 500, 500);
            currentLevel = 0;
        }

        public Rectangle getCurrentLevelArea()
        {
            return currentLevelArea;
        }
    }
}
