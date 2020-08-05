using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Hexagon : Figure
    {
        public Hexagon()
        {
            Vertices = new Vertex[6];
        }
        
        public Hexagon(float width, float height, Color color, Vector2f position) : base(width, height, color, position)
        {
            Vertices = new Vertex[6];
            InitializeVertices();
        }
        
        public override IShape Clone() => new Hexagon(Width, Height, ColorCurrent, Position);
        
        public override string ToString()
        {
            return "6-" + Width + "," + Height + "-" + ColorDefault.R + "," + ColorDefault.G + "," + ColorDefault.B +
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