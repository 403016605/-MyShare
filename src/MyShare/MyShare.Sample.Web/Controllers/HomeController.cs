using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShare.Kernel.Commands;
using MyShare.Sample.Commands;
using MyShare.Sample.Queries;

namespace MyShare.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryBook _queryBook;

        public HomeController(ICommandSender commandSender, IQueryBook queryBook)
        {
            _queryBook = queryBook;
            _commandSender = commandSender;
        }

        public IActionResult Index()
        {
            ViewData.Model = _queryBook.GetList();
            return View();
        }

        public ActionResult Details(Guid id)
        {
            ViewData.Model = _queryBook.Get(id);
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
            _commandSender.Send(new CreateBookCommand(Guid.NewGuid(), name));
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Remove(Guid id, int version)
        {
            await _commandSender.Send(new RemoveBookCommand(id, version));
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
