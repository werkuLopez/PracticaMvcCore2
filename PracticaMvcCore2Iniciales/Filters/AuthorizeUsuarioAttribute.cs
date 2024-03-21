﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PracticaMvcCore2Iniciales.Models;

namespace PracticaMvcCore2Iniciales.Filters
{
    public class AuthorizeUsuarioAttribute : AuthorizeAttribute,
        IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();

            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();

            var tempData = provider.LoadTempData(context.HttpContext);
            tempData["controller"] = controller;
            tempData["action"] = action;

            provider.SaveTempData(context.HttpContext, tempData);

            if (usuario.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRoute("Managed", "LogIn");
            }
        }

        public RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary route =
                new RouteValueDictionary(
                new
                {
                    controller = controller,
                    action = action
                });

            RedirectToRouteResult result =
                new RedirectToRouteResult(route);
            return result;
        }
    }
}
