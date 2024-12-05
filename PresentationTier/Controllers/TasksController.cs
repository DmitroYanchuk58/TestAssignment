using ApplicationTier.Interfaces;
using ApplicationTier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTO;
using System.Security.Claims;
using Task = ApplicationTier.Models.Task;

namespace PresentationTier.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateTask([FromBody]TaskForCreate taskDto)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationTier.Models.Task task = new ApplicationTier.Models.Task()
                {
                    Description = taskDto.Description,
                    DueDate = taskDto.DueDate,
                    Priority = taskDto.Priority,
                    Status = taskDto.Status,
                    Title = taskDto.Title,
                };
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    _service.Create(task, userId);

                    return Created();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTasks([FromQuery]Filter filter, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    switch (filter.FilterItem)
                    {
                        case "Priority":
                            List<ApplicationTier.Models.Task> tasksPriority = new List<ApplicationTier.Models.Task>();
                            if (filter.Priority != null)
                            {
                                tasksPriority=_service.GetAllByPriority(filter.Priority, userId);
                            }
                            else
                            {
                                tasksPriority=_service.GetAll(userId);
                            }
                            return Json(tasksPriority);

                        case "Status":
                            List<ApplicationTier.Models.Task> tasksStatus = new List<ApplicationTier.Models.Task>();
                            if (filter.Priority != null)
                            {
                                tasksStatus = _service.GetAllByStatus(filter.Status, userId);
                            }
                            else
                            {
                                tasksStatus = _service.GetAll(userId);
                            }
                            return Json(tasksStatus);

                        case "DueDate":
                            List<ApplicationTier.Models.Task> tasksDueDate = new List<ApplicationTier.Models.Task>();
                            if (filter.Priority != null)
                            {
                                tasksDueDate = _service.GetTasksByDueDate(filter.DueDate, userId);
                            }
                            else
                            {
                                tasksDueDate = _service.GetAll(userId);
                            }
                            return Json(tasksDueDate);

                        case "SortOption":
                            List<ApplicationTier.Models.Task> taskSortOption = new List<ApplicationTier.Models.Task>();
                            if (filter.Priority != null)
                            {
                                taskSortOption = _service.GetAllSort(filter.SortOption, userId);
                            }
                            else
                            {
                                taskSortOption = _service.GetAll(userId);
                            }
                            return Json(taskSortOption);
                    }
                    return BadRequest();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return BadRequest(); 
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetTaskById(Guid id)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    var task = _service.Get(id,userId);
                    if (task == null) return NotFound();
                    return Ok(task);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch 
            { 
                return BadRequest(); 
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateTask(Guid id, [FromBody]TaskForUpdate taskDto)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    DataTier.Entities.Task task = new DataTier.Entities.Task()
                    {
                        Id = taskDto.Id,
                        Description = taskDto.Description,
                        DueDate = taskDto.DueDate,
                        Title = taskDto.Title
                    };
                    _service.Update(task, userId);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    _service.Delete(id, userId);
                    return NoContent();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
