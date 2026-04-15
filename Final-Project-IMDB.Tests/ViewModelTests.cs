using Microsoft.VisualStudio.TestTools.UnitTesting;
using Final_Project_IMDB.ViewModels;
using Final_Project_IMDB.Data.Generated;
using Final_Project_IMDB.Models.Generated;
using System.Linq;

namespace Final_Project_IMDB.Tests
{
    [TestClass]
    public class TitlesViewModelTests
    {
        // Verifies that the TitlesViewModel loads an initial set of titles when created
        [TestMethod]
        public void TitlesViewModel_LoadsTitles_OnStartup()
        {
            var vm = new TitlesViewModel();

            Assert.IsNotNull(vm.Titles);
            Assert.IsTrue(vm.Titles.Count > 0);
        }

        // Verifies that moving to the next page changes the loaded titles
        [TestMethod]
        public void NextPageCommand_ChangesLoadedTitles()
        {
            var vm = new TitlesViewModel();
            var firstPageFirstTitleId = vm.Titles.First().TitleId;

            vm.NextPageCommand.Execute(null);

            Assert.IsTrue(vm.Titles.Count > 0);
            Assert.AreNotEqual(firstPageFirstTitleId, vm.Titles.First().TitleId);
        }

        // Verifies that going to the previous page while already on page one remains valid
        [TestMethod]
        public void PrevPageCommand_OnFirstPage_RemainsValid()
        {
            var vm = new TitlesViewModel();

            vm.PrevPageCommand.Execute(null);

            Assert.IsNotNull(vm.Titles);
            Assert.IsTrue(vm.Titles.Count > 0);
        }

        // Verifies that titles are sorted in descending order by average rating
        [TestMethod]
        public void SortByRatingCommand_SortsDescendingByAverageRating()
        {
            var vm = new TitlesViewModel();

            vm.SortByRatingCommand.Execute(null);

            Assert.IsTrue(vm.Titles.Count > 1);

            for (int i = 0; i < vm.Titles.Count - 1; i++)
            {
                decimal current = vm.Titles[i].Rating?.AverageRating ?? 0;
                decimal next = vm.Titles[i + 1].Rating?.AverageRating ?? 0;

                Assert.IsTrue(current >= next);
            }
        }

        // Verifies that filtering titles returns at least one matching result
        [TestMethod]
        public void ApplyFilter_ReturnsMatchingTitles()
        {
            var vm = new TitlesViewModel();
            var originalCount = vm.Titles.Count;

            var existingTitle = vm.Titles.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.PrimaryTitle));
            Assert.IsNotNull(existingTitle);

            string query = existingTitle.PrimaryTitle.Substring(0, 1);
            vm.ApplyFilter(query);

            Assert.IsTrue(vm.Titles.Count > 0);
            Assert.IsTrue(vm.Titles.Any(t =>
                !string.IsNullOrWhiteSpace(t.PrimaryTitle) &&
                t.PrimaryTitle.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }

        // Verifies that the detail view model correctly loads data for an existing title
        [TestMethod]
        public void TitleDetailViewModel_LoadsDetails_ForExistingTitle()
        {
            using var db = new ImdbProjectContext();
            var title = db.Titles.FirstOrDefault();

            Assert.IsNotNull(title);

            var main = new MainViewModel();
            var vm = new TitleDetailViewModel(title, main);

            Assert.IsNotNull(vm.SelectedTitle);
            Assert.AreEqual(title.TitleId, vm.SelectedTitle.TitleId);
            Assert.IsNotNull(vm.Cast);
            Assert.IsNotNull(vm.Genres);
        }
    }
}