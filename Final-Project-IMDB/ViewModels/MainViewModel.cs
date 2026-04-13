using Final_Project_IMDB.Models.Generated;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Final_Project_IMDB.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Pages { get; } = new() { "Home", "Actors", "Titles" };
        public Title SelectedTitle { get; set; }
        public Name SelectedActor { get; set; }

        public void OpenTitle(Title title)
        {
            CurrentViewModel = new TitleDetailViewModel(title);
        }

        public void OpenActor(Name actor)
        {
            CurrentViewModel = new ActorDetailViewModel(actor);
        }

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
                // Optionally trigger filtering logic here
            }
        }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private void SwitchView(string page)
        {
            System.Diagnostics.Debug.WriteLine("SWITCH VIEW: " + page);

            CurrentViewModel = page switch
            {
                "Home" => new HomeViewModel(),
                "Actors" => new ActorsViewModel(),
                "Titles" => new TitlesViewModel(),
                _ => CurrentViewModel
            };
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            SelectedPage = "Home";  // sets CurrentViewModel via SwitchView
        }
    }
}