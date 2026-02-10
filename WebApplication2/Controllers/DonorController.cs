using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.BLL;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorBLL _donorBll;

        public DonorController(IDonorBLL donorBll) => _donorBll = donorBll;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _donorBll.GetAllDonorsAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? giftName)
        {
            var results = await _donorBll.GetDonorsByFilterAsync(name, email, giftName);
            return Ok(results);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateDonorDTO createDonor)
        {
            var donor = new DonorDTO
            {
                Name = createDonor.Name,
                Email = createDonor.Email,
                Address = createDonor.Address
            };
            await _donorBll.AddDonorAsync(donor);
            return Ok(new { message = "התורם נוסף בהצלחה!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DonorDTO donor)
        {
            donor.Id = id;
            await _donorBll.UpdateDonorAsync(donor);
            return Ok(new { message = "התורם עודכן בהצלחה!" });
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _donorBll.DeleteDonorAsync(id);
            return Ok(new { message = "התורם נמחק בהצלחה!" });
        }
    }
}