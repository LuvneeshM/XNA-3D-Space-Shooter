using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA_Project_3D
{
    struct Bullet
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public void Update(float delta)
        {
            position += direction * speed *
                        GameConstants.BulletSpeedAdjustment * delta;

            if (position.X > GameConstants.BulletFieldSize ||
                position.X < -GameConstants.BulletFieldSize ||
                position.Y > GameConstants.BulletFieldSize ||
                position.Y < -GameConstants.BulletFieldSize)
                isActive = false;

        }
    }
}
