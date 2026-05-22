using System.ComponentModel.DataAnnotations;

namespace SmartDiary.Web.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}