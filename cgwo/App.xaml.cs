using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace cgwo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
			AutoMapper.Mapper.Initialize(cfg =>
			{
				cfg.AddProfile<Configuration.AutoMapperClientProfile>();
				cfg.AddProfile<Cogs.Data.LiteDb.AutoMapperLiteDbProfile>();
			});
			base.OnStartup(e);
        }
    }
}
