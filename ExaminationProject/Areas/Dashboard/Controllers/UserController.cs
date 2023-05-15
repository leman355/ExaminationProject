using ExaminationProject.Areas.Dashboard.ViewModels;
using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Helper;
using WebApp.DTOs;

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


        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        public IActionResult Index()
        {
            UserVM vm = new();
            vm.Users = _userManager.Users.ToList();
            vm.Groups = _context.Groups.ToList();
            vm.UserGroups = _context.UserGroups.ToList();
            //var users = _userManager.Users.ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var group = _context.Groups.Where(x => x.IsDeleted == false).ToList();

            if (group.Count == 0)
            {
                return RedirectToAction("Create", "Group");
            }

            ViewData["Groups"] = _context.Groups.Where(x => x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user, IFormFile NewPhoto, int groupId, bool IsDeleted)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "123123Az@");
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            user.IsDeleted = IsDeleted;

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
        public async Task<IActionResult> Edit(string id)
        {
            var gr = await _context.Groups.Where(x => x.IsDeleted == false).ToListAsync();
             if (gr.Count == 0)
            {
                return RedirectToAction("Create", "Group");
            }

            var usr = await _userManager.FindByIdAsync(id);
            var group = _context.Groups.Where(x => x.IsDeleted == false).ToList();
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
        public async Task<IActionResult> Edit(User user, IFormFile NewPhoto, string OldPhoto, int groupId, bool IsDeleted)
        {
            try
            {
                if (NewPhoto != null)
                {
                    user.PhotoUrl = ImageHelper.UploadImage(NewPhoto, _webHostEnvironment);
                }
                else
                {
                    user.PhotoUrl = OldPhoto;
                }

                DateTime createdDate = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == user.Id)?.CreatedDate ?? DateTime.MinValue;
                if (createdDate != DateTime.MinValue)
                {
                    user.CreatedDate = createdDate;
                }
                _context.Entry(user).Property(x => x.CreatedDate).IsModified = false;

                //user.IsDeleted = IsDeleted;
                //user.UpdatedDate = DateTime.Now;
                var data = await _userManager.FindByIdAsync(user.Id);
                data.Surname = user.Surname;
                data.Email = user.Email;
                data.UserName = user.UserName;
                data.IsDeleted = IsDeleted;
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
        //[HttpGet]
        //public async Task<IActionResult> UserInfo()
        //{
        //    var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    User user = await _userManager.FindByIdAsync(userId);

        //    return View(user);
        //}

    }
}
