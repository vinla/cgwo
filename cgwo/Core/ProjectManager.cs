using System;
using System.IO;
using Newtonsoft.Json;

namespace cgwo.Core
{
	public class ProjectManager
	{
		public static readonly ProjectManager Instance = new ProjectManager();

		private Project _loadedProject;
		public void LoadProject(string path)
		{
			var json = File.ReadAllText(path);
			_loadedProject = JsonConvert.DeserializeObject<Project>(json);
			OnProjectLoaded();
		}		

		public void LoadProject(Project project)
		{
			_loadedProject = project;
			OnProjectLoaded();
		}

		public void SaveProject(string projectFolder)
		{
			if (_loadedProject == null)
				throw new InvalidOperationException("No project has been loaded");

			var filename = Path.Combine(projectFolder, $"{_loadedProject.Name}.dcproj");
			File.WriteAllText(filename, JsonConvert.SerializeObject(_loadedProject));
		}

		public event EventHandler ProjectLoaded;

		protected virtual void OnProjectLoaded()
		{
			ProjectLoaded?.Invoke(this, EventArgs.Empty);
		}

		public Project LoadedProject => _loadedProject;
	}
}
