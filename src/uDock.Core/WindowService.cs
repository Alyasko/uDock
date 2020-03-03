using System;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using uDock.Core.Model;

namespace uDock.Core
{
    public class WindowService
    {
        private readonly ApplicationState _applicationState;

        private const string AppConfigFilename = "config.json";

        public SizeF ScreenSize { get; set; }
        public SizeF WindowSize { get; set; }
        public PointF WindowLocation { get; set; }
        public PointF RestoreLocation { get; private set; }

        public bool IsWindowShown { get; private set; } = true;

        public WindowService(ApplicationState applicationState)
        {
            _applicationState = applicationState;
        }

        public event EventHandler<WindowPositionChangeRequestedEventArgs> WindowPositionChangeRequested;

        public void UpdateWindowLocation(double left, double top)
        {

        }

        public void Show()
        {
            if(IsWindowShown)
                return;

            IsWindowShown = true;

            var handler = WindowPositionChangeRequested;
            handler?.Invoke(this, new WindowPositionChangeRequestedEventArgs()
            {
                NewTop = RestoreLocation.Y
            });
        }

        public void Toggle()
        {
            if (IsWindowShown)
                Hide();
            else
                Show();
        }

        public void Hide()
        {
            if(!IsWindowShown)
                return;

            RestoreLocation = WindowLocation;

            IsWindowShown = false;

            var handler = WindowPositionChangeRequested;
            handler?.Invoke(this, new WindowPositionChangeRequestedEventArgs()
            {
                NewTop = ScreenSize.Height + WindowSize.Height + 100
            });
        }

        public void ResetPosition()
        {
            var handler = WindowPositionChangeRequested;

            WindowLocation = new PointF((ScreenSize.Width - WindowSize.Width) / 2, (ScreenSize.Height - WindowSize.Height) / 2);

            handler?.Invoke(this, new WindowPositionChangeRequestedEventArgs()
            {
                NewTop = WindowLocation.Y,
                NewLeft = WindowLocation.X,
            });
        }

        public void SaveLocation(double top, double left)
        {
            var dir = _applicationState.DataDirectory;
            var configPath = Path.Combine(dir.FullName, AppConfigFilename);

            var config = new AppConfig()
            {
                WindowLocation = new WindowLocation(top, left)
            };

            var json = JsonConvert.SerializeObject(config, Formatting.Indented);

            File.WriteAllText(configPath, json);
        }

        public WindowLocation LoadLocation()
        {
            var dir = _applicationState.DataDirectory;
            var configPath = Path.Combine(dir.FullName, AppConfigFilename);

            if (!File.Exists(configPath))
                return null;

            var json = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<AppConfig>(json);

            return config.WindowLocation;
        }
    }
}
