using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class MembersController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Add(AddUserFormData model)
        {
            if (ModelState.IsValid)
            {
                var signupFormData = new SignUpFormData
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = "DefaultPassword123!"
                };

                var result = await _userService.CreateUserAsync(signupFormData);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, result.Error);
            }

            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    var memberResult = await _userService.GetUsersAsync(id);
        //    if (!memberResult.Succeeded)
        //    {
        //        return NotFound();
        //    }

        //    var member = memberResult.Result.First();
        //    var model = new User
        //    {
        //        Id = member.Id,
        //        FirstName = member.FirstName,
        //        LastName = member.LastName,
        //        Email = member.Email,
        //        Phone = member.Phone,
        //        JobTitle = member.JobTitle
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(User model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _userService.EditUserAsync(model);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index");
        //        }

        //        ModelState.AddModelError(string.Empty, result.Error);
        //    }

        //    return View(model);
        //}
    }
}
