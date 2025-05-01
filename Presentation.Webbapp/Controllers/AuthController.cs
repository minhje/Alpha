using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels.Auth;

namespace Presentation.WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
            return View(model);

        var signUpFormData = model.MapTo<SignUpFormData>();

        var result = await _authService.SignUpAsync(signUpFormData);
        if (result.Succeeded)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var signInFormData = model.MapTo<SignInFormData>();

        var result = await _authService.SignInAsync(signInFormData);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    public async Task<IActionResult> Logout(string returnUrl = "~/")
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");

        //return LocalRedirect(returnUrl);
    }
}