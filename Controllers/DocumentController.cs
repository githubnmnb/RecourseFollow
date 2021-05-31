using SahisRucu.Models;
using SahisRucu.YardimciModeller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE1006 // Naming Styles

namespace SahisRucu.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DocumentController : ControllerBase
  {

    [HttpPost("[action]")]
#pragma warning disable CA1822 // Mark members as static
    public reqResult<document[]> upload(List<IFormFile> files, bool isTemp)
#pragma warning restore CA1822 // Mark members as static
    {
      List<Yardimci.Document> docs = new List<Yardimci.Document>();
#pragma warning disable CA1062 // Validate arguments of public methods
      foreach (var fl in files)
#pragma warning restore CA1062 // Validate arguments of public methods
      {
        if (fl.Length > 0)
        {
          var doc = Yardimci.Document.Empty(fl.FileName, isTemp);
          using (var stream = new System.IO.FileStream(doc.StoreFilename, System.IO.FileMode.OpenOrCreate))
          {
            Task.Factory
                .StartNew(() => fl.CopyToAsync(stream).Wait(),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning, // guarantees separate thread
                    TaskScheduler.Default)
                .Wait();
          }
          docs.Add(doc);
        }
      }
      return reqResult<document[]>.gen((from x in docs select x.AsModel()).ToArray());
    }

    [HttpGet("[action]")]
    public IActionResult download(string documentID, bool isTemp)
    {
      try
      {
        Yardimci.Document doc = Yardimci.Document.Get(documentID, isTemp);
#pragma warning disable CA2000 // Dispose objects before losing scope
        return File(new System.IO.FileStream(doc.StoreFilename, System.IO.FileMode.Open), "application/octet-stream", doc.Filename);
#pragma warning restore CA2000 // Dispose objects before losing scope
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch
#pragma warning restore CA1031 // Do not catch general exception types
      {
        return NotFound();
      }
    }

    [HttpGet("[action]")]
#pragma warning disable CA1822 // Mark members as static
    public reqResult<document> info(string cUser, string cComp, string documentID, bool isTemp)
#pragma warning restore CA1822 // Mark members as static
    {
      Yardimci.Document doc = Yardimci.Document.Get(documentID, isTemp);
      return reqResult<document>.gen(doc.AsModel());
    }

  }
}

namespace SahisRucu.Models
{
  public class document
  {
#pragma warning disable CA1051 // Do not declare visible instance fields
    public string documentID;
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning disable CA1051 // Do not declare visible instance fields
    public string filename;
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning disable CA1051 // Do not declare visible instance fields
    public string extension;
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning disable CA1051 // Do not declare visible instance fields
    public bool isTemporary;
#pragma warning restore CA1051 // Do not declare visible instance fields
  }

}

#pragma warning restore IDE1006 // Naming Styles
