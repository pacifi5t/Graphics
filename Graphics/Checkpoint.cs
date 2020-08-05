namespace Graphics
{
    public class Checkpoint //Contains static info about all figures in string
    {
        public string State { get;}

        public Checkpoint(string state)
        {
            State = state;
        }

        public override bool Equals(object obj)
        {
            return ((Checkpoint) obj)?.State == State;
        }

        public override int GetHashCode()
        {
            return State != null ? State.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return State;
        }
    }
}