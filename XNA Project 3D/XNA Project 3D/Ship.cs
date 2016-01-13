using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace XNA_Project_3D
{
    class Ship
    {
        public Model Model;
        public Matrix[] Transforms;

        //Position of the model in world space
        public Vector3 Position = Vector3.Zero;

        public float speed = 0;
        public float startS = GameConstants.shipSpeedNorm;
        //amplifies controller speed input
        private const float VelocityScale = 5.0f;

        //dead or alive
        public bool isActive = true;

        //Velocity of the model, applied each frame to the model's position
        public Vector3 Velocity = Vector3.Zero;

        public Matrix RotationMatrix = Matrix.Identity;
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                RotationMatrix = Matrix.CreateRotationZ(rotation);
            }
        }

         public void Update(KeyboardState currentKeyState)
        {
            //rotation
            if (currentKeyState.IsKeyDown(Keys.A))
                Rotation -= 0.10f;
            else if (currentKeyState.IsKeyDown(Keys.D))
                Rotation += 0.10f;
            //up and down
            if (currentKeyState.IsKeyDown(Keys.Up))
                Position.Y += startS + speed;
            else if (currentKeyState.IsKeyDown(Keys.Down))
                Position.Y -= startS + speed;
            if (currentKeyState.IsKeyDown(Keys.Left))
                Position.X += startS + speed;
            if (currentKeyState.IsKeyDown(Keys.Right))
                Position.X -= startS + speed;
            //moving all directions
            if (currentKeyState.IsKeyDown(Keys.W))
            {
                Velocity = RotationMatrix.Backward * VelocityScale * 5f; // 2.0f is too high
            }
            else if (currentKeyState.IsKeyDown(Keys.S))
            {
                Velocity = RotationMatrix.Forward * VelocityScale * 5f; // 2.0f is too high
            }
            else
                Velocity = RotationMatrix.Forward * 0f;
        }

         public void Draw()
         {
             Matrix shipTransformMatrix = RotationMatrix
                    * Matrix.CreateTranslation(Position);

             //Draw the model, a model can have multiple meshes, so loop
             foreach (ModelMesh mesh in Model.Meshes)
             {
                 //This is where the mesh orientation is set
                 foreach (BasicEffect effect in mesh.Effects)
                 {
                     effect.World =
                         Transforms[mesh.ParentBone.Index] *
                         shipTransformMatrix;
                 }
                 //Draw the mesh, will use the effects set above.
                 mesh.Draw();
             }
         }

    }
}
