using LKM_PAA.Helpers;
using LKM_PAA.Models;
using Microsoft.AspNetCore.Mvc;

namespace LKM_PAA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SqlDbHelper _db;

        public UsersController()
        {
            _db = new SqlDbHelper("Host=localhost;Username=postgres;Password=babamamak;Database=dbPerpus");
        }

        // GET ALL USERS
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _db.ExecuteQuery("SELECT * FROM users WHERE deleted_at IS NULL ORDER BY id ASC");

            return Ok(new
            {
                status = "success",
                data
            });
        }

        // GET USER BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _db.ExecuteQuery(
                "SELECT * FROM users WHERE id = @id AND deleted_at IS NULL",
                new Dictionary<string, object> { { "@id", id } }
            );

            if (data.Count == 0)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "User tidak ditemukan"
                });
            }

            return Ok(new
            {
                status = "success",
                data = data[0]
            });
        }

        // CREATE USER
        [HttpPost]
        public IActionResult Create([FromBody] User input)
        {
            if (input.name == null || input.email == null)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Name dan Email wajib diisi"
                });
            }

            _db.ExecuteNonQuery(
                "INSERT INTO users (name, email) VALUES (@name, @email)",
                new Dictionary<string, object>
                {
                    { "@name", input.name },
                    { "@email", input.email }
                }
            );

            return Ok(new
            {
                status = "success",
                message = "User berhasil ditambahkan"
            });
        }

        // UPDATE USER
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User input)
        {
            var result = _db.ExecuteNonQuery(
                "UPDATE users SET name = @name, email = @email, updated_at = CURRENT_TIMESTAMP WHERE id = @id",
                new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@name", (string)input.name },
                    { "@email", (string)input.email }
                }
            );

            if (result == 0)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "User tidak ditemukan"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User berhasil diupdate"
            });
        }

        // DELETE USER
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _db.ExecuteNonQuery(
                "DELETE FROM users WHERE id = @id",
                new Dictionary<string, object> { { "@id", id } }
            );

            if (result == 0)
            {
                return NotFound(new
                {
                    status = "error",
                    message = "User tidak ditemukan"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User berhasil dihapus"
            });
        }
    }
}