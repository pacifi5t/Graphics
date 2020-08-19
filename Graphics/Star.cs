using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Star : RadiusBasedFigure
    {
	    public Star()
	    {
		    Vertices = new Vertex[10];
	    }

	    private Star(float radius, Color color, Vector2f position) : base(radius, color, position)
        {
            Vertices = new Vertex[10];
            InitializeVertices();
        }

        public override string ToString()
        {
            return "5-" + Radius + "-" + ColorDefault.R + "," + ColorDefault.G + "," + ColorDefault.B +
                   "-" + Position.X + "," + Position.Y;
        }
        
        public override IShape Clone() => new Star(Radius, ColorCurrent, Position);
        
        protected sealed override void InitializeVertices()
        {
	        int degrees = 90;
	        for (int i = 0; i < Vertices.Length; i += 2)
	        {
		        Vector2f offset = new Vector2f(Radius, Radius);
		        offset.X *= Convert.ToSingle(Math.Cos(degrees * Math.PI / 180));
		        offset.Y *= Convert.ToSingle(Math.Sin(degrees * Math.PI / 180));
		        Vertices[i] = new Vertex(Position + new Vector2f(offset.X / 2, offset.Y / 2), ColorCurrent);
		        degrees -= 72;
	        }
	        degrees = 54;
	        for (int i = 1; i < Vertices.Length; i += 2)
	        {
		        Vector2f offset = new Vector2f(Radius, Radius);
		        offset.X *= Convert.ToSingle(Math.Cos(degrees * Math.PI / 180));
		        offset.Y *= Convert.ToSingle(Math.Sin(degrees * Math.PI / 180));
		        Vertices[i] = new Vertex(Position + offset, ColorCurrent);
		        degrees -= 72;
	        }
        }
    }
}