using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_Project_3D
{
    class GameConstants
    {
        //camera constants
        public const float CameraHeight = -5000.0f;
        public const float PlayfieldSizeX = 3500.0f;
        public const float PlayfieldSizeY = 2000.0f;
        public const float PlayFieldSizeZ = -6000.0f;

        public const float BulletFieldSize = 25000f;

        public const float StartZ = 50000.0f;

        //asteroid constants
        public const int NumAsteroids = 2;

        public const float AsteroidMinSpeed = 250.0f;
        public const float AsteroidMaxSpeed = 300.0f;

        public const float AsteroidSpeedAdjustment = 5.0f;

        public const float AsteroidBoundingSphereScale = 80f;  //alot% size
        public const float AsteroidBoundingSphereScaleShip = 80f;
        public const float ShipBoundingSphereScale = 10f;  //50% size

        public const float shipSpeedNorm = 30f;

        public const int NumBullets = 10000;
        public const float BulletSpeedAdjustment = 100.0f;

        public const int ShotPenalty = 1;
        public const int DeathPenalty = 100;
        public const int WarpPenalty = 50;
        public const int KillBonus = 25;
    }
}
