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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace diskplot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
        }
        int partnm = 1;
        bool isright = false;

        public enum PlotType
        {
            ViewScreen = 0,
            Create = 1
        }

        public void Plot(string name, string directory, string icon, double size, PlotType type)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;

                string capacity = Math.Round(ByteSizeLib.ByteSize.FromMegaBytes(size).LargestWholeNumberValue, 2) + " " + ByteSizeLib.ByteSize.FromMegaBytes(size).LargestWholeNumberSymbol;
                if (type == PlotType.Create)
                {
                    Directory.CreateDirectory(directory);
                    Properties.Settings.Default.core.Add(directory);
                    Properties.Settings.Default.core3.Add(size);
                    Properties.Settings.Default.core2.Add(name);
                    Properties.Settings.Default.core4.Add(icon);
                    Properties.Settings.Default.Save();

                }

                MaterialDesignThemes.Wpf.Card pr = new MaterialDesignThemes.Wpf.Card();

                System.Windows.Controls.Grid par = new Grid();
                System.Windows.Controls.Image img = new Image();
                System.Windows.Controls.Label lbl = new Label();
                System.Windows.Controls.Label full = new Label();
                System.Windows.Controls.Button btn = new Button();
                System.Windows.Controls.Button menu = new Button();
                full.HorizontalAlignment = HorizontalAlignment.Left;
                full.Margin = new Thickness(106, 65, 0, 0);
                full.VerticalAlignment = VerticalAlignment.Top;
                full.FontSize = 12;
                full.Name = "size" + partnm;
                full.Content = "0 / " + capacity;
                lbl.HorizontalAlignment = HorizontalAlignment.Left;
                lbl.Height = 56;
                lbl.Margin = new Thickness(105, 11, 0, 0);
                lbl.VerticalAlignment = VerticalAlignment.Top;
                lbl.FontSize = 17;
                lbl.Name = "lbl" + partnm;
                if (name == "")
                {
                    lbl.Content = Properties.strings.plot + " " + partnm;
                }
                else
                {
                    lbl.Content = name;
                }
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.Height = 56;
                img.Margin = new Thickness(26, 20, 0, 0);
                img.VerticalAlignment = VerticalAlignment.Top;
                img.Width = 57;
                if (icon == "")
                {
                    image.UriSource = new Uri("ploticon.ico", UriKind.Relative);
                    image.EndInit();
                    img.Source = image;
                }
                else
                {
                    if (File.Exists(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + icon))
                    {
                        image.UriSource = new Uri(Properties.Settings.Default.mainfolder + @"\IconsLibrary\" + icon);
                        image.EndInit();
                        img.Source = image;
                    }
                    else
                    {
                        image.UriSource = new Uri("ploticon.ico", UriKind.Relative);
                        image.EndInit();
                        img.Source = image;
                    }

                }
                par.Height = 130;
                btn.Margin = new Thickness(200, 70, 0, 0);
                menu.Name = "menu" + partnm;
                menu.Height = 30;
                menu.Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.SettingsOutline, Width = 18, Height = 18 };
                menu.Click += Menu_Click;
                menu.Margin = new Thickness(-240, 70, 0, 0);
                menu.Width = 50;
                menu.SetResourceReference(Control.StyleProperty, "MaterialDesignToolButton");
                btn.SetResourceReference(Control.StyleProperty, "MaterialDesignFlatButton");
                btn.Height = 30;
                btn.Width = 90;
                //btn.Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Launch, Width = 22, Height = 22};
                btn.Content = Properties.strings.open_lbl.ToUpper();
                btn.Name = "btn" + partnm;
                //btn.HorizontalContentAlignment = HorizontalAlignment.Right;
                btn.Click += btn_Click;

                par.Width = 327;
                System.Windows.Controls.ProgressBar status = new ProgressBar();
                status.Name = "status" + partnm;
                Thickness progressmargin = new Thickness(109, 46, 0, 0);
                status.Margin = progressmargin;
                status.Height = 18;
                status.Width = 193;
                status.HorizontalAlignment = HorizontalAlignment.Left;
                status.VerticalAlignment = VerticalAlignment.Top;
                if (size == 0)
                {
                    status.Visibility = Visibility.Collapsed;
                }
                else
                {
                    status.Maximum = size;
                }
                status.Minimum = 0;
                status.Foreground = Brushes.Indigo;
                status.SetResourceReference(Control.StyleProperty, "");
                
                if (type == PlotType.ViewScreen)
                {
                    if (!Directory.Exists(directory))
                    {
                        btn.Foreground = Brushes.Gray;
                        btn.Content = Properties.strings.lost_lbl.ToUpper();
                        full.Content = Properties.strings.lost;
                    }
                    else
                    {
                        double s = ByteSizeLib.ByteSize.FromBytes(GetDirectorySize(directory)).LargestWholeNumberValue;
                        double sizes = ByteSizeLib.ByteSize.FromBytes(GetDirectorySize(directory)).MegaBytes;
                        string ss = ByteSizeLib.ByteSize.FromBytes(GetDirectorySize(directory)).LargestWholeNumberSymbol;
                        status.Value = sizes;
                        if (size != 0)
                        {
                            full.Content = Math.Round(s, 2) + " " + ss + " / " + capacity;
                        }
                        else
                        {
                            full.Content = Math.Round(s, 2) + " " + ByteSizeLib.ByteSize.FromBytes(GetDirectorySize(directory)).LargestWholeNumberSymbol;
                        }

                    }
                }

                par.Name = "part" + partnm;
                par.Children.Add(full);
                par.Children.Add(lbl);
                par.Children.Add(img);
                par.Children.Add(btn);
                par.Children.Add(status);
                par.Children.Add(menu);

                pr.Content = par;
                System.Windows.Controls.Label l = new Label();
                l.Content = "";
                l.FontSize = 2;
                if (isright)
                {
                    cards.Children.Add(pr);
                    cards.Children.Add(l);

                    System.Windows.Controls.Label ll = new Label();
                    ll.Width = 400;
                    cards.Children.Add(ll);
                }
                else
                {
                    cards.Children.Add(pr);
                    cards.Children.Add(l);
                }

                isright = !isright;
                partnm++;
            }
            catch (Exception h)
            {
                MessageBox.Show(Properties.strings.error_unexpected + "\n\n" + h.Message,"DiskPlot",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            string code = ((Button)sender).Name.Replace("menu", "");
            partplot n = new partplot(partplot.DialogType.Edit, Convert.ToInt32(code) - 1);
            n.ShowDialog();
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == Properties.strings.lost_lbl.ToUpper())
            {
                MessageBox.Show(Properties.strings.error_lost,Properties.strings.error_lost_title,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else if (((Button)sender).Content.ToString() == Properties.strings.open_lbl.ToUpper())
            {
                string code = ((Button)sender).Name.Replace("btn", "");
                Process.Start(Properties.Settings.Default.core[Convert.ToInt32(code) - 1].ToString());
            }
            
        }

        void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.iscreated)
            {
                    Plot(Properties.Settings.Default.name, Properties.Settings.Default.directory, Properties.Settings.Default.icon, Properties.Settings.Default.size, PlotType.Create);
                
                Properties.Settings.Default.directory = "";
                Properties.Settings.Default.name = "";
                Properties.Settings.Default.size = 0;
            }
            else if (Properties.Settings.Default.index > -1)
            {
                int i = Properties.Settings.Default.index;
                Properties.Settings.Default.core2[i] = Properties.Settings.Default.name;
                Properties.Settings.Default.core3[i] = Properties.Settings.Default.size;
                Properties.Settings.Default.core4[i] = Properties.Settings.Default.icon;
                Properties.Settings.Default.directory = "";
                Properties.Settings.Default.name = "";
                Properties.Settings.Default.size = 0;
            }
            Properties.Settings.Default.index = -1;
            Properties.Settings.Default.iscreated = false;
            Properties.Settings.Default.Save();

            if (!Properties.Settings.Default.performancemode)
            {
                LoadPlots(PlotType.ViewScreen);
            }
        }

        public void CreateDiskPlot(string directory)
        {
            string diskplotfolder = directory + @"\DiskPlot";
            if (!Directory.Exists(diskplotfolder))
            {

                Directory.CreateDirectory(diskplotfolder);
                Directory.CreateDirectory(diskplotfolder + @"\IconsLibrary");

                /*
                // Export mainicon.ico
                string s = diskplotfolder + @"\mainicon.ico";
                using (FileStream fs = new FileStream(s, FileMode.Create))
                    Properties.Resources.mainicon.Save(fs);
                File.SetAttributes(s, FileAttributes.Hidden);

                // Create desktop.ini
                string[] lines = { "[.ShellClassInfo]", "IconFile=" + s, "IconIndex=0", "[ViewState]", "Mode=", "Vid=", "FolderType=Generic" };
                File.WriteAllLines(diskplotfolder + @"\desktop.ini", lines);
                File.SetAttributes(diskplotfolder + @"\desktop.ini", FileAttributes.Hidden | FileAttributes.System);*/
                
                Properties.Settings.Default.mainfolder = diskplotfolder;
                Properties.Settings.Default.Save();

                intro d = new intro();
                d.ShowInTaskbar = false;
                d.ShowDialog();
            }
            else
            {
                
            }
        }


        private void Window_Initialized(object sender, EventArgs e)
        {
            settings.ToolTip = Properties.strings.settings;
            refresh.ToolTip = Properties.strings.refresh;
            plots.Content = Properties.strings.plots;
            about.Header = Properties.strings.about;
            website.Header = Properties.strings.website;
            debuginfo.Header = Properties.strings.debuginfo;
            fresh.Header = Properties.strings.fresh_reset;
            uninstall.Header = Properties.strings.uninstall;
            language.Header = Properties.strings.performance;
            Properties.Settings.Default.iscreated = false;
            if (Properties.Settings.Default.firstrun)
            {
                Properties.Settings.Default.performancemode = false;
                Properties.Settings.Default.core = new ArrayList();
                Properties.Settings.Default.core2 = new ArrayList();
                Properties.Settings.Default.core3 = new ArrayList();
                Properties.Settings.Default.core4 = new ArrayList();
                Properties.Settings.Default.iconlibrary = new ArrayList();
                Properties.Settings.Default.firstrun = false;
                Properties.Settings.Default.Save();

                string currentfolder = System.AppDomain.CurrentDomain.BaseDirectory;
                CreateDiskPlot(currentfolder);
            }

            if (Directory.Exists(Properties.Settings.Default.mainfolder))
            {
                DriveInfo n = new DriveInfo(Directory.GetDirectoryRoot(Properties.Settings.Default.mainfolder).Substring(0, 2));
                if (n.VolumeLabel == "")
                {
                    statuslbl.Content = n.Name;
                }
                else
                {
                    statuslbl.Content = n.VolumeLabel;
                }


                ByteSizeLib.ByteSize k = new ByteSizeLib.ByteSize(n.TotalSize);
                ByteSizeLib.ByteSize kk = new ByteSizeLib.ByteSize(n.TotalSize - n.TotalFreeSpace);

                prg.Maximum = k.Bytes;
                prg.Minimum = 0;
                prg.Value = kk.Bytes;
                space.Content = Math.Round(kk.LargestWholeNumberValue, 3) + " " + kk.LargestWholeNumberSymbol + " / " + Math.Round(k.LargestWholeNumberValue, 3) + " " + k.LargestWholeNumberSymbol;
                addplot.IsEnabled = true;
                LoadPlots(PlotType.ViewScreen);

            }
            else
            {
                MessageBox.Show(Properties.strings.error_launch + "\n\n" + Properties.strings.diskplot_main + ":\n" + Properties.Settings.Default.mainfolder + "\n\n" + Properties.strings.diskplot_current + ":\n" + System.AppDomain.CurrentDomain.BaseDirectory,Properties.strings.error_launch_title);
            }

            
        }

        public void LoadPlots(PlotType type)
        {
            cards.Children.Clear();
            partnm = 1;
            isright = false;
            for (int i = 0; i < Properties.Settings.Default.core.Count; i++)
            {
                Plot(Properties.Settings.Default.core2[i].ToString(), Properties.Settings.Default.core[i].ToString(), Properties.Settings.Default.core4[i].ToString(), (double)Properties.Settings.Default.core3[i], type);
            }

            debug.Content = Properties.Settings.Default.core.Count;
            debug.Content += ":directory - " + Properties.Settings.Default.core2.Count;
            debug.Content += ":name - " + Properties.Settings.Default.core3.Count;
            debug.Content += ":size - " + Properties.Settings.Default.core4.Count;
            debug.Content += ":icon  -  " + Properties.Settings.Default.iconlibrary.Count;
            debug.Content += " icons in library. ";
            debug.Content += Properties.Settings.Default.name;
        }

        static long GetDirectorySize(string p)
        {
            string[] a = Directory.GetFiles(p, "*.*",SearchOption.AllDirectories);
            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadPlots(PlotType.ViewScreen);
        }

        private void Addplot_Click(object sender, RoutedEventArgs e)
        {
            partplot n = new partplot(partplot.DialogType.Create,0);
            n.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.strings.delete_fresh + "\n\n" 
                + Properties.strings.warning.ToUpper() + ":\n - " 
                + Properties.strings.warning_fresh + "\n\n" 
                + Properties.strings.ask_continue, Properties.strings.fresh_reset,MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.performancemode = false;
                Properties.Settings.Default.core = new ArrayList();
                Properties.Settings.Default.core2 = new ArrayList();
                Properties.Settings.Default.core3 = new ArrayList();
                Properties.Settings.Default.core4 = new ArrayList();
                Properties.Settings.Default.iconlibrary = new ArrayList();
                Properties.Settings.Default.firstrun = true;
                Properties.Settings.Default.Save();
                this.Close();
            }
            
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //LoadPlots(PlotType.Delete);
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DiskPlot by Yusuf Cihan\nyusufcihandemirbas@gmail.com\nyusufcihan.com\n@yyusufcihan");
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Process.Start("https://yusufcihan.com");
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            debug.Visibility = Visibility.Visible;
        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            debug.Visibility = Visibility.Collapsed;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.strings.delete_uninstall + "\n\n"
                + Properties.strings.warning.ToUpper() + ":\n - "
                + Properties.strings.warning_uninstall + "\n\n"
                + Properties.strings.ask_continue, Properties.strings.uninstall, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Properties.Settings.Default.performancemode = false;
                Properties.Settings.Default.core = new ArrayList();
                Properties.Settings.Default.core2 = new ArrayList();
                Properties.Settings.Default.core3 = new ArrayList();
                Properties.Settings.Default.core4 = new ArrayList();
                Properties.Settings.Default.iconlibrary = new ArrayList();
                Properties.Settings.Default.firstrun = true;
                Properties.Settings.Default.Save();
                if (Directory.Exists(Properties.Settings.Default.mainfolder))
                {
                    Directory.Delete(Properties.Settings.Default.mainfolder,true);
                }

                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\unins000.exe"))
                {
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + @"\unins000.exe");
                }
                this.Close();
            }
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:yusufcihandemirbas@gmail.com");
        }

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://yusufcihan.com");
        }

        private void Language_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.performancemode = true;
            Properties.Settings.Default.Save();
        }

        private void Language_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.performancemode = false;
            Properties.Settings.Default.Save();
        }
    }


}
