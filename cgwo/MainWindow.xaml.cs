﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using cgwo.Configuration;

namespace cgwo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
            var dialogService = new GorgleDevs.Wpf.Mvvm.WindowsDialogService(DialogHost);			
            DataContext = new ViewModels.MainViewModel(new Cogs.Data.LiteDb.LiteDbCardGameDataStoreFactory(), dialogService);
		}
	}
}
