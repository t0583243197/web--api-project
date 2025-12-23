using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.DTO;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryBLL _categoryBll;
    public CategoryController(ICategoryBLL categoryBll) => _categoryBll = categoryBll;

    [HttpGet] // פתוח לכולם לצפייה
    public IActionResult Get() => Ok(_categoryBll.GetAllCategories());

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpPost]
    public IActionResult Post([FromBody] CategoryDTO category)
    {
        _categoryBll.AddCategory(category);
        return Ok("Category added successfully");
    }

    [Authorize(Roles = "Manager")]
    [HttpPut]
    public IActionResult Update([FromBody] CategoryDTO category)
    {
        _categoryBll.UpdateCategory(category);
        return Ok("Category updated successfully");
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _categoryBll.DeleteCategory(id);
        return Ok("Category deleted successfully");
    }
}