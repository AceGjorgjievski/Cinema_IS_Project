using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models.Domain;
using Cinema.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CinemaUser> userManager;
        private readonly SignInManager<CinemaUser> signInManager;
        private readonly ApplicationDbContext _context; 

        public AccountController(
            UserManager<CinemaUser> userManager, 
            SignInManager<CinemaUser> signInManager,
            ApplicationDbContext context) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context; 
        }

        [HttpGet]
        public IActionResult Register()
        {
            CinemaUserRegistrationDto model = new CinemaUserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(CinemaUserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new CinemaUser
                    {
                        Name = request.Name,
                        Surname = request.LastName,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = request.PhoneNumber
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        // Create and associate an Order for the user
                        // var order = new Order
                        // {
                        //     CinemaUser = user, // Associate the Order with the user
                        //     // Set other properties for the Order as needed
                        // };
                        //
                        // // Add the Order to the context
                        // _context.Orders.Add(order);
                        // user.Or = order.Id;
                        //
                        // // Save changes to the database
                        // await _context.SaveChangesAsync();
        
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            CinemaUserLoginDto model = new CinemaUserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(CinemaUserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}