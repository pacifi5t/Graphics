using System;
using System.Collections.Generic;

namespace Graphics
{
    public class Originator //Contains ever-changing info about all figures in string.
    {                       //This info can be exported as Checkpoint or a list of figures
        private string State { get; set; }
        
        public Originator()
        {
            State = String.Empty;
        }
        
        public Originator(Checkpoint checkpoint)
        {
            State = checkpoint.State;
        }

        public List<IShape> Export()
        {
            List<IShape> exportable = new List<IShape>();
            string[] figures = State.Split('\n');
            foreach (string str in figures)
            {
                IShape figure;
                switch (str[0])
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
                    case '6':
                        figure = new Hexagon();
                        break;
                    default:
                        figure = new Aggregate();
                        break;
                }

                figure.ConvertFromString(str);
                exportable.Add(figure);
            }
            return exportable;
        }
        
        public void Load(Checkpoint checkpoint)
        {
            State = checkpoint.State;
        }
        
        public Checkpoint Save() => new Checkpoint(State);
    }
}