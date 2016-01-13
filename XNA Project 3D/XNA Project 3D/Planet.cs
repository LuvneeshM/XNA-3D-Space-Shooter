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
    class Planet
    {
        public Model Model;
        public Matrix[] Transforms;

        // The aspect ratio determines how to scale 3d to 2d projection.
        public float aspectRatio;

        //Position of the model in world space
        public Vector3 modelPosition = new Vector3();

        public Matrix RotationMatrix = Matrix.Identity;
        private float modelRotation;
        public float rotate;

        public void Update(GameTime gameTime)
        {
            modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds *
        MathHelper.ToRadians(rotate);
        }

        public void Draw()
        {
            Matrix transformMatrix = Matrix.CreateRotationY(modelRotation) 
                * Matrix.CreateTranslation(modelPosition);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.World =
                       Transforms[mesh.ParentBone.Index] *
                       transformMatrix *
                       Matrix.CreateScale(aspectRatio, aspectRatio, aspectRatio); 
                }
                mesh.Draw();
            }
        }
    }
}
