using System;
using System.IO;
using Newtonsoft.Json;

namespace cgwo.Core
{
	public class ProjectManager
	{
		public static readonly ProjectManager Instance = new ProjectManager();

		private Project _loadedProject;
		private string _savePath;
		public void LoadProject(string path)
		{
			var json = File.ReadAllText(path);
			_loadedProject = JsonConvert.DeserializeObject<Project>(json);
			_savePath = path;
			OnProjectLoaded();
		}		

		public void LoadProject(Project project)
		{
			_loadedProject = project;
			OnProjectLoaded();
		}

		public void SaveProject(string projectFolder)
		{
			_savePath = Path.Combine(projectFolder, $"{_loadedProject.Name}.dcproj");
			SaveProject(); 			
		}

		public void SaveProject()
		{
			if (_loadedProject == null)
				throw new InvalidOperationException("No project has been loaded");
			if (String.IsNullOrEmpty(_savePath))
				throw new InvalidOperationException("No save location has been set");

			File.WriteAllText(_savePath, JsonConvert.SerializeObject(_loadedProject));
		}

		public event EventHandler ProjectLoaded;

		protected virtual void OnProjectLoaded()
		{
			ProjectLoaded?.Invoke(this, EventArgs.Empty);
		}

		public Project LoadedProject => _loadedProject;
	}
}
