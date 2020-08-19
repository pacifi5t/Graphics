using System.Collections.Generic;

namespace Graphics
{
    public class Caretaker //Contains all checkpoints and can make undo's and redo's
    {
        public Caretaker(Originator originator)
        {
            this.originator = originator;
        }

        public void CreateCheckpoint()
        {
            if (checkpointsBack.Count == 0)
            {
                checkpointsBack.Push(originator.Save());
            }
            else
            {
                Checkpoint temp = originator.Save();
                Checkpoint last = GetLastCheckpoint();
                if (!Equals(temp, last))
                {
                    checkpointsBack.Push(temp);
                }

                checkpointsForward.Clear();
            }
        }
        
        public void StepBack()
        {
            if (checkpointsBack.Count != 0)
            {
                Checkpoint temp = checkpointsBack.Pop();
                checkpointsForward.Push(temp);
                if (Equals(temp, originator.Save()))
                {
                    StepBack();
                }
                else
                {
                    originator.Load(temp);
                }
            }
        }

        public void StepForward()
        {
            if (checkpointsForward.Count != 0)
            {
                Checkpoint temp = checkpointsForward.Pop();
                checkpointsBack.Push(temp);
                if (Equals(temp, originator.Save()))
                {
                    StepForward();
                }
                else
                {
                    originator.Load(temp);
                }
            }
        }

        public Checkpoint GetLastCheckpoint()
        {
            Checkpoint last = checkpointsBack.Pop();
            checkpointsBack.Push(last);
            return last;
        }

        private readonly Stack<Checkpoint> checkpointsBack = new Stack<Checkpoint>(); //Checkpoints for undo operation
        private readonly Stack<Checkpoint> checkpointsForward = new Stack<Checkpoint>(); //for redo
        private readonly Originator originator;
    }
}