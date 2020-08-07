using System.Collections.Generic;

namespace Graphics
{
    public class Caretaker //Contains all checkpoints and can make undo's and redo's
    {
        public Caretaker(Originator originator)
        {
            _originator = originator;
        }

        public void CreateCheckpoint()
        {
            if (_checkpointsBack.Count == 0)
            {
                _checkpointsBack.Push(_originator.Save());
            }
            else
            {
                Checkpoint temp = _originator.Save();
                Checkpoint last = GetLastCheckpoint();
                if (!Equals(temp, last))
                {
                    _checkpointsBack.Push(temp);
                }

                _checkpointsForward.Clear();
            }
        }
        
        public void StepBack()
        {
            if (_checkpointsBack.Count != 0)
            {
                Checkpoint temp = _checkpointsBack.Pop();
                _checkpointsForward.Push(temp);
                if (Equals(temp, _originator.Save()))
                {
                    StepBack();
                }
                else
                {
                    _originator.Load(temp);
                }
            }
        }

        public void StepForward()
        {
            if (_checkpointsForward.Count != 0)
            {
                Checkpoint temp = _checkpointsForward.Pop();
                _checkpointsBack.Push(temp);
                if (Equals(temp, _originator.Save()))
                {
                    StepForward();
                }
                else
                {
                    _originator.Load(temp);
                }
            }
        }

        public Checkpoint GetLastCheckpoint()
        {
            Checkpoint last = _checkpointsBack.Pop();
            _checkpointsBack.Push(last);
            return last;
        }

        private Stack<Checkpoint> _checkpointsBack = new Stack<Checkpoint>(); //Checkpoints for undo operation
        private Stack<Checkpoint> _checkpointsForward = new Stack<Checkpoint>(); //for redo
        private Originator _originator;
    }
}