using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class ActorsViewModel
    {
        public ObservableCollection<Name> Actors { get; set; } = new();

        public ICommand SelectActorCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        private int _skip = 0;
        private const int PageSize = 20;

        public ActorsViewModel()
        {
            SelectActorCommand = new RelayCommand<Name>(OpenActor);
            NextPageCommand = new RelayCommand<object>(_ => NextPage());
            PrevPageCommand = new RelayCommand<object>(_ => PrevPage());

            LoadPage();
        }

        private void LoadPage()
        {
            Actors.Clear();

            using var db = new ImdbProjectContext();

            var page = db.Names
                .OrderBy(n => n.NameId)
                .Skip(_skip)
                .Take(PageSize)
                .ToList();

            foreach (var a in page)
                Actors.Add(a);
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

        private void OpenActor(Name actor)
        {
            var main = (MainViewModel)System.Windows.Application.Current.MainWindow.DataContext;
            main.CurrentViewModel = new ActorDetailViewModel(actor);
        }
    }
}