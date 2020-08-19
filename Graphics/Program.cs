using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;
using static SFML.Window.Mouse;

namespace Graphics
{
    static class Program
    {
        private const string FilePath = "LastCheckpoint.txt";
        private static bool trailEnabled;
        private static int degrees; //Needed for MoveRound method
        private static Scene scene;
        private static List<IShape> shapes;
        private static Originator originator;
        private static Caretaker caretaker;

        static void Main()
        {
            DisplayStartingMessage();
            InitializeFields();

            if (!LoadFromFile()) //Attempt to load from file. If failed, generating new figures
            {
                shapes = GenerateRandomShapes();
                originator = new Originator(new Checkpoint(ConvertShapesToString()));
                caretaker = new Caretaker(originator);
                CreateCheckpoint();
            }

            Scene.Window.KeyPressed += WindowOnKeyPressed; //Event subscriptions
            Scene.Window.MouseButtonPressed += WindowOnMouseButtonPressed;
            Scene.Window.Closed += WindowOnClosed;

            while (Scene.Window.IsOpen) //Mainloop
            {
                Scene.Window.DispatchEvents();
                Vector2f offset = new Vector2f(0, 0);

                if (IsKeyPressed(Key.Up)) //Located here because these buttons can be held for a long time
                {
                    offset += new Vector2f(0, -1);
                }

                if (IsKeyPressed(Key.Down))
                {
                    offset += new Vector2f(0, 1);
                }

                if (IsKeyPressed(Key.Left))
                {
                    offset += new Vector2f(-1, 0);
                }

                if (IsKeyPressed(Key.Right))
                {
                    offset += new Vector2f(1, 0);
                }

                if (offset != new Vector2f(0, 0))
                {
                    MoveShapes(offset);
                }

                if (IsKeyPressed(Key.R))
                {
                    foreach (IShape shape in shapes.Where(shape => shape.Activated))
                        shape.MoveRound(degrees, new Vector2f(1, 1));
                    degrees++;
                }

                CheckCollisions(); //Also switches colors
                scene.LockWindow(); //Makes window impossible to remove or resize
                
                if (!trailEnabled) //Clear window before every frame?
                {
                    Scene.Window.Clear();
                }

                for (int i = shapes.Count - 1; i >= 0; i--) //Display every figure in reversed order
                {
                    Scene.Window.Draw(shapes[i] as Drawable);
                }

                originator.Load(new Checkpoint(ConvertShapesToString())); //Change Originator state to relevant
                Scene.Window.Display();
            }

            Scene.Window.KeyPressed -= WindowOnKeyPressed; //Event unsubscriptions
            Scene.Window.MouseButtonPressed -= WindowOnMouseButtonPressed;
            Scene.Window.Closed -= WindowOnClosed;
        }

        static void BackupToFile() //Save last checkpoint to file
        {
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(caretaker.GetLastCheckpoint().ToString());
                fileStream.Write(array, 0, array.Length);
            }

            Console.WriteLine("Last checkpoint was saved to file");
        }

        static void ChangeVisibility()
        {
            foreach (IShape shape in shapes)
            {
                if (shape.Activated)
                {
                    shape.ChangeVisibility();
                }
            }

            Console.WriteLine("Visibility changed");
        }

        static void CheckCollisions()
        {
            foreach (IShape one in shapes)
            {
                foreach (IShape another in shapes.Where(another => one != another))
                {
                    one.ChangeColorToDefault();
                    if (one.Intersects(another))
                    {
                        one.ChangeColorToReversed();
                        break;
                    }
                }
            }
        }

        static string ConvertShapesToString()
        {
            string output = "";
            foreach (IShape shape in shapes)
            {
                output += shape + "\n";
            }

            return output.Trim('\n');
        }

        static void CreateAggregate() //Combine shapes (their clones actually) to one 
        {
            List<IShape> removableList = new List<IShape>();
            Aggregate aggregate = new Aggregate();

            foreach (IShape shape in shapes)
            {
                if (shape.Activated)
                {
                    aggregate.AddFigure(shape.Clone());
                    removableList.Add(shape);
                }
            }

            foreach (IShape removable in removableList)
            {
                shapes.Remove(removable);
            }

            shapes.Add(aggregate);
            Console.WriteLine("Aggregate was created from selected shapes");
        }

        static void CreateCheckpoint() //Save state of all figures to one object
        {
            originator.Load(new Checkpoint(ConvertShapesToString()));
            caretaker.CreateCheckpoint();
            Console.WriteLine("Checkpoint created");
        }

        static void Deform()
        {
            foreach (IShape shape in shapes)
            {
                if (shape.Activated)
                {
                    shape.Deform();
                }
            }

            Console.WriteLine("Deformation applied");
        }

        static void DeselectShape(Vector2i mousePosition) //Or remove from list of shapes
        {
            IShape temp = null;
            foreach (IShape shape in shapes)
                if (shape.Hover(mousePosition))
                {
                    temp = shape;
                    break;
                }

            if (temp != null)
            {
                shapes.Remove(temp);
            }

            Console.WriteLine("Shape removed");
        }

        static void DisplayStartingMessage()
        {
            Console.Write(
                "----- Controls -----\n\nLMB - select shape\nRMB - deselect shape\nS - create checkpoint\n" +
                "Z - step back\nY - step forward\nB - save last checkpoint to file\nL - load data from file\n" +
                "T - turn on/off movement trail\n\n\n" +
                "When shape(s) is/are selected:\n\nArrow keys - move shape(s)\nR - move round\n" +
                "V - turn visible/invisible\nD - deform\nA - create aggregate\n\n" +
                "Press enter to continue...");
            Console.ReadLine();
            Console.WriteLine("\n\nLogs\n");
        }

        static List<IShape> GenerateRandomShapes()
        {
            List<IShape> list = new List<IShape>();
            Random random = new Random();

            for (int i = 0; i <= random.Next(3, 6); i++)
            {
                IShape shape;
                int type = random.Next(0, 6);
                switch (type)
                {
                    case 1:
                        shape = new Circle();
                        break;
                    case 2:
                        shape = new Ellipse();
                        break;
                    case 3:
                        shape = new Triangle();
                        break;
                    case 4:
                        shape = new Rectangle();
                        break;
                    case 5:
                        shape = new Star();
                        break;
                    case 6:
                        shape = new Hexagon();
                        break;
                    default:
                        return GenerateRandomShapes(); //In case when smth went wrong (for aggregate actually)
                }

                int radius = 0;
                switch (type)
                {
                    case 1:
                    case 5:
                    {
                        radius = random.Next(50, 150);
                        break;
                    }
                }

                int x = random.Next(100, 300);
                int y = random.Next(100, 300);
                Vector2f pos = new Vector2f(random.Next(151, 1449), random.Next(151, 749));
                byte cr = (byte) random.Next(50, 240);
                byte cg = (byte) random.Next(50, 240);
                byte cb = (byte) random.Next(50, 240);
                string str = "-" + cr + "," + cg + "," + cb + "-" + pos.X + "," + pos.Y;

                if (radius == 0)
                {
                    shape.ConvertFromString(type + "-" + x + "," + y + str); //Simple figure
                }
                else
                {
                    shape.ConvertFromString(type + "-" + radius + str); //RadiusBased figure
                }

                list.Add(shape);
            }

            Console.WriteLine("Random shapes generated");
            return list;
        }

        static void InitializeFields()
        {
            shapes = new List<IShape>();
            originator = new Originator();
            trailEnabled = false;
            degrees = 0;
            scene = Scene.Instance;
        }

        static bool LoadFromFile() //Recreates figures from file (last checkpoint from previous usage)
        {
            try
            {
                string[] strings = File.ReadAllLines(FilePath);
                string data = "";
                foreach (string t in strings)
                {
                    data += t + "\n";
                }

                originator.Load(new Checkpoint(data.Trim('\n')));
                caretaker = new Caretaker(originator);
                caretaker.CreateCheckpoint();
                shapes = originator.Export();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return false;
            }

            Console.WriteLine("Shapes loaded from file");
            return true;
        }

        static void MoveShapes(Vector2f offset)
        {
            foreach (IShape shape in shapes)
            {
                if (shape.Activated)
                {
                    shape.Move(offset);
                }

                if (shape.CrossedBorder()) //Shapes can't leave the window
                {
                    shape.Move(-offset);
                }
            }
        }

        static void SelectShape(Vector2i mousePosition)
        {
            foreach (IShape shape in shapes)
            {
                if (shape.Hover(mousePosition))
                {
                    shape.Activate();
                    break;
                }
            }
        }

        private static void StepForward()
        {
            caretaker.StepForward();
            shapes = originator.Export();
            Console.WriteLine("Redo");
        }

        private static void StepBack()
        {
            caretaker.StepBack();
            shapes = originator.Export();
            Console.WriteLine("Undo");
        }
        
        private static void ToggleTrail()
        {
            trailEnabled = !trailEnabled;
            Console.WriteLine("Trail toggled");
        }
        
        static void WindowOnClosed(object sender, EventArgs e)
        {
            Scene.Window.Close();
        }
        
        static void WindowOnKeyPressed(object obj, KeyEventArgs args)
        {
            switch (args.Code)
            {
                case Key.S:
                    CreateCheckpoint();
                    break;
                case Key.Z:
                    StepBack();
                    break;
                case Key.B:
                    BackupToFile();
                    break;
                case Key.L:
                    LoadFromFile();
                    break;
                case Key.Y:
                    StepForward();
                    break;
                case Key.T:
                    ToggleTrail();
                    break;
                case Key.V:
                    ChangeVisibility();
                    break;
                case Key.D:
                    Deform();
                    break;
                case Key.A:
                    CreateAggregate();
                    break;
            }
        }
        
        static void WindowOnMouseButtonPressed(object obj, MouseButtonEventArgs args)
        {
            switch (args.Button)
            {
                case Button.Left:
                    SelectShape(new Vector2i(args.X, args.Y));
                    break;
                case Button.Right:
                    DeselectShape(new Vector2i(args.X, args.Y));
                    break;
            }
        }
    }
}