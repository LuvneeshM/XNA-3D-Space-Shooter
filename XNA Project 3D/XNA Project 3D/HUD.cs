using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_Project_3D
{
    class HUD
    {
        SpriteBatch spriteBatch;

        public Texture2D texture;
        Vector2 position;
        public Rectangle rectangle;

        float elapsed;
        float delay = 500f;
        int restore;
        public int loseHealth;

        public void Initialize(SpriteBatch s, int r, int x, int y, int l)
        {
            spriteBatch = s;
            restore = r;
            loseHealth = l;
            position = new Vector2(x, y);
            rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime, int typeUpdate)
        {
            if (typeUpdate == 0 && rectangle.Width > loseHealth)
                rectangle.Width -= loseHealth;
            else if (typeUpdate == 0 && rectangle.Width < loseHealth)
                rectangle.Width = 0;

            if (typeUpdate == 1)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsed >= delay && rectangle.Width != texture.Width)
                {
                    rectangle.Width += restore;
                    elapsed = 0;
                }
            }
        }


        public void Draw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, rectangle, Color.White);
            spriteBatch.End();
        }

    }
}
