using Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolikliniksApiController : ControllerBase
    {
        private readonly RandevuContext _context;

        public PolikliniksApiController(RandevuContext context)
        {
            _context = context;
        }
        // get - api/polikliniks
        [HttpGet]
        public ActionResult<IEnumerable<Poliklinik>> GetPoliklinikler()
        {
            return _context.Poliklinikler.ToList();
        }

        // get - api/polikliniks/{number} işte number kaçsa 5 ise 5 v.s
        [HttpGet("{id}")]
        public ActionResult<Poliklinik> GetPoliklinik(int id)
        {
            var poliklinik = _context.Poliklinikler.Find(id);

            if (poliklinik == null)
            {
                return NotFound();
            }

            return poliklinik;
        }

        // post - api/polikliniks
        [HttpPost]
        public ActionResult<Poliklinik> PostPoliklinik(Poliklinik poliklinik)
        {
            _context.Poliklinikler.Add(poliklinik);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPoliklinikler), new { id = poliklinik.PoliklinikId }, poliklinik);
        }

        // put - api/polikliniks/{number} işte number kaçsa 5 ise 5 v.s
        [HttpPut("{id}")]
        public IActionResult PutPoliklinik(int id, Poliklinik poliklinik)
        {
            if (id != poliklinik.PoliklinikId)
            {
                return BadRequest();
            }

            _context.Entry(poliklinik).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // delete - api/polikliniks/{number} işte number kaçsa 5 ise 5 v.s
        [HttpDelete("{id}")]
        public ActionResult<Poliklinik> DeletePoliklinik(int id)
        {
            var poliklinik = _context.Poliklinikler.Find(id);

            if (poliklinik == null)
            {
                return NotFound();
            }

            _context.Poliklinikler.Remove(poliklinik);
            _context.SaveChanges();

            return poliklinik;
        }
    }
}
