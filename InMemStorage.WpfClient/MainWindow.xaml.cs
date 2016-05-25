using System;
using System.Configuration;
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
using InMemStorage.ClientLib;

namespace InMemStorage.WpfClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		InMemStorageClient storageClient = new InMemStorageClient(ConfigurationManager.AppSettings["inMemStorageBaseUrl"]);

		public MainWindow()
		{
			InitializeComponent();

			RefreshKeysList();
		}

		private void RefreshKeysList()
		{
			storageClient.GetKeys().ContinueWith(t =>
			{
				KeysListBox.Dispatcher.BeginInvoke((Action)(() =>
				{
					KeysListBox.Items.Clear();

					foreach (var key in t.Result)
					{
						KeysListBox.Items.Add(key);
					}
				}));
			});
		}

		private void KeysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (KeysListBox.SelectedItem == null)
			{
				return;
			}

			var key = KeysListBox.SelectedItem as string;
			
			KeyTextBox.Text = key;

			storageClient.Get(key).ContinueWith(t =>
			{
				ValueTextBox.Dispatcher.BeginInvoke((Action)(() =>
				{
					ValueTextBox.Text = t.Result;
				}));
			});
		}

		private async void SetButton_Click(object sender, RoutedEventArgs e)
		{
			SetButton.IsEnabled = false;

			var key = KeyTextBox.Text;
			var value = ValueTextBox.Text;

			if (!string.IsNullOrWhiteSpace(key))
			{
				await storageClient.Set(key, value);
				
				RefreshKeysList();
			}

			SetButton.IsEnabled = true;
		}

		private async void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			DeleteButton.IsEnabled = false;

			var key = KeysListBox.SelectedItem as string;
			
			if (!string.IsNullOrWhiteSpace(key))
			{
				await storageClient.Remove(key);
				
				KeyTextBox.Text = "";
				ValueTextBox.Text = "";

				RefreshKeysList();
			}

			DeleteButton.IsEnabled = true;
		}
	}
}
