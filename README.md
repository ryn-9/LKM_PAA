Nama	: M. Bintang Royyan
NIM	: 242410102040
Kelas	: PAA A


Perpustakaan API

Deskripsi

Perpustakaan API adalah aplikasi berbasis ASP.NET Core Web API yang digunakan untuk mengelola data pengguna, buku, dan peminjaman. API ini mendukung operasi CRUD (Create, Read, Update, Delete) serta menerapkan soft delete untuk menjaga integritas data.

---

Teknologi yang Digunakan

* ASP.NET Core Web API
* PostgreSQL
* Swagger
* SQL Query

---

Struktur Database

1. Users

* id (Primary Key)
* name
* email
* created_at
* updated_at
* deleted_at

2. Books

* id (Primary Key)
* title
* author
* stock
* created_at
* updated_at
* deleted_at

3. Borrowings

* id (Primary Key)
* user_id (Foreign Key)
* book_id (Foreign Key)
* borrow_date
* return_date
* created_at
* updated_at
* deleted_at

---

Cara Menjalankan Project

1. Clone repository:

   git clone https://github.com/ryn-9/LKM_PAA.git

2. Import database:

   * Buka PostgreSQL
   * Jalankan file `dbPerpus.sql`

3. Konfigurasi koneksi database:

   * Buka file `SqlDbHelper`
   * Sesuaikan connection string:

     ```
     Host=localhost;Username=postgres;Password=[Password];Database=dbPerpus
     ```

4. Jalankan project:

   * Buka di Visual Studio 2022
   * Klik Run

5. Akses Swagger:

   https://localhost:xxxx/swagger

---

Endpoint API

Users

| Method | Endpoint        | Deskripsi                          |
| ------ | --------------- | ---------------------------------- |
| GET    | /api/users      | Mengambil seluruh data user        |
| GET    | /api/users/{id} | Mengambil data user berdasarkan ID |
| POST   | /api/users      | Menambahkan data user              |
| PUT    | /api/users/{id} | Memperbarui data user              |
| DELETE | /api/users/{id} | Menghapus data user (soft delete)  |

---

Books

| Method | Endpoint        | Deskripsi                          |
| ------ | --------------- | ---------------------------------- |
| GET    | /api/books      | Mengambil seluruh data buku        |
| GET    | /api/books/{id} | Mengambil data buku berdasarkan ID |
| POST   | /api/books      | Menambahkan data buku              |
| PUT    | /api/books/{id} | Memperbarui data buku              |
| DELETE | /api/books/{id} | Menghapus data buku (soft delete)  |

---

Borrowings

| Method | Endpoint             | Deskripsi                                |
| ------ | -------------------- | ---------------------------------------- |
| GET    | /api/borrowings      | Mengambil seluruh data peminjaman        |
| GET    | /api/borrowings/{id} | Mengambil detail peminjaman              |
| POST   | /api/borrowings      | Menambahkan data peminjaman              |
| PUT    | /api/borrowings/{id} | Memperbarui data peminjaman              |
| DELETE | /api/borrowings/{id} | Menghapus data peminjaman (soft delete)  |

---

Fitur Utama

* Implementasi RESTful API
* Operasi CRUD pada seluruh entitas
* Soft delete menggunakan kolom deleted_at
* Relasi antar tabel menggunakan foreign key
* Dokumentasi dan pengujian API menggunakan Swagger

---

Contoh Request (POST Books)

```json
{
  "title": "Laskar Pelangi",
  "author": "Andrea Hirata",
  "stock": 5
}
```

---

Contoh Response

```json
{
  "status": "success",
  "data": [
    {
      "id": 1,
      "title": "Laskar Pelangi",
      "author": "Andrea Hirata",
      "stock": 5
    }
  ]
}
```

---

Kesimpulan

Aplikasi ini mengimplementasikan RESTful API dengan struktur database relasional, mendukung operasi CRUD, serta menggunakan soft delete untuk menjaga konsistensi data. Dokumentasi API disediakan melalui Swagger untuk memudahkan pengujian.

---
