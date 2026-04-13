using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Final_Project_IMDB.ViewModels
{
    public class ActorDetailViewModel
    {
        private readonly MainViewModel _main;

        public Name SelectedActor { get; set; }

        public string FormattedProfession { get; set; } = "";

        public List<Title> KnownFor { get; set; } = new();

        public ICommand BackCommand => _main.BackCommand;
        public ICommand OpenTitleCommand { get; }

        public ActorDetailViewModel(Name actor, MainViewModel main)
        {
            _main = main;

            OpenTitleCommand = new RelayCommand<Title>(OpenTitle);

            using var db = new ImdbProjectContext();

            SelectedActor = db.Names
                .First(n => n.NameId == actor.NameId);

            FormattedProfession = Format(SelectedActor.PrimaryProfession);

            KnownFor = (
                from p in db.Principals
                join t in db.Titles on p.TitleId equals t.TitleId
                where p.NameId == actor.NameId
                select t
            ).Distinct().ToList();
        }

        private void OpenTitle(Title title)
        {
            if (title == null) return;
            _main.CurrentViewModel = new TitleDetailViewModel(title, _main);
        }

        private static string Format(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            return string.Join(", ",
                input.Split(',')
                     .Select(x => x.Trim())
                     .Where(x => x.Length > 0));
        }
    }
}