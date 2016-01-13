using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_Project_3D
{
    struct Asteroid
    {
        //public Model asteroidModel;
        //public Matrix[] asteroidTransforms;

        public Vector3 position;
        public static Vector3 getPosition;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public void Update(float delta)
        {
            position += direction * speed * GameConstants.AsteroidSpeedAdjustment * delta;
            if (position.X > GameConstants.PlayfieldSizeX)
                position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (position.X < -GameConstants.PlayfieldSizeX)
                position.X += 2 * GameConstants.PlayfieldSizeX;
            if (position.Y > GameConstants.PlayfieldSizeY)
                position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (position.Y < -GameConstants.PlayfieldSizeY)
                position.Y += 2 * GameConstants.PlayfieldSizeY;

            //if (position.Z > GameConstants.PlayFieldSizeZ)
            //    position.Z -= 2 * GameConstants.PlayFieldSizeZ;
            if (position.Z < GameConstants.PlayFieldSizeZ)
                //position.Z += GameConstants.StartZ;
                isActive = false;
            //if (position.Z < GameConstants.CameraHeight)
            //    isActive = false; 
            getPosition = position;
        }

       /* public void Draw()
        {
            Matrix asteroidTransform = Matrix.CreateTranslation(position);
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in asteroidModel.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        asteroidTransforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(10f) *
                        asteroidTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }*/
    }
}
