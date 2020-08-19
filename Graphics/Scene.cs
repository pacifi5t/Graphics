using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Graphics
{
    public class Scene
    {
        public static RenderWindow Window { get; private set; }

        public static Scene Instance
        {
            get { return instance ??= new Scene(); }
        }

        public void LockWindow()
        {
            Window.Position = new Vector2i();
            Window.Size = new Vector2u(1600, 900);
        }
        
        private Scene()
        {
            Window = new RenderWindow(new VideoMode(1600, 900), "C# Graphics") {Position = new Vector2i()};
            Window.SetFramerateLimit(150);
        }
        
        private static Scene instance;
    }
}