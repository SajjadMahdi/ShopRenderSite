﻿using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using GoogleReCaptcha.V3.Interface;
using Microsoft.Win32;
using System.Reflection;

namespace Shop.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constractor
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }
        #endregion

        #region register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {
            #region captcha Validator
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Token))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نمیباشد";
                return View(register);
            }
            #endregion

            if (ModelState.IsValid)
            {
                var result = await _userService.RegisteUser(register);
                switch (result)
                {
                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "شماره تلفن وارد شده قبلا در سیستم ثبت شده است";
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد";
                        return RedirectToAction("ActiveAccount","Account",new { mobile =register.PhoneNumber });
                }
            }

            return View(register);
        }
        #endregion

        #region login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            #region captcha Validator
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Token))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نمیباشد";
                return View(login);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(login);
                switch (result)
                {
                    case LoginUserResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
                    case LoginUserResult.NotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نمیباشد";
                        break;
                    case LoginUserResult.IsBlocked:
                        TempData[WarningMessage] = "حساب شما توسط واحد پشتیبانی مسدود شده است";
                        TempData[InfoMessage] = "جهت اطلاع بیشتر لطفا به قسمت تماس باما مراجعه کنید";
                        break;
                    case LoginUserResult.Success:
                        var user = await _userService.GetUserByPhoneNumber(login.PhoneNumber);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.PhoneNumber),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principle, properties);
                        TempData[SuccessMessage] = "شما با موفقیت وارد شدید";
                        return Redirect("/");
                }
            }

            return View(login);
        }
        #endregion

        #region log-out

        [HttpGet("log-Out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            TempData[InfoMessage] = "شما با موفقیت خارج شدید";
            return Redirect("/");
        }
        #endregion

        #region activate account


        [HttpGet("activate-account/{mobile}")]
        public async Task<IActionResult> ActiveAccount(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activeAccount = new ActiveAccountViewModel { PhoneNumber = mobile };

            return View(activeAccount);
        }

        [HttpPost("activate-account/{mobile}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveAccount(ActiveAccountViewModel activeAccount)
        {

            #region captcha Validator
            if (!await _captchaValidator.IsCaptchaPassedAsync(activeAccount.Token))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نمیباشد";
                return View(activeAccount);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.ActiveAccount(activeAccount);
                switch (result)
                {
                    case ActiveAccountResult.Error:
                        TempData[ErrorMessage] = "عملیات فعال کردن حساب کاربری با شکست مواجه شد";
                        break;
                    case ActiveAccountResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ActiveAccountResult.Success:
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        TempData[InfoMessage] = "لظفا جهت ادامه فراید وارد حساب کاربری خود شود";
                        return RedirectToAction("Login");
                }
            }
            return View(activeAccount);
        }

        #endregion
    }
}
