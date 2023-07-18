using Microsoft.AspNetCore.Mvc;
using Minesweeper.Models;

namespace Minesweeper.Controllers
{
    public class TestsController : Controller
    {
        public IActionResult Index()
        {
            MinesweeperModel testgame = new MinesweeperModel(1);

            return View("Index", testgame.IdButtonState);
        }
    }
}
