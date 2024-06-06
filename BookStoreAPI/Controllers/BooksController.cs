using BookStoreAPI.Models;
using BookStoreAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute]int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            
            return Ok(book);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookModel bookModel)
        {
            var id = await _bookRepository.AddBookAsync(bookModel);

            var newBookObject = new BookModel()
            {
                Id = id,
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            //return Ok(id);
            return CreatedAtAction(nameof(GetBookById), 
                new { id = id, Controller = "Books" }, newBookObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookModel bookModel)
        {
            var result = await _bookRepository.UpdateBookAsync(id, bookModel);

            var newBookObject = new BookModel()
            {
                Id = id,
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            if (result)
            {
                return Ok(newBookObject);
            }
            return Ok($"Book ID {id} not found and not updated");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookPatch([FromRoute] int id, [FromBody] JsonPatchDocument bookModel)
        {
            var result = await _bookRepository.UpdateBookPatchAsync(id, bookModel);

            if (result)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute]int id)
        {
            if (await _bookRepository.DeleteBookAsync(id))
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
