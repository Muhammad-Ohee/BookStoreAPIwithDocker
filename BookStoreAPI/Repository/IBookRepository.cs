using BookStoreAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreAPI.Repository
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAllBooksAsync();

        Task<BookModel?> GetBookByIdAsync(int bookId);

        Task<int> AddBookAsync(BookModel bookModel);

        //Task UpdateBookAsync(int bookId, BookModel bookModel);
        //public async Task UpdateBookAsync(int bookId, BookModel bookModel);

        Task<bool> UpdateBookAsync(int bookId, BookModel bookModel);

        Task<bool> UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel);

        Task<bool> DeleteBookAsync(int bookId);
    }
}
