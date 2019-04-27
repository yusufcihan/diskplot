using System;
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
using System.Windows.Shapes;

namespace diskplot
{
    /// <summary>
    /// intro.xaml etkileşim mantığı
    /// </summary>
    public partial class intro : Window
    {
        public intro()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            plots.Content = Properties.strings.plots;
            organise1.Content = Properties.strings.welcome;
            organise2.Text = Properties.strings.welcome1;
            organise3.Text = Properties.strings.welcome2;
            disk1.Content = Properties.strings.welcome3;
            disk2.Text = Properties.strings.welcome4;
            close.Content = Properties.strings.close;
            yourphotos.Content = Properties.strings.yourphotos;
            yourdocuments.Content = Properties.strings.yourdocuments;
            homeworks.Content = Properties.strings.documents;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
