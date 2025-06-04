using AuthDemos.Core.DTOs.Author;
using AuthDemos.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Infrastructure.BusinessLogic
{
    public interface IAuthorBusinessLogic
    {
        Task<ResponseDTO> CreateAuthor(CreateAuthorDTO createAuthorDTO);
        Task<ResponseDTO> CreateBooks(CreateBookDTO createBookDTO);
        Task<ResponseDTO> CreateCategory(CreateCategoryDTO createCategory);
        Task<ResponseDTO> GetAllAuthors();
        Task<ResponseDTO> GetAllBooks();
        Task<ResponseDTO> GetAllCategories();
        Task<ResponseDTO> UpdateAuthor(UpdateAuthorDTO updateAuthorDTO);
        Task<ResponseDTO> DeleteBooks(int bookId);
    }
}
