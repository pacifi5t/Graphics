using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Circle : RadiusBasedFigure
    {
        public Circle()
        {
            Vertices = new Vertex[60];
        }

        public Circle(float radius, Color color, Vector2f position) : base(radius, color, position)
        {
            Vertices = new Vertex[60];
            InitializeVertices();
        }
        
        public override IShape Clone() => new Circle(Radius, ColorCurrent, Position);

        public override string ToString()
        {
            return "1-" + Radius + "-" + ColorDefault.R + "," + ColorDefault.G + "," + ColorDefault.B +
                   "-" + Position.X + "," + Position.Y;
        }
        
        protected sealed override void InitializeVertices()
        {
            int degrees = 0;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2f offset = new Vector2f(Radius, Radius);
                offset.X *= Convert.ToSingle(Math.Cos(degrees * Math.PI / 180));
                offset.Y *= Convert.ToSingle(Math.Sin(degrees * Math.PI / 180));
                Vertices[i] = new Vertex(Position + offset, ColorCurrent);
                degrees += 360 / Vertices.Length;
            }
        }
    }
}