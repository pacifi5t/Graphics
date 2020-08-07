using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public abstract class RadiusBasedFigure : Figure //Represents shapes that use radius to define size
    {
        public float Radius { get; protected set; }
        
        public override void ConvertFromString(string str)
        {
            str = str.Substring(2);
            string[] data = str.Split('-', ',');
            Radius = Convert.ToSingle(data[0]);
            Width = Height = Radius * 2;
            
            InitializeColorAndPosition(data);
            InitializeVertices();
        }

        protected RadiusBasedFigure()
        {
            Radius = 0;
        }
        
        protected RadiusBasedFigure(float radius, Color color, Vector2f position) : base(radius * 2,
            radius * 2, color, position)
        {
            Radius = radius;
        }
    }
}