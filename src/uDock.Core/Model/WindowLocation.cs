namespace uDock.Core.Model
{
    public class WindowLocation
    {
        public WindowLocation(double top, double left)
        {
            Top = top;
            Left = left;
        }
        
        public double Top { get; }

        public double Left { get; }
    }
}
