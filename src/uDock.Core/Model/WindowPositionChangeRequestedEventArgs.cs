using System;

namespace uDock.Core.Model
{
    public class WindowPositionChangeRequestedEventArgs : EventArgs
    {
        public double? NewTop { get; set; }

        public double? NewLeft { get; set; }
    }
}
