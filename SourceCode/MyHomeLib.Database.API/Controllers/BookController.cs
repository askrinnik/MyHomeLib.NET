using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyHomeLib.Database.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    private readonly IDatabaseRepository repository;

    public BookController(IDatabaseRepository repository)
    {
      this.repository = repository;
    }

    // GET: api/Book/test
    [HttpGet("{titlePart}", Name = "Get")]
    public IEnumerable<BookInfo> Get(string titlePart)
    {
      if (string.IsNullOrWhiteSpace(titlePart) || titlePart.Length<5)
        return null;
      return repository.GetBooksByTitle(titlePart);
    }
  }
}
