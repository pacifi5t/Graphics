using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Triangle : Figure
    {
        public Triangle()
        {
            Vertices = new Vertex[3];
        }

        private Triangle(float width, float height, Color color, Vector2f position) : base(width, height, color,
            position)
        {
            Vertices = new Vertex[3];
            InitializeVertices();
        }
        
        public override IShape Clone() => new Triangle(Width, Height, ColorCurrent, Position);

        public override string ToString()
        {
            return "3-" + Width + "," + Height + "-" + ColorDefault.R + "," + ColorDefault.G + "," + ColorDefault.B +
                   "-" + Position.X + "," + Position.Y;
        }

        protected sealed override void InitializeVertices()
        {
            Vertices[0] = new Vertex(new Vector2f(Position.X, Position.Y - Height / 2), ColorCurrent);
            Vertices[1] = new Vertex(new Vector2f(Position.X + Width / 2, Position.Y + Height / 2), ColorCurrent);
            Vertices[2] = new Vertex(new Vector2f(Position.X - Width / 2, Position.Y + Height / 2), ColorCurrent);
        }
    }
}