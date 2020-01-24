using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using uDock.Core.Model;

namespace uDock.Core
{
    public class ApplicationState
    {
        private static Guid _appGuid;

        public ApplicationState()
        {
            DataDirectory = GetDataDirectory();
        }

        private DirectoryInfo GetDataDirectory()
        {
            var asm = Assembly.GetEntryAssembly();
            if(asm == null)
                throw new NullReferenceException();

            var attr = (asm.GetCustomAttributes(typeof(GuidAttribute), true));

            var guidAttr = attr.FirstOrDefault() as GuidAttribute;
            
            _appGuid = new Guid(guidAttr.Value);

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                _appGuid.ToString());

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return new DirectoryInfo(path);
        }

        public Project Project { get; set; }

        public ObservableCollection<LinkItem> LinkItems { get; set; } = new ObservableCollection<LinkItem>();

        public DirectoryInfo DataDirectory { get; }
    }
}
