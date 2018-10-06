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
    [Guid("89BF6B36-A0B0-4C95-A666-87A55C226986")]
    [CSDeskBandRegistration(Name = "Sample WPF Deskband")]
    public partial class UserControl1 : INotifyPropertyChanged
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
            InitializeComponent();
            Options.MinHorizontal.Width = 60;
            Options.MinVertical.Width = 60;
            Options.MinVertical.Height = 35;

            PercentageDl = 10;
            try
            {
                new Thread(() =>
                {
                    PercentageDl = 50;
                    FritzBoxSoap.FritzBoxSoap soap = new FritzBoxSoap.FritzBoxSoap("192.168.178.1", "-");
                    while (true)
                    {
                        try
                        {
                            this.grid.Dispatcher.Invoke( () =>
                            {
                                this.grid.UpdateLayout();
                                this.grid.Visibility = Visibility.Hidden;
                                this.grid.Visibility = Visibility.Visible;
                            });
                           

                            var currentdl = soap.getCurrentDlSpeed();
                            var currentul = soap.getCurrentUpSpeed();

                            var percdl = soap.getPercentageUsageDownStream();
                            var percul = soap.getPercentageUsageUoStream();

                            this.PercentageDl = Convert.ToInt32(percdl);
                            this.PercentageUl = Convert.ToInt32(percul);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        finally
                        {
                            Thread.Sleep(5000);
                        }
                    }

                }).Start();
             

                Options.ContextMenuItems = ContextMenuItems;
            } catch (Exception e)
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
