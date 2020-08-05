using SFML.System;

namespace Graphics
{
    public interface IShape //Contains methods for any figure and aggregate
    {
        public bool Activated { get;} //Defines if shape is operable
        public void Activate(); //Make shape operable
        public IShape Clone();
        public bool CrossedBorder();
        public void ChangeVisibility(); //Visible - invisible
        public void Deform(); //Move randomly some vertices
        public void ConvertFromString(string data); //Recreates object from string
        public bool Hover(Vector2i mousePosition); //Mouse intersects shape
        public bool Intersects(IShape other);
        public void Move(Vector2f offset);
        public void MoveRound(int degrees, Vector2f direction); //Move using round trajectory 
        public void ChangeColorToDefault();
        public void ChangeColorToReversed();
    }
}