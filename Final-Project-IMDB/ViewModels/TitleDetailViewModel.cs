using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class TitleDetailViewModel
    {
        private readonly MainViewModel _main;

        public Title SelectedTitle { get; set; }

        public decimal? AverageRating { get; set; }
        public int? NumVotes { get; set; }

        public List<Name> Cast { get; set; } = new();
        public List<Genre> Genres { get; set; } = new();

        public ICommand BackCommand => _main.BackCommand;

        public TitleDetailViewModel(Title title, MainViewModel main)
        {
            _main = main;

            using var db = new ImdbProjectContext();

            SelectedTitle = db.Titles
                .Include(t => t.Rating)
                .Include(t => t.Genres)
                .First(t => t.TitleId == title.TitleId);

            AverageRating = SelectedTitle.Rating?.AverageRating;
            NumVotes = SelectedTitle.Rating?.NumVotes;

            Genres = SelectedTitle.Genres.ToList();

            Cast = db.Principals
                .Where(p => p.TitleId == title.TitleId)
                .Include(p => p.Name)
                .Select(p => p.Name!)
                .ToList();
        }
    }
}