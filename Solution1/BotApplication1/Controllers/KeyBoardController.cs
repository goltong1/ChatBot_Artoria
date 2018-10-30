using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BotApplication1.Controllers
{
    public class KeyBoardController : Controller
    {
        // GET: KeyBoard
        public ActionResult Index()
        {
            Keyboard keyboard = new Keyboard();

            keyboard.type = "buttons";
            keyboard.buttons = new string[] {"대화 시작"};

            return Json(keyboard, JsonRequestBehavior.AllowGet);
            return View();
        }

        public class Keyboard
        {
            public string type { get; set; }
            public string[] buttons { get; set; }

        }
    }
}