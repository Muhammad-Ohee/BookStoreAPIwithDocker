
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Data;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private BookStoreContext _context;
        private IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            // Using Automapper
            var records = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(records);

            /*var records =await _context.Books.Select(x => new BookModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).ToListAsync();

            return records;*/
        }

        public async Task<BookModel?> GetBookByIdAsync(int bookId)
        {
            // Using Automapper
            var record = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(record);


            /*var record = await _context.Books.Where(x => x.Id == bookId)
                .Select(x => new BookModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).FirstOrDefaultAsync();

            return record;*/
        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books()
            {
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        /*public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            var book = new Books()
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            _context.Books.Update(book); // Updates a single row
            await _context.SaveChangesAsync();
        }*/

        public async Task<bool> UpdateBookAsync(int bookId, BookModel bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.Title = bookModel.Title;
                book.Description = bookModel.Description;

                await _context.SaveChangesAsync(); // Updates full database
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            
            if (book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            /*var book = new Books()
            {
                Id = bookId
            };

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();*/


            // If table does not have a primary key
            //var book = _context.Books.Where(x => x.Title == "").FirstOrDefault();


            var BookIdExist = await _context.Books.FindAsync(bookId);

            if (BookIdExist != null)
            {
                _context.Books.Remove(BookIdExist);

                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
