using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

[ApiController]
[Route("students")]
public class StudentController : ControllerBase
{
    private readonly SchoolContext _context;

    public StudentController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public StudentDto Get(long id)
    {
        Student student = _context.Students
            .Include(x => x.Enrollments)
            .ThenInclude(x => x.Course)
            .SingleOrDefault(x => x.Id == id);
        if (student is null)
            return null;

        return new StudentDto
        {
            StudentId = student.Id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = student.Enrollments.Select(x => new EnrollmentDto
            {
                Course = x.Course.Name,
                Grade = x.Grade.ToString()
            }).ToList()
        };
    }
}
