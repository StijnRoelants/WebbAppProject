using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Klant> _signInManager;
        private readonly UserManager<Klant> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly WebshopContext _context;

        public Postcode postcode = null;
        public Land land = null;

        public RegisterModel(
            UserManager<Klant> userManager,
            SignInManager<Klant> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            WebshopContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Voornaam")]
            public string Voornaam { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Achternaam")]
            public string Achternaam { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Mobiel")]
            public string Mobiel { get; set; }

            [Required]  
            [DataType(DataType.PostalCode)]
            [Display(Name = "Postcode")]
            public string Postcode { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Gemeente")]
            public string Gemeente { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Land")]
            public string Land { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Adres")]
            public string Adres { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Firmanaam")]
            public string Firmanaam { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Btwnummer")]
            public string BtwNummer { get; set; }

            [Display(Name = "Bedrijf")]
            public bool IsBedrijf { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            postcode = await _context.Postcodes
                .Where(x => x.Nummer == Input.Postcode)
                .FirstOrDefaultAsync();
            land = await _context.Landen
                .Where(x => x.Naam.ToUpper() == Input.Land.ToUpper() || x.Zoeknaam.ToUpper() == Input.Land.ToUpper())
                .FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var user = new Klant()
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Voornaam = Input.Voornaam,
                    Achternaam = Input.Achternaam,
                    StraatEnNummer = Input.Adres,
                    Mobiel = Input.Mobiel,
                    Land = land,
                    Postcode = postcode,
                    PostcodeID = postcode.PostcodeID,
                    LandID = land.LandID,
                    IsBedrijf = Input.IsBedrijf
                };
                user.WachtWoordReset = false;
                if (user.IsBedrijf == true)
                {
                    user.Naam = Input.Firmanaam.ToString().Replace(".", "");
                    user.BtwNummer = Input.BtwNummer;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
