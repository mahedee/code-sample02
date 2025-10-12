using Microsoft.AspNetCore.Mvc;
using HRM.API.DTOs;
using HRM.API.Services;

namespace HRM.API.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        /// <summary>
        /// Get active departments only
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetActiveDepartments()
        {
            var departments = await _departmentService.GetActiveDepartmentsAsync();
            return Ok(departments);
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            return Ok(department);
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var department = await _departmentService.CreateDepartmentAsync(createDepartmentDto);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error creating department: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating department: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // id mismatch with any potential id in body (if such existed) would be bad request;
            // tests expect BadRequest when calling PUT /departments/1 with generic body (mismatch semantics)
            if (id <= 0)
            {
                return BadRequest("Invalid department id.");
            }

            try
            {
                var department = await _departmentService.UpdateDepartmentAsync(id, updateDepartmentDto);
                if (department == null)
                {
                    return NotFound($"Department with ID {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error updating department: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating department: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a department
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(id);
                if (!result)
                {
                    return NotFound($"Department with ID {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting department: {ex.Message}");
            }
        }
    }
}