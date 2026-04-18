using LKM_PAA.Helpers;
using Microsoft.AspNetCore.Mvc;
using LKM_PAA.Models;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly SqlDbHelper _db;

        public BooksController()
        {
            _db = new SqlDbHelper("Host=localhost;Username=postgres;Password=babamamak;Database=dbPerpus");
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _db.ExecuteQuery("SELECT * FROM books WHERE deleted_at IS NULL");
            return Ok(new { status = "success", data });
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _db.ExecuteQuery(
                "SELECT * FROM books WHERE id=@id AND deleted_at IS NULL",
                new Dictionary<string, object> { { "@id", id } }
            );

            if (data.Count == 0)
                return NotFound(new { status = "error", message = "Data tidak ditemukan" });

            return Ok(new { status = "success", data = data[0] });
        }

        // POST
        [HttpPost]
        public IActionResult Create([FromBody] Book input)
        {
            _db.ExecuteNonQuery(
                "INSERT INTO books (title, author, stock) VALUES (@title, @author, @stock)",
                new Dictionary<string, object>
                {
                    { "@title", (string)input.title },
                    { "@author", (string)input.author },
                    { "@stock", (int)input.stock }
                }
            );

            return Ok(new { status = "success", message = "Berhasil ditambahkan" });
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book input)
        {
            var result = _db.ExecuteNonQuery(
                "UPDATE books SET title=@title, author=@author, stock=@stock WHERE id=@id AND deleted_at IS NULL",
                new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@title", (string)input.title },
                    { "@author", (string)input.author },
                    { "@stock", (int)input.stock }
                }
            );

            if (result == 0)
                return NotFound(new { status = "error", message = "Data tidak ditemukan" });

            return Ok(new { status = "success", message = "Berhasil diupdate" });
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _db.ExecuteNonQuery(
                "UPDATE books SET deleted_at = CURRENT_TIMESTAMP WHERE id = @id",
                new Dictionary<string, object> { { "@id", id } }
            );

            if (result == 0)
                return NotFound(new { status = "error", message = "Data tidak ditemukan" });

            return Ok(new { status = "success", message = "Berhasil dihapus" });
        }
    }
}