using EMGVoitures.Models;
using EMGVoitures.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMGVoitures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Chercher l'utilisateur par son nom d'utilisateur
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(); // Si l'utilisateur n'existe pas
            }

            // V�rifier le mot de passe
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(); // Si l'authentification �choue
            }

            // G�n�rer le token JWT
            var token = _jwtService.GenerateJwtToken(user.Id);

            // R�cup�rer les r�les de l'utilisateur
            var roles = await _userManager.GetRolesAsync(user);

            // Retourner le token et les r�les dans la r�ponse
            return Ok(new
            {
                Token = token,
                Roles = roles  // Inclure les r�les dans la r�ponse
            });
        }
    }

    // Mod�le de login
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
