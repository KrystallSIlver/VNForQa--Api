using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace VotingSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SampleDataController : Controller
    {
        [HttpPost]
        [ActionName("User")]
        public ActionResult NewUser(string name)
        {
            name = String.IsNullOrEmpty(name) == true ? "Student" : name;
            DataAccessLayer.Models.User user = new DataAccessLayer.Models.User() { Name = name };

            HttpContext.Response.Cookies.Append("Id", $"{user.Id}");
            HttpContext.Response.Cookies.Append("Name", user.Name);

            return Ok(user);
        }

        [HttpPost]
        [ActionName("SetCookies")]
        public ActionResult Cookies([FromBody]Alternative[] alternatives, int type)
        {
            HttpContext.Response.Cookies.Append("alternatives", JsonConvert.SerializeObject(alternatives));
            HttpContext.Response.Cookies.Append("type", JsonConvert.SerializeObject(type));

            return Ok();
        }

        class AltMType
        {
            public Alternative[] Alternatives { get; set; }
            public int? MarkType { get; set; }
        }

        [HttpGet]
        [ActionName("GetCookies")]
        public ActionResult Cookies()
        {
            //List<JsonResult> jsons = new List<JsonResult>();

            string alts, type, urel;

            HttpContext.Request.Cookies.TryGetValue("userRel", out urel);
            HttpContext.Request.Cookies.TryGetValue("alternatives", out alts);
            HttpContext.Request.Cookies.TryGetValue("type", out type);


            if (urel == null || alts == null || type == null) return Ok();

            var a = JsonConvert.DeserializeObject<Alternative[]>(alts);
            var t = JsonConvert.DeserializeObject<int>(type);

            var altMType = new AltMType
            {
                Alternatives = a,
                MarkType = t
            };

            return Ok(altMType);
        }

        [HttpGet]
        [ActionName("DelCookies")]
        public ActionResult DelCookies()
        {
            HttpContext.Response.Cookies.Delete("alternatives");
            HttpContext.Response.Cookies.Delete("type");
            HttpContext.Response.Cookies.Delete("userRel");

            return Ok();
        }

        [HttpGet]
        [ActionName("UserReload")]
        public ActionResult UserReload()
        {
            HttpContext.Response.Cookies.Append("userRel", "true", new CookieOptions() { MaxAge = new TimeSpan(0, 1, 0) });

            return Ok();
        }
    }
}
