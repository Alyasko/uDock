using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace uDock.Core.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Project
    {
        public static readonly Project Empty = new Project();

        [JsonProperty]
        public List<LinkItem> Items { get; set; } = new List<LinkItem>();

        public static Project Save(List<LinkItem> items, string path)
        {
            var project = new Project()
            {
                Items = items
            };

            var json = JsonConvert.SerializeObject(project, Formatting.Indented);
            File.WriteAllText(path, json);

            return project;
        }

        public static Project Load(string path)
        {
            var fi = new FileInfo(path);
            if (!fi.Exists)
                return null;

            var json = File.ReadAllText(fi.FullName);
            var project = JsonConvert.DeserializeObject<Project>(json);

            return project;
        }
    }
}
