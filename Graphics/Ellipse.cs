using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Ellipse : Figure
    {
        public Ellipse()
        {
            Vertices = new Vertex[36];
        }
        
        public Ellipse(float width, float height, Color color, Vector2f position) : base(width, height, color, position)
        {
            Vertices = new Vertex[36];
            InitializeVertices();
        }
        
        public override IShape Clone() => new Ellipse(Width, Height, ColorCurrent, Position);
        
        public override string ToString()
        {
            return "2-" + Width + "," + Height + "-" + ColorDefault.R + "," + ColorDefault.G + "," + ColorDefault.B +
                   "-" + Position.X + "," + Position.Y;
        }

        protected sealed override void InitializeVertices()
        {
            int degrees = 0;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2f offset = new Vector2f(Width / 2, Height / 2);
                offset.X *= Convert.ToSingle(Math.Cos(degrees * Math.PI / 180));
                offset.Y *= Convert.ToSingle(Math.Sin(degrees * Math.PI / 180));
                Vertices[i] = new Vertex(Position + offset, ColorCurrent);
                degrees += 360 / Vertices.Length;
            }
        }
    }
}