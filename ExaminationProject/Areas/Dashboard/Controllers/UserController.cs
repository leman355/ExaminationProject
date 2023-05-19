using ExaminationProject.Areas.Dashboard.ViewModels;
using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Web.Helper;

namespace ExaminationProject.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]

    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;
        private IConfiguration Configuration { get; }

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            UserVM vm = new();
            vm.Users = _userManager.Users.ToList();
            vm.Groups = _context.Groups.ToList();
            vm.UserGroups = _context.UserGroups.ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Groups"] = _context.Groups.Where(x => x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user, IFormFile NewPhoto, int groupId)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, user.Password);
            user.CreatedDate = DateTime.Now;

            user.PhotoUrl = ImageHelper.UploadImage(NewPhoto, _webHostEnvironment);

            await _userManager.CreateAsync(user);

            var gr = _context.Groups.FirstOrDefault(x => x.Id == groupId);

            UserGroup userGroup = new()
            {
                UserId = user.Id,
                GroupId = gr.Id,
            };

            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();
         
            return RedirectToAction(nameof(Index));
        }
        private static string GeneratePassword()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new();
            string password = new string(Enumerable.Repeat(characters, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var usr = await _userManager.FindByIdAsync(id);
            var group = await _context.Groups.ToListAsync();
            var userGroup = await _context.UserGroups.Where(x => x.UserId == usr.Id).ToListAsync();

            UserEditVM editVM = new()
            {
                User = usr,
                Groups = group,
                UserGroups = userGroup,
            };
            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, IFormFile NewPhoto, string OldPhoto, int groupId)
        {
            try
            {
                var data = await _userManager.FindByIdAsync(user.Id);

                if (NewPhoto != null)
                {
                    data.PhotoUrl = ImageHelper.UploadImage(NewPhoto, _webHostEnvironment);
                }
                else
                {
                    data.PhotoUrl = OldPhoto;
                }

                data.UserName = user.UserName;
                data.Surname = user.Surname;
                data.Email = user.Email;
                data.IsDeleted = user.IsDeleted;
                data.UpdatedDate = DateTime.Now;

                await _userManager.UpdateAsync(data);
                await _context.SaveChangesAsync();

                var userGroup = await _context.UserGroups.Where(x => x.UserId == user.Id).ToListAsync();
                _context.UserGroups.RemoveRange(userGroup);
                var gr = await _context.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
                UserGroup ug = new()
                {
                    UserId = user.Id,
                    GroupId = gr.Id,
                };
                await _context.UserGroups.AddAsync(ug);
                await _context.SaveChangesAsync();
             
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> AddRole(string id)
        {
            if (id == null) return NotFound();

            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();

            UserRoleAddViewModel vm = new()
            {
                User = user,
                Roles = roles.Except(userRoles)
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            if (id == null) return NotFound();

            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userAddRole = await _userManager.AddToRoleAsync(user, role);

            if (!userAddRole.Succeeded)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRole(string id)
        {
            if (id == null)
                return NotFound();

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();

            UserRoleAddViewModel vm = new UserRoleAddViewModel()
            {
                User = user,
                Roles = userRoles
            };

            return View("RemoveRole", vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string id, string role)
        {
            if (id == null || role == null)
                return NotFound();

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            if (await _userManager.IsInRoleAsync(user, role))
            {
                var userRemoveRole = await _userManager.RemoveFromRoleAsync(user, role);

                if (!userRemoveRole.Succeeded)
                {
                    ViewBag.Error = "Failed to remove role.";
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            try
            {
                var us = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
                us.IsDeleted = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}