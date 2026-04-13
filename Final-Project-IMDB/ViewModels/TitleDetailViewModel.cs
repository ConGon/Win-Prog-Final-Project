using Final_Project_IMDB.Models.Generated;

namespace Final_Project_IMDB.ViewModels
{
    public class TitleDetailViewModel
    {
        public Title SelectedTitle { get; set; }

        public TitleDetailViewModel(Title title)
        {
            SelectedTitle = title;
        }
    }
}