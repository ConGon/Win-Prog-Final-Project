using System;
using System.Collections.Generic;

namespace Final_Project_IMDB.Models.Generated;

public partial class Rating
{
    public string TitleId { get; set; } = null!;

    public decimal? AverageRating { get; set; }

    public int? NumVotes { get; set; }

    public virtual Title Title { get; set; } = null!;
}
