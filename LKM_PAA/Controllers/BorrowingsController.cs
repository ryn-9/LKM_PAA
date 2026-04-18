using LKM_PAA.Helpers;
using LKM_PAA.Models;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingsController : ControllerBase
    {
        private readonly SqlDbHelper _db;

        public BorrowingsController()
        {
            _db = new SqlDbHelper("Host=localhost;Username=postgres;Password=babamamak;Database=dbPerpus");
        }

        //GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _db.ExecuteQuery(@"
                SELECT b.id, u.name AS user_name, bk.title AS book_title,
                       b.borrow_date, b.return_date
                FROM borrowings b
                JOIN users u ON b.user_id = u.id
                JOIN books bk ON b.book_id = bk.id
                WHERE b.deleted_at IS NULL
                ORDER BY b.id ASC
            ");

            return Ok(new
            {
                status = "success",
                data
            });
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _db.ExecuteQuery(@"
                SELECT b.id, u.name AS user_name, bk.title AS book_title,
                       b.borrow_date, b.return_date
                FROM borrowings b
                JOIN users u ON b.user_id = u.id
                JOIN books bk ON b.book_id = bk.id
                WHERE b.id = @id AND b.deleted_at IS NULL
            ",
            new Dictionary<string, object> { { "@id", id } });

            if (data.Count == 0)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "Data peminjaman tidak ditemukan"
                });
            }

            return Ok(new
            {
                status = "success",
                data = data[0]
            });
        }

        // CREATE
        [HttpPost]
        public IActionResult Create([FromBody] Borrowing input)
        {
            if (input.user_id == null || input.book_id == null || input.borrow_date == null)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "user_id, book_id, dan borrow_date wajib diisi"
                });
            }

            _db.ExecuteNonQuery(@"
                INSERT INTO borrowings (user_id, book_id, borrow_date, return_date)
                VALUES (@user_id, @book_id, @borrow_date, @return_date)",

                new Dictionary<string, object>
                {
                    { "@user_id", input.user_id },
                    { "@book_id", input.book_id },
                    { "@borrow_date", input.borrow_date },
                    { "@return_date", input.return_date ?? (object)DBNull.Value }
                }
            );

            return Ok(new
            {
                status = "success",
                message = "Data peminjaman berhasil ditambahkan"
            });
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Borrowing input)
        {
            var result = _db.ExecuteNonQuery(@"
                UPDATE borrowings 
                SET user_id = @user_id,
                    book_id = @book_id,
                    borrow_date = @borrow_date,
                    return_date = @return_date,
                    updated_at = CURRENT_TIMESTAMP
                WHERE id = @id AND deleted_at IS NULL
            ",
            new Dictionary<string, object>
            {
                { "@id", id },
                { "@user_id", (int)input.user_id },
                { "@book_id", (int)input.book_id },
                { "@borrow_date", (DateTime)input.borrow_date },
                { "@return_date", input.return_date != null ? (DateTime)input.return_date : DBNull.Value }
            });

            if (result == 0)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "Data peminjaman tidak ditemukan"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Data peminjaman berhasil diupdate"
            });
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _db.ExecuteNonQuery(
                "UPDATE borrowings SET deleted_at = CURRENT_TIMESTAMP WHERE id = @id AND deleted_at IS NULL",
                new Dictionary<string, object> { { "@id", id } }
            );

            if (result == 0)
                return NotFound(new { status = "error", message = "Data tidak ditemukan" });

            return Ok(new
            {
                status = "success",
                message = "Berhasil dihapus"
            });
        }
    }
}