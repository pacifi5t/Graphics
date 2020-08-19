using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Graphics
{
    public class Aggregate : Drawable, IShape
    {
        public bool Activated { get; private set; }
        private List<IShape> FigureList { get; set; }

        public Aggregate()
        {
            Activated = false;
            FigureList = new List<IShape>();
        }

        public void AddFigure(IShape figure)
        {
            FigureList.Add(figure);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (IShape figure in FigureList)
            {
                if (figure is Aggregate aggregate)
                {
                    aggregate.Draw(target, states);
                }
                else
                {
                    target.Draw((Figure) figure);
                }
            }
        }

        public IShape Clone()
        {
            //Aggregate clone = new Aggregate {FigureList = FigureList};
            //return clone;
            return new Aggregate { FigureList = FigureList };
        }

        public bool CrossedBorder()
        {
            foreach (IShape figure in FigureList)
            {
                if (figure.CrossedBorder())
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeVisibility()
        {
            foreach (IShape figure in FigureList)
            {
                figure.ChangeVisibility();
            }
        }

        public void Deform()
        {
            foreach (IShape figure in FigureList)
            {
                figure.Deform();
            }
        }

        public void ConvertFromString(string str)
        {
            int size = Convert.ToInt32(str[2]) - 48;
            string block = str.Substring(4);
            block = block.Substring(0, block.Length - 1);
            
            for (int i = 0; i < size; i++)
            {
                IShape figure;
                int blockSize;
                
                if (block[0] == '0')
                {
                    blockSize = block.IndexOf(';') + 1;
                    figure = new Aggregate();
                }
                else
                {
                    blockSize = block.IndexOf('/') + 1;
                    switch (block[0])
                    {
                        case '1':
                            figure = new Circle();
                            break;
                        case '2':
                            figure = new Ellipse();
                            break;
                        case '3':
                            figure = new Triangle();
                            break;
                        case '4':
                            figure = new Rectangle();
                            break;
                        case '5':
                            figure = new Star();
                            break;
                        default:
                            figure = new Hexagon();
                            break;
                    }
                }

                figure.ConvertFromString(block.Substring(0, blockSize).Trim('/'));
                block = block.Substring(blockSize);
                AddFigure(figure);
            }
        }

        public bool Hover(Vector2i mousePosition)
        {
            foreach (IShape figure in FigureList)
            {
                if (figure.Hover(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Intersects(IShape other)
        {
            foreach (IShape figure in FigureList)
            {
                if (figure.Intersects(other))
                {
                    return true;
                }
            }
            return false;
        }

        public void Move(Vector2f offset)
        {
            foreach (IShape figure in FigureList)
            {
                figure.Move(offset);
            }
        }

        public void MoveRound(int degrees, Vector2f direction)
        {
            foreach (IShape figure in FigureList)
            {
                figure.MoveRound(degrees, direction);
            }
        }

        public void Activate()
        {
            Activated = true;
            foreach (IShape figure in FigureList)
            {
                figure.Activate();
            }
        }

        public void ChangeColorToDefault()
        {
            foreach (IShape figure in FigureList)
            {
                figure.ChangeColorToDefault();
            }
        }

        public void ChangeColorToReversed()
        {
            foreach (IShape figure in FigureList)
            {
                figure.ChangeColorToReversed();
            }
        }
        
        public override string ToString()
        {
            string output = "0-" + FigureList.Count + ":";
            foreach (IShape figure in FigureList)
            {
                output += figure.ToString();
                if (!(figure is Aggregate))
                {
                    output += "/";
                }
            }
            output += ";";
            return output;
        }
    }
}