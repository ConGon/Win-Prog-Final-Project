using Final_Project_IMDB.Models.Generated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;
        private readonly Stack<object> _history = new();
        private bool _isNavigatingBack;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand BackCommand { get; }

        public List<string> Pages { get; } = new() { "Home", "Actors", "Titles" };

        public Title SelectedTitle { get; set; }
        public Name SelectedActor { get; set; }

        private string _selectedPage;
        public string SelectedPage
        {
            get => _selectedPage;
            set
            {
                if (_selectedPage == value) return;
                _selectedPage = value;
                OnPropertyChanged(nameof(SelectedPage));
                SwitchView(value);
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value) return;
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));

                ApplySearch();
            }
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != null && !_isNavigatingBack)
                    _history.Push(_currentViewModel);

                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel()
        {
            BackCommand = new RelayCommand<object>(_ => GoBack());
            SelectedPage = "Home";
        }

        public void OpenActor(Name actor)
        {
            CurrentViewModel = new ActorDetailViewModel(actor, this);
        }

        public void OpenTitle(Title title)
        {
            CurrentViewModel = new TitleDetailViewModel(title, this);
        }

        private void SwitchView(string page)
        {
            CurrentViewModel = page switch
            {
                "Home" => new HomeViewModel(),
                "Actors" => new ActorsViewModel(),
                "Titles" => new TitlesViewModel(),
                _ => CurrentViewModel
            };
        }

        private void ApplySearch()
        {
            if (CurrentViewModel is ActorsViewModel actorsVM)
            {
                actorsVM.ApplyFilter(_searchQuery);
                return;
            }

            if (CurrentViewModel is TitlesViewModel titlesVM)
            {
                titlesVM.ApplyFilter(_searchQuery);
                return;
            }

            // Home: do nothing
        }

        private void GoBack()
        {
            if (_history.Count == 0)
                return;

            _isNavigatingBack = true;
            CurrentViewModel = _history.Pop();
            _isNavigatingBack = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}