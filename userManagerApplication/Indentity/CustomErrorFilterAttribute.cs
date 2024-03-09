using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace userManagerApplication.Indentity
{
    //Redirect if you do not have appropriate permission or token for this view
    public class CustomErrorFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated, display a custom error message and redirect if necessary.
                //context.Result = new ViewResult
                //{
                //    ViewName = "Error403",
                //    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                //};

                context.Result = new RedirectToActionResult("Index", "Access", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
