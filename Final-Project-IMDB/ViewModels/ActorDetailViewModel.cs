using Final_Project_IMDB.Models.Generated;

namespace Final_Project_IMDB.ViewModels
{
    public class ActorDetailViewModel
    {
        public Name SelectedActor { get; set; }

        public ActorDetailViewModel(Name actor)
        {
            SelectedActor = actor;
        }
    }
}