using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Seneca.Models.Entities;
using Seneca.Services;
using System.Security.Claims;

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


        [HttpPost]
        public async Task<IActionResult> FindUser(string correo, string clave)
        {
            Usuario usuario = await _userService.GetUsuario(correo, clave);

            if (usuario == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado.";
                return View("~/Views/Home/Login.cshtml");
            }

            List<Claim> claims = new List<Claim>() { 
            new Claim(ClaimTypes.Name, usuario.Nombres)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("Index", "Home");
        }
    }
}
