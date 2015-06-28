using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ServiceNode;

namespace LoggerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Schema_OnClick(object sender, RoutedEventArgs e)
        {
           string[] schemaStrings = await LogLib.EnumerateParams(@"http://servicenode.azurewebsites.net/schema");
            MessageBox.Show(string.Join(",",schemaStrings));
        }

        private async void Log_OnClick(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>()
            {
                {"Path",Environment.CurrentDirectory},
                {"User",Environment.UserName},
                {"Cores",Environment.ProcessorCount.ToString()},
                {"OSver",Environment.OSVersion.VersionString},
                {"WorkingSet",Environment.WorkingSet.ToString()},
            };
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {"Type","Debug"},
                {"Data",Uri.EscapeUriString(DictionnaryToUrl(dataDictionary))},
                {"TimeStamp",DateTime.Now.ToString("O")},
                {"MachineId",Environment.MachineName}
            };
            if (await LogLib.CreateLog(@"http://servicenode.azurewebsites.net/addlogx", dictionary))
                MessageBox.Show("Done");
            else
                MessageBox.Show("Error");
        }

        string DictionnaryToUrl(Dictionary<string, string> dictionary)
        {
            return string.Join("&", dictionary.Select(item => item.Key + "=" + item.Value));
        }
    }
}
