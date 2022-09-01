﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MustafidApp.Helpers;
using MustafidApp.JWT;
using MustafidAppModels.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [AllowAnonymous]
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
        /// <summary>
        /// Sign In By Code
        /// </summary>
        /// <param name="Phone">Reprenset Entered Phone</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignIn(string Phone)
        {
            if (Phone == null)
                return Ok(new ResponseClass() { Success = false, data = "Null" });

            if (!Phone.StartsWith("966") && !Phone.StartsWith("+966"))
                return Ok(new ResponseClass() { Success = false, data = "ISNT_SA" });

            //int code = new Random().Next(1000, 9999);
            int code = 9999;

            string message_en = $@"Use this code {code} to complete your Login.";
            string message_ar = $@"برجاء استخدام هذا الكود {code} لتاكيد هويتك.";

            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());
            var res = HttpClientAdminBackend.getDataAdmin($"/Request/NotifyUser?Mobile={Phone}&message_en={message_en}&message_ar={message_ar}", _configuration);

            var resJson = await res.Content.ReadAsStringAsync();

            var Res = JsonConvert.DeserializeObject<ResponseClass>(resJson);
            if (Res.Success)
                return Ok(new ResponseClass() { Success = true, data = CypherCode });
            else
                return Ok(new ResponseClass() { Success = false });
        }
        [HttpPost]
        public async Task<IActionResult> Complete_SignIn(string Phone, int code, string C_Code)
        {
            if (Phone == null || C_Code == null)
                return Ok(new ResponseClass() { Success = false, data = "Null" });

            if (!Phone.StartsWith("966") && !Phone.StartsWith("+966"))
                return Ok(new ResponseClass() { Success = false, data = "ISNT_SA" });

            var CypherCode = Helpers.EncryptManager.EncryptString(code.ToString());

            if (CypherCode == C_Code)
            {
                var token = _jWTManager.Authenticate(Phone);

                return Ok(new ResponseClass() { Success = true, data = token });
            }
            else
                return Ok(new ResponseClass() { Success = false });
        }
    }
}