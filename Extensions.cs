using CQRS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Extensions
{
    public static class Extensions
    {
        public static IActionResult ToResponse(this CQRSResponse response)
        {
            if (response.IsUnauthorised)
                return new StatusCodeResult(response.StatusCode);
            if (response.IsUnsuccessful)
                return new ObjectResult(new { response.ErrorMessage }) { StatusCode = response.StatusCode };
            else if (response.HasData)
                return new ObjectResult(new { Data = response.GetData() }) { StatusCode = response.StatusCode };
            else
                return new StatusCodeResult(response.StatusCode);
        }
    }
}
