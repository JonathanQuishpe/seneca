using Microsoft.AspNetCore.Mvc;
using Seneca.Models.Entities;
using Seneca.Services;

namespace Seneca.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;
        public IUserService _userService;

        public UserController(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid) {
                try
                {
                    await _userService.SaveUsuario(usuario);
                    TempData["AlertMessage"] = "Usuario creado con éxito. Por favor revise su correo para activar la cuenta.";
                }
                catch (Exception ex) {
                    ModelState.AddModelError(String.Empty,ex.Message);
                }

                return RedirectToAction("Login", "Home");
            }

            return View("~/Views/Home/Register.cshtml", usuario);
            
        }
    }
}
