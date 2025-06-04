using AuthDemo.Infrastructure.BusinessLogic;
using AuthDemos.Core.DTOs.Author;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController(IAuthorBusinessLogic authorBusinessLogic)
        : ControllerBase
    {

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            var response = await authorBusinessLogic.CreateAuthor(createAuthorDTO);
            return Ok(response);
        }

        [HttpPost]
        [ActionName("CreateBooks")]
        public async Task<IActionResult> CreateBooks(CreateBookDTO createBookDTO)
        {
            var response = await authorBusinessLogic.CreateBooks(createBookDTO);
            return Ok(response);
        }

        [HttpPost]
        [ActionName("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var response = await authorBusinessLogic.CreateCategory(createCategoryDTO);
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var response = await authorBusinessLogic.GetAllAuthors();
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await authorBusinessLogic.GetAllBooks();
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await authorBusinessLogic.GetAllCategories();
            return Ok(response);
        }

        [HttpPut]
        [ActionName("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(UpdateAuthorDTO requestDTO)
        {
            var response = await authorBusinessLogic.UpdateAuthor(requestDTO);
            return Ok(response);
        }


        [HttpDelete("{Id}")]
        [ActionName("DeleteBook")]
        public async Task<IActionResult> DeleteBook(int Id)
        {
            var response = await authorBusinessLogic.DeleteBooks(Id);
            return Ok(response);
        }

    }
}
