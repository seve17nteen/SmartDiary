using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartDiary.Web.Models;
using SmartDiary.Web.ViewModels;

namespace SmartDiary.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;

		private readonly SignInManager<User> _signInManager;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new User
				{
					UserName = model.Username,
					Email = model.Email
				};

				var result =
					await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, false);

					return RedirectToAction("Index", "Task");
				}
			}

			return View(model);
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result =
					await _signInManager.PasswordSignInAsync(
						model.Username,
						model.Password,
						false,
						false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Task");
				}
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Login");
		}
	}
}