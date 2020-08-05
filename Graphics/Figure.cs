using System;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public abstract class Figure : Drawable, IShape //Represents any shape that has finite set of vertices
    {
        public bool Activated { get; protected set; }
        public Vector2f Position { get; protected set; }
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public Color ColorCurrent { get; protected set; }
        
        protected Figure()
        {
            Activated = false;
            Position = new Vector2f();
            Width = 0;
            Height = 0;
            ColorDefault = Color.White;
            ColorReversed = Color.Black;
            ColorCurrent = ColorDefault;
        }
        
        protected Figure(float width, float height, Color color, Vector2f position)
        {
            Activated = false;
            Position = position;
            Width = width;
            Height = height;
            ColorCurrent = color;
            ColorDefault = color;
            ColorReversed = Color.White - ColorDefault;
            ColorReversed.A = 255;
        }
        
        public abstract IShape Clone();
        protected abstract void InitializeVertices();

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Vertices, PrimitiveType.TriangleFan);
        }
        
        public bool CrossedBorder()
        {
            Vector2u windowSize = Scene.Window.Size;
            return Position.X - Width / 2 < 0 || Position.X + Width / 2 > windowSize.X ||
                   Position.Y - Height / 2 < 0 || Position.Y + Height / 2 > windowSize.Y;
        }

        public void ChangeVisibility()
        {
            ColorDefault.A = (byte) (255 - ColorDefault.A);
            ColorReversed.A = (byte) (255 - ColorReversed.A);
        }
        
        public void Deform()
        {
            Random random = new Random();
            for (uint i = 0; i < Vertices.Length; i++)
            {
                int vertexID = random.Next(0, Vertices.Length - 1);
                int force = random.Next(-30, 30);
                if (i % 2 == 0)
                {
                    Vertices[vertexID].Position.X += force;
                }
                else
                {
                    Vertices[vertexID].Position.Y += force;
                }
            }
        }
        
        public virtual void ConvertFromString(string data)
        {
            data = data.Substring(2);
            string[] fields = data.Split('-', ',');
            Width = Convert.ToSingle(fields[0]);
            Height = Convert.ToSingle(fields[1]);
            ColorDefault.R = Convert.ToByte(fields[2]);
            ColorDefault.G = Convert.ToByte(fields[3]);
            ColorDefault.B = Convert.ToByte(fields[4]);
            ColorCurrent = ColorDefault;
            ColorReversed = Color.White - ColorDefault;
            ColorReversed.A = 255;
            Vector2f position;
            position.X = Convert.ToSingle(fields[5]);
            position.Y = Convert.ToSingle(fields[6]);
            Position = position;
            InitializeVertices();
        }

        public bool Hover(Vector2i mousePosition)
        {
            return mousePosition.X < Position.X + Width / 2 && mousePosition.X > Position.X - Width / 2 &&
                   mousePosition.Y < Position.Y + Height / 2 && mousePosition.Y > Position.Y - Height / 2;
        }

        public bool Intersects(IShape other) //AABB collision detection
        {
            if (other == null)
            {
                throw new NullReferenceException();
            }

            if (other is Aggregate)
            {
                return other.Intersects(this);
            }
            
            Figure figure = (Figure) other;
            Vector2f delta = new Vector2f(Position.X - figure.Position.X, Position.Y - figure.Position.Y);
            Vector2f distance = new Vector2f(Math.Abs(delta.X) - (figure.Width / 2 + Width / 2),
                Math.Abs(delta.Y) - (figure.Height / 2 + Height / 2));
            return distance.X < 0 && distance.Y < 0;
        }

        public void Move(Vector2f offset)
        {
            Position += offset;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Position += offset;
            }
        }

        public void MoveRound(int degrees, Vector2f direction)
        {
            Vector2f offset = new Vector2f(Convert.ToSingle(direction.X * Math.Cos(degrees * Math.PI / 180)),
                Convert.ToSingle(direction.Y * Math.Sin(degrees * Math.PI / 180)));
            Move(offset);
            if (CrossedBorder())
            {
                Move(-offset);
            }
        }

        public void Activate()
        {
            Activated = true;
        }

        public void ChangeColorToDefault()
        {
            ColorCurrent = ColorDefault;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Color = ColorDefault;
            }
        }

        public void ChangeColorToReversed()
        {
            ColorCurrent = ColorReversed;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Color = ColorReversed;
            }
        }

        protected Color ColorDefault;
        protected Color ColorReversed;
        protected Vertex[] Vertices;
    }
}