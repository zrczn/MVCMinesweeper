using Microsoft.AspNetCore.Mvc;
using Minesweeper.Models;

namespace Minesweeper.Controllers
{
    public class MinesweeperController : Controller
    {
        static MinesweeperModel GameSetup;

        static int[,] hiddenBoard;
        static int[,] unhiddenBoard;

        public IActionResult Index()
        {
            ViewBag.DifficultyLevel = null;

            return View("Index");
        }

        public IActionResult ChoosenGameDiff(string difficultyLevel)
        {
            var diff = int.Parse(difficultyLevel);

            GameSetup = new MinesweeperModel(diff);

            unhiddenBoard = GameSetup.IdButtonState;
            hiddenBoard = MinesweeperModel.HiddenFields(unhiddenBoard);


            ViewBag.DifficultyLevel = difficultyLevel;

            return View("Index", hiddenBoard);

        }

        public IActionResult HandleButtonClick(string buttonGrid)
        {

            string[] splittedCoordinates = buttonGrid.Split(" ");

            var x = int.Parse(splittedCoordinates[0]);
            var y = int.Parse(splittedCoordinates[1]);

            ViewBag.DifficultyLevel = $"{GameSetup.Difficulty}";

            if (unhiddenBoard[x, y] == 0)
            {
                ViewBag.GameStatus = "failed";
                hiddenBoard = unhiddenBoard;
            }
            else
            {
                hiddenBoard[x, y] = unhiddenBoard[x, y];
            }

            return View("Index", hiddenBoard);
        }
    }
}
