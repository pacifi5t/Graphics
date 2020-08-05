using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public abstract class RadiusBasedFigure : Figure //Represents shapes that use radius to define size
    {
        public float Radius { get; protected set; }
        
        public override void ConvertFromString(string data)
        {
            data = data.Substring(2);
            string[] fields = data.Split('-', ',');
            Radius = Convert.ToSingle(fields[0]);
            Width = Radius * 2;
            Height = Radius * 2;
            ColorDefault.R = Convert.ToByte(fields[1]);
            ColorDefault.G = Convert.ToByte(fields[2]);
            ColorDefault.B = Convert.ToByte(fields[3]);
            ColorCurrent = ColorDefault;
            ColorReversed = Color.White - ColorDefault;
            ColorReversed.A = 255;
            Vector2f position;
            position.X = Convert.ToSingle(fields[4]);
            position.Y = Convert.ToSingle(fields[5]);
            Position = position;
            InitializeVertices();
        }

        protected RadiusBasedFigure()
        {
            Radius = 0;
        }
        
        protected RadiusBasedFigure(float radius, Color color, Vector2f position) : base(radius * 2, radius * 2,
            color, position)
        {
            Radius = radius;
        }
    }
}