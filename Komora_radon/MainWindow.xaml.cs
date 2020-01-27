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
using System.Threading;
using LiveCharts;
using LiveCharts.Wpf;


namespace Komora_radon
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // SeriesCollection data = new SeriesCollection();
        SeriesCollection data;
        Hardware electronics;
        Gas chamber_air;

        public MainWindow()
        {
            InitializeComponent();
            chamber_air = new Gas(0.001, Double.Parse(radium_act_box.Text));

            data = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Ra-226",
                    Values = new ChartValues<double> { 1, 2, 3, 4 },
                    PointGeometry = null
                }
                
            };
            
            

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Combo_ports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            electronics = new Hardware(Combo_ports.SelectedItem.ToString());
        }

        private void Combo_ports_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            string[] ports = Hardware.Get_ports();
            Combo_ports.Items.Clear();
            foreach(string s in ports)
            {
                Combo_ports.Items.Add(s);
            }
            
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            if(null != electronics)
                electronics.Start();
        }

        private void disconnect_button_Click(object sender, RoutedEventArgs e)
        {
            if( null != electronics)
                electronics.Stop();
        }

        private void send_button_Click(object sender, RoutedEventArgs e)
        {
            if(null != electronics)
            {
                electronics.Write(command_text.Text.ToString());
                received_text.Text = electronics.Get_data();
            }
        }


    }
}
