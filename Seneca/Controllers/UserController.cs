using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seneca.Models.Entities;
using Seneca.Services;
using Seneca.Utilities;
using System.Security.Claims;

namespace Seneca.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public readonly MailJetService _mailJetService;

        public UserController(IUserService userService,MailJetService mailJetService)
        {
            _userService = userService;
            _mailJetService = mailJetService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ValidateEmail(long id)
        {
            Usuario usuario = await _userService.FindUser(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateEmail(long id, Usuario usuario)
        {
            Usuario u = await _userService.FindUser(id);
            u.Estado = 1;
            await _userService.UpdateUsuario(u);

            return RedirectToAction("ValidateEmail");

        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(string correo)
        {
            Usuario usuario = await _userService.FindUserByEmail(correo);
            if (usuario == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado.";

                return View("~/Views/Home/RecoverPassword.cshtml");
            }
            string clave = _mailJetService.GenerateRandomString(6);
            usuario.Contrasenia = clave;
            await _userService.UpdateUsuario(usuario);
            await _mailJetService.SendMail(usuario, "Recuperar contraseña", 2);

            TempData["AlertMessage"] = "La contraseña ha sido enviada a su correo electrónico.";

            return RedirectToAction("RecoverPassword", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid) {
                try
                {
                    await _userService.SaveUsuario(usuario);
                    TempData["AlertMessage"] = "Usuario creado con éxito. Por favor revise su correo para activar la cuenta.";

                    await _mailJetService.SendMail(usuario, "Activación de cuenta");
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
                ViewData["Mensaje"] = "Usuario no encontrado o cuenta no activada.";
                return View("~/Views/Home/Login.cshtml");
            }

            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, usuario.Nombres),
                new Claim("id", usuario.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            await _userService.RegisterLog(usuario);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Update(long id)
        {
            Usuario usuario = await _userService.FindUser(id);
            
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(long id, Usuario usuario)
        {
            if(id != usuario.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUsuario(usuario);

                }catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                TempData["AlertMessage"] = "Perfil actualizado.";
            }

            return View(usuario);
        }

    }
}
