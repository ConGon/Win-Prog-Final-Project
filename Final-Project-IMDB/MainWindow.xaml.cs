using System.Windows;
using Final_Project_IMDB.ViewModels;

namespace Final_Project_IMDB
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}