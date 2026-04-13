using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class TitlesViewModel
    {
        public ObservableCollection<Title> Titles { get; set; } = new();

        public ICommand SelectTitleCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        private int _skip = 0;
        private const int PageSize = 20;

        public TitlesViewModel()
        {
            SelectTitleCommand = new RelayCommand<Title>(OpenTitle);
            NextPageCommand = new RelayCommand<object>(_ => NextPage());
            PrevPageCommand = new RelayCommand<object>(_ => PrevPage());

            LoadPage();
        }

        private void LoadPage()
        {
            Titles.Clear();

            using var db = new ImdbProjectContext();

            var page = db.Titles
                .OrderBy(t => t.TitleId)
                .Skip(_skip)
                .Take(PageSize)
                .ToList();

            foreach (var t in page)
                Titles.Add(t);
        }

        private void NextPage()
        {
            _skip += PageSize;
            LoadPage();
        }

        private void PrevPage()
        {
            if (_skip >= PageSize)
                _skip -= PageSize;

            LoadPage();
        }

        private void OpenTitle(Title title)
        {
            var main = (MainViewModel)System.Windows.Application.Current.MainWindow.DataContext;
            main.CurrentViewModel = new TitleDetailViewModel(title);
        }
    }
}