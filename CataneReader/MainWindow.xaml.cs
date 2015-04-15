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

namespace CataneReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Reader reader;

        protected List<string> Maps;



        public int Current
        {
            get { return (int)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(int), typeof(MainWindow), new PropertyMetadata(0));




        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Total.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(MainWindow), new PropertyMetadata(0));




        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(MainWindow));

        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            FilePath = @"D:\Source\Python\aiCatane\Source\out.txt";
            reader = new Reader(FilePath);

            reader.Parsed += reader_Parsed;



            reader.Parse();
        }

        void reader_Parsed(object sender, List<string> e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Maps = e;
                TheMap.Text = Maps[0];
                Current = 0;
                Total = Maps.Count;
            }));
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            if (Current == 0)
            {
                Current = Maps.Count;
            }

            Current--;

            TheMap.Text = Maps[Current];
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (Current == Maps.Count - 1)
            {
                Current = 0;
            }
            else 
            {
                Current++;
            }

            TheMap.Text = Maps[Current];
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((int)e.NewValue > 0 && (int)e.NewValue < Maps.Count)
            {
                Current = (int)e.NewValue;
                TheMap.Text = Maps[Current];
            }
        }

        private void ThePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            reader.Parsed -= reader_Parsed;
            reader.Dispose();

            reader = new Reader(FilePath);
            reader.Parsed += reader_Parsed;
        }
    }
}
