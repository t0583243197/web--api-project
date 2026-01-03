using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryBLL _categoryBll;
    public CategoryController(ICategoryBLL categoryBll) => _categoryBll = categoryBll;

    [HttpGet] // פתוח לכולם לצפייה
    public async Task<IActionResult> Get() => Ok(await _categoryBll.GetAllCategories());

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CategoryDTO category)
    {
        await _categoryBll.AddCategory(category);
        return Ok("Category added successfully");
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryDTO category)
    {
        await _categoryBll.UpdateCategory(category);
        return Ok("Category updated successfully");
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryBll.DeleteCategory(id);
        return Ok("Category deleted successfully");
    }
}