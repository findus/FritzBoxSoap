using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CSDeskBand.Wpf;
using System.Runtime.InteropServices;
using CSDeskBand;
using CSDeskBand.Annotations;
using System.Threading;
using FritzBoxSoap;

namespace Sample.Wpf
{
    /// <summary>
    /// Example WPF deskband. Shows taskbar info capabilities and context menus
    /// </summary>
    [ComVisible(true)]
    [Guid("89BF6B36-A0B0-4C95-A666-87A55C226985")]
    [CSDeskBandRegistration(Name = "Fritzbox Widget")]
    public partial class UserControl1: INotifyPropertyChanged
    {
        private int _percentageDl;
        public int PercentageDl
        {
            get => _percentageDl;
            set
            {
                if (value == _percentageDl) return;
                _percentageDl = value;
                OnPropertyChanged();
            }
        }

        private int _percentageUl;

        public int PercentageUl
        {
            get => _percentageUl;
            set
            {
                if (value == _percentageUl) return;
                _percentageUl = value;
                OnPropertyChanged();
            }
        }

        private String _rx;

        public String Rx
        {
            get => _rx;
            set
            {
                if (value == _rx) return;
                _rx = value;
                OnPropertyChanged();
            }
        }

        private String _tx;

        public String Tx
        {
            get => _tx;
            set
            {
                if (value == _tx) return;
                _tx = value;
                OnPropertyChanged();
            }
        }

        private System.Windows.Media.Brush _progressbarColor = Brushes.White ;

        public System.Windows.Media.Brush ProgressbarColor
        {
            get => _progressbarColor;
            set
            {
                if (value == _progressbarColor) return;
                _progressbarColor = value;
                OnPropertyChanged();
            }
        }


        private List<CSDeskBandMenuItem> ContextMenuItems
        {
            get
            {
                var action = new CSDeskBandMenuAction("Action - Toggle submenu");
                var separator = new CSDeskBandMenuSeparator();
                var submenuAction = new CSDeskBandMenuAction("Submenu Action - Toggle checkmark");
                var submenu = new CSDeskBandMenu("Submenu")
                {
                    Items = { submenuAction }
                };

                action.Clicked += (sender, args) => submenu.Enabled = !submenu.Enabled;
                submenuAction.Clicked += (sender, args) => submenuAction.Checked = !submenuAction.Checked;

                return new List<CSDeskBandMenuItem>() {action, separator, submenu};
            }
        }

        public UserControl1()
        {
            // InitializeComponent();

            InitializeComponent();
            Options.MinHorizontal.Width = 60;
            Options.MinVertical.Width = 60;
            Options.MinVertical.Height = 35;

            PercentageDl = 10;
            try
            {
                Thread thread = new Thread(() =>
                {
                    PercentageDl = 10;
                    FritzBoxSoap.FritzBoxSoap soap = new FritzBoxSoap.FritzBoxSoap("192.168.178.1", "-");
                    while (true)
                    {
                        try
                        {
                            this.mainGrid.Dispatcher.Invoke(() =>
                            {
                                this.mainGrid.UpdateLayout();
                                this.mainGrid.Visibility = Visibility.Hidden;
                                this.mainGrid.Visibility = Visibility.Visible;
                            });


                            var currentdl = soap.getCurrentDlSpeed();
                            var currentul = soap.getCurrentUpSpeed();

                            var percdl = soap.getPercentageUsageDownStream();
                            var percul = soap.getPercentageUsageUoStream();

                            this.ProgressbarColor = Brushes.White;

                            this.PercentageDl = Convert.ToInt32(percdl);
                            this.PercentageUl = Convert.ToInt32(percul);

                            this.Rx = currentdl.ToString();
                            this.Tx = currentul.ToString();

                  

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);

                            this.PercentageDl = Convert.ToInt32(50);
                            this.PercentageUl = Convert.ToInt32(50);

                            this.ProgressbarColor = Brushes.Red;

                        }
                        finally
                        {
                            Thread.Sleep(5000);
                        }
                    }

                });

                thread.Start();

            }
            catch (Exception e)
            {

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
