using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for partplot.xaml
    /// </summary>
    public partial class partplot : Window
    {
        int index = 0;
        bool edit = false;
        public enum DialogType
        {
            Edit = 0,
            Create = 1
        }

        public partplot(DialogType type, int inde)
        {
            InitializeComponent();
            this.Title = Properties.strings.plot;
            save.Content = Properties.strings.save.ToUpper();
            directorylbl.Content = Properties.strings.directory;
            limitlbl.Content = Properties.strings.limit;
            namelbl.Content = Properties.strings.name;
            customlbl.Content = Properties.strings.customicon;
            limitinfo.Text = Properties.strings.limitinfo;
            if (type == DialogType.Create)
            {
                cancel.Content = Properties.strings.cancel.ToUpper();
                cancel.Foreground = Brushes.Gray;
            }
            else
            {
                index = inde;
                edit = true;
                id.Text = inde.ToString();
            }
            if (edit)
            {
                drivelbl.Text = Properties.Settings.Default.core2[index].ToString();
                icon.SelectedIndex = Properties.Settings.Default.iconlibrary.IndexOf(Properties.Settings.Default.core4[index]) + 1;
                size.Text = Properties.Settings.Default.core3[index].ToString();
                direcotrylbl.Text = Properties.Settings.Default.core[index].ToString().Split('\\')[Properties.Settings.Default.core[index].ToString().Split('\\').Length - 1];
                cancel.Content = Properties.strings.delete.ToUpper();
                cancel.Foreground = Brushes.Red;
            }
        }

        public void ShowError(string text)
        {
            MessageBox.Show(text, "DiskPlot", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (drivelbl.Text.Trim() == "")
                {
                    ShowError(Properties.strings.error_name);
                }
                else if (drivelbl.Text == "IconsLibrary")
                {
                    ShowError(Properties.strings.error_system);
                }
                else if (!edit && (Properties.Settings.Default.core.Contains(Properties.Settings.Default.mainfolder + @"\" + drivelbl.Text)))
                {
                    ShowError(Properties.strings.error_same);
                }
                else
                {

                    if (edit)
                    {
                        Properties.Settings.Default.index = index;
                    }
                    else
                    {
                        Properties.Settings.Default.iscreated = true;
                        Properties.Settings.Default.directory = Properties.Settings.Default.mainfolder + @"\" + drivelbl.Text;

                    }
                    Properties.Settings.Default.name = drivelbl.Text;
                    if (icon.SelectedItem == null)
                    {
                        Properties.Settings.Default.icon = "";
                    }
                    else
                    {
                        Properties.Settings.Default.icon = icon.SelectedItem.ToString();
                    }

                    if (size.Text == "")
                    {
                        Properties.Settings.Default.size = 0;
                    }
                    else if (sizetype.SelectedIndex == 1)
                    {
                        Properties.Settings.Default.size = (double)(Convert.ToDouble(size.Text) * 1024);
                    }
                    else
                    {
                        Properties.Settings.Default.size = Convert.ToDouble(size.Text);
                    }
                    this.Close();
                }
            }
            catch (Exception h)
            {
                MessageBox.Show(Properties.strings.error_unexpected + "\n\n" + h.Message, "DiskPlot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog addn = new System.Windows.Forms.OpenFileDialog();
            addn.CheckFileExists = true;
            addn.CheckPathExists = true;
            addn.FileName = "";
            addn.Filter = Properties.strings.icons + " (.ico / .png)|*.ico;*.png";
            addn.FilterIndex = 1;
            if (addn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!Directory.Exists(Properties.Settings.Default.mainfolder + @"\IconsLibrary"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.mainfolder + @"\IconsLibrary");
                }

                if (Properties.Settings.Default.iconlibrary.Contains(addn.FileName) || File.Exists(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + addn.SafeFileName))
                {
                    ShowError(Properties.strings.error_icon + "\n\n" + addn.SafeFileName);
                }
                else
                {
                    File.Copy(addn.FileName, Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + addn.SafeFileName);
                    Properties.Settings.Default.iconlibrary.Add(addn.SafeFileName);
                    Properties.Settings.Default.Save();
                }
                LoadLibrary(0);
            }
            
        }

        public void LoadLibrary(long index)
        {
            icon.Items.Clear();
            icon.Items.Add("");
            foreach (var item in Properties.Settings.Default.iconlibrary)
            {
                icon.Items.Add(item);
                imgicon.Source = null;
            }
            if (index != -1)
            {
                icon.SelectedIndex = icon.Items.Count - 1;
            }
            else
            {
                icon.SelectedIndex = 0;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            LoadLibrary(-1);
            
        }

        private void Icon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((icon.SelectedIndex == -1) || (icon.SelectedIndex == 0))
            {
                trash.Visibility = Visibility.Collapsed;
                imgicon.Source = null;
            }
            else
            {
                trash.Visibility = Visibility.Visible;
                if (File.Exists(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + icon.SelectedItem.ToString()))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + icon.SelectedItem.ToString());
                    image.EndInit();
                    imgicon.Source = image;
                }
                else
                {

                }
                
            }
        }
        private static readonly Regex _regex = new Regex("[^0-9]+");
        private void Trash_Click(object sender, RoutedEventArgs e)
        {
            if (icon.SelectedItem.ToString() == "ploticon.ico")
            {
                ShowError(Properties.strings.error_cantdelete);
            }
            else
            {
                imgicon.Source = null;
                Properties.Settings.Default.iconlibrary.Remove(icon.SelectedItem.ToString());
                Properties.Settings.Default.Save();
                File.Delete(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + icon.SelectedItem.ToString());
                icon.SelectedIndex = -1;
                trash.Visibility = Visibility.Collapsed;
                LoadLibrary(-1);
            }
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (cancel.Content.ToString() == Properties.strings.cancel.ToUpper())
            {
                this.Close();
            }
            else
            {
                if (MessageBox.Show(Properties.strings.delete_plot + " " + Properties.strings.ask_continue, Properties.strings.delete_plot_title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (Directory.Exists(Properties.Settings.Default.mainfolder + @"\" + direcotrylbl.Text))
                    {
                        Directory.Delete(Properties.Settings.Default.mainfolder + @"\" + direcotrylbl.Text);
                    }
                    Properties.Settings.Default.core.RemoveAt(index);
                    Properties.Settings.Default.core2.RemoveAt(index);
                    Properties.Settings.Default.core3.RemoveAt(index);
                    Properties.Settings.Default.core4.RemoveAt(index);
                    Properties.Settings.Default.iscreated = false;
                    Properties.Settings.Default.index = -1;
                    Properties.Settings.Default.Save();
                    this.Close();
                }
            }
            Properties.Settings.Default.index = -1;
            Properties.Settings.Default.Save();
        }

        private void Drivelbl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!edit)
            {
                direcotrylbl.Text = drivelbl.Text;
            }
        }

        private void Drivelbl_Copy_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private static bool IsTextAllowed(string text)
        {
                return !_regex.IsMatch(text);   
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (size.Text != "")
            {
                if ((sizetype.SelectedIndex == 1) && (Convert.ToDouble(size.Text) > 0))
                {
                    size.Text = ((double)(Convert.ToDouble(size.Text) / 1024)).ToString();
                }
                else if ((sizetype.SelectedIndex == 0) && (Convert.ToDouble(size.Text) > 0))
                {
                    size.Text = ((double)(Convert.ToDouble(size.Text) * 1024)).ToString();
                }
            }
            
        }

        private void Size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Size_TextChanged(object sender, TextChangedEventArgs e)
        {
            size.Text = size.Text.Trim();
        }

        private void Size_MouseEnter(object sender, MouseEventArgs e)
        {
            limitwindow.Visibility = Visibility.Visible;
        }

        private void Size_MouseLeave(object sender, MouseEventArgs e)
        {
            limitwindow.Visibility = Visibility.Collapsed;
        }
    }
}
