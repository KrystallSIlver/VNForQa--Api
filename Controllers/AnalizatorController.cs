using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Helpers;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VotingSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AnalizatorController : Controller
    {
        [ActionName("Saaty")]
        [HttpPost]
        public ActionResult Saaty([FromBody]Alternative[] array)
        {
            array.ToList().ForEach(x => x.Mark = Math.Round(x.Mark));
            var saaty = NormalizationHelper.DefaultSaaty(array);
            return Ok(saaty);
        }

        [ActionName("UnrangetSaaty")]
        [HttpPost]
        public ActionResult UnrangetSaaty([FromBody]Alternative[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mark = NormalizationHelper.MarkToSaatyRange(array[i].Mark);
            }
            var saaty = NormalizationHelper.DefaultSaaty(array);
            return Ok(saaty);
        }

        [ActionName("KolSaaty")]
        [HttpPost]
        public ActionResult KolSaaty([FromBody]Alternative[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mark = NormalizationHelper.MarkToSaatyRange(array[i].Mark);
            }
            var saaty = NormalizationHelper.KolSaaty(array);
            return Ok(saaty);
        }

        [ActionName("Saaty99")]
        [HttpPost]
        public ActionResult Saaty99([FromBody]Alternative[] array)
        {
            var saaty = NormalizationHelper.Saaty99(array);
            return Ok(saaty);
        }

        [ActionName("Geometry")]
        [HttpPost]
        public ActionResult Geometry([FromBody]Alternative[] array)
        {
            var geom = NormalizationHelper.GeometryMethod(array);
            return Ok(geom);
        }
    }
}
