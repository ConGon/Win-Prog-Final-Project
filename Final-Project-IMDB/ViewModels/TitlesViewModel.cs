using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class TitlesViewModel
    {
        public ObservableCollection<Title> Titles { get; set; } = new();
        private bool _sortHighToLow = true;

        public ICommand SortByRatingCommand { get; }
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
            SortByRatingCommand = new RelayCommand<object>(_ => SortByRating());
            LoadPage();
        }

        private void SortByRating()
        {
            using var db = new ImdbProjectContext();

            var page = db.Titles
                .Include(t => t.Rating)
                .OrderByDescending(t => t.Rating != null ? t.Rating.AverageRating : 0)
                .Skip(_skip)
                .Take(PageSize)
                .ToList();

            Titles.Clear();

            foreach (var t in page)
                Titles.Add(t);
        }

        private void LoadPage()
        {
            using var db = new ImdbProjectContext();

            var page = db.Titles
                .OrderBy(t => t.TitleId)
                .Skip(_skip)
                .Take(PageSize)
                .ToList();

            Titles.Clear();
            foreach (var t in page)
                Titles.Add(t);
        }

        public void ApplyFilter(string query)
        {
            using var db = new ImdbProjectContext();

            if (string.IsNullOrWhiteSpace(query))
            {
                LoadPage();
                return;
            }

            _skip = 0;

            var results = db.Titles
                .Where(t => t.PrimaryTitle.Contains(query))
                .OrderBy(t => t.TitleId)
                .Take(200)
                .ToList();

            Titles.Clear();
            foreach (var t in results)
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
            main.CurrentViewModel = new TitleDetailViewModel(title, main);
        }
    }
}