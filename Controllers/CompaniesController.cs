using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Data.SqlClient;
using SahisRucu.Models;
using SahisRucu.Yardimci;
using SahisRucu.YardimciModeller;

namespace SahisRucu.Models
{
  public class companyList
  {
    public string tramerNo;
    public string sirketAdi;
    public bool aktif;
    public string yetkili;
    public string vergiNo;
  }


  namespace SahisRucu.Controllers
  {
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : Controller
    {

      [HttpGet("[action]")]
      public reqResult<companyList[]> SirketListe()
      {
        List<companyList> result = new List<companyList>();

        var res = SQL.read("SELECT TramerNo, SirketAdi, Aktif, Yetkili, VergiNo FROM Sirketler ORDER BY SirketAdi", new { });

        foreach (var item in res)
        {
          result.Add(new companyList
          {
            tramerNo = (string)item["TramerNo"],
            sirketAdi = (string)item["SirketAdi"],
            yetkili = (string)item["Yetkili"],
            aktif = (bool)item["Aktif"],
            vergiNo = (string)item["VergiNo"]
          });
        }
        return reqResult<companyList[]>.gen(result.ToArray());
      }
    }
  }
}
