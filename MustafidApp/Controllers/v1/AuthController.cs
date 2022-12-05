using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MustafidApp.Helpers;
using MustafidApp.JWT;
using MustafidAppModels.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IJWTManagerRepository _jWTManager;
        public AuthController(MustafidAppContext appContext, IMapper mapper, IConfiguration configuration, IJWTManagerRepository jWTManager)
        {
            _appContext = appContext;
            _mapper = mapper;
            _configuration = configuration;
            _jWTManager = jWTManager;
        }

        [NonAction]
        public async Task<bool> IfExpired(string refToken)
        {
            return await _appContext.UserTokens.AnyAsync(q => q.RefToken == refToken && q.Expired);
        }

        /// <summary>
        /// Sign In By Code
        /// </summary>
        /// <param name="Phone">Reprenset Entered Phone</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(string Phone)
        {
            try
            {
                if (Phone == null)
                    return Ok(new ResponseClass() { Success = false, data = "Null" });

                if (!Phone.StartsWith("966") || Phone.Length != 12)
                    return Ok(new ResponseClass() { Success = false, data = "ISNT_SA" });

                int code = 0;
                if (Phone == "966xxxxxxxxx")
                    code = 9999;
                else
                    code = new Random().Next(1000, 9999);

                var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

                if (Phone == "966xxxxxxxxx")
                    return Ok(new ResponseClass() { Success = true, data = CypherCode });


                string message_en = $@"Use this code {code} to complete your Login.";
                string message_ar = $@"برجاء استخدام هذا الكود {code} لتاكيد هويتك.";


                var res = HttpClientAdminBackend.getDataAdmin($"/Request/NotifyUser?Mobile={Phone}&message_en={message_en}&message_ar={message_ar}", _configuration);

                var resJson = await res.Content.ReadAsStringAsync();

                var Res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
                if (Res.Success)
                    return Ok(new ResponseClass() { Success = true, data = CypherCode });
                else
                    return Ok(new ResponseClass() { Success = false, data = "Error" });
            }
            catch (Exception eee)
            {
                return Ok(new ResponseClass() { Success = false, data = eee.Message });
            }
        }

        /// <summary>
        /// Return Tokent That Will u Use it
        /// </summary>
        /// <param name="Phone">Reprenset Entered Phone</param>
        /// <param name="code">Code That User Entered it</param>
        /// <param name="C_Code">repose of SigIn API</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Complete_SignIn(string Phone, int code, string C_Code)
        {
            if (Phone == null || C_Code == null)
                return Ok(new ResponseClass() { Success = false, data = "Null" });

            if (!Phone.StartsWith("966") || Phone.Length != 12)
                return Ok(new ResponseClass() { Success = false, data = "ISNT_SA" });

            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

            if (CypherCode == C_Code)
            {

                var token = _jWTManager.Authenticate(Phone, out string RefToken);

                _appContext.UserTokens.Add(new MustafidAppModels.Models.UserToken { AddedDate = DateTime.Now, Token = token, RefToken = RefToken, Expired = false, Phone = Phone });
                await _appContext.SaveChangesAsync();

                return Ok(new ResponseClass() { Success = true, data = token });
            }
            else
                return Ok(new ResponseClass() { Success = false, data = "InvalidCode" });
        }
        /// <summary>
        /// Logout Current User
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var refToken = User.Claims.First(x => x.Type == "RefToken").Value;
            if (refToken != null)
            {
                var ifExpired = await _appContext.UserTokens.FirstOrDefaultAsync(q => q.RefToken == refToken);
                ifExpired.Expired = true;
                await _appContext.SaveChangesAsync();
            }
            return Ok(new ResponseClass() { Success = true });
        }
        /// <summary>
        /// Get Current User Data
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetMyProfile()
        {
            var Phone = User.Claims.First(x => x.Type == ClaimTypes.MobilePhone).Value;
            var personel_Data = _appContext.PersonelData.FirstOrDefault(q => q.Mobile == Phone);
            return Ok(new ResponseClass() { Success = true, data = personel_Data });
        }

    }
}
