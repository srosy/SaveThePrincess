using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaveThePrincess.Graphics
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public float Speed { get; set; }
        public int Width { get; set; } = 16;
        public int Height { get; set; } = 22;
        public Rectangle SourceRectangle { get; set; }
    }
}
