namespace Puya.Debugging
{
    public class NoDebugger : IDebugger
    {
        public bool IsDebugging
        {
            get { return false; }
            set { }
        }
    }
}
