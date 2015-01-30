using KodarOAuth.API.Models;
using KodarOAuth.API.Services;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace KodarOAuth.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthService _service = null;
 
        public AccountController()
        {
            _service = new AuthService();
        }
 
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(ModelUser userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            IdentityResult result = await _service.RegisterUser(userModel);
 
            IHttpActionResult errorResult = GetErrorResult(result);
 
            if (errorResult != null)
            {
                return errorResult;
            }
 
            return Ok();
        }
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
 
            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
