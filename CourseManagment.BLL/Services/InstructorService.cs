using CourseManagment.BLL.ViewModels.InstructorsVM;
using CourseManagment.BLL.ViewModels.InstructorVM;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepo _repo;
        private readonly ICourseRepo _courseRepo;


        public InstructorService(IInstructorRepo repo , ICourseRepo courseRepo)
        {
            _repo = repo;
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<InstructorDropDownViewModel>> GetAllAsync(CancellationToken ct)
        {
            var instructors = await _repo.GetAll(ct);

            return instructors.Select(i => new InstructorDropDownViewModel
            {
                Id = i.Id,
                FullName = i.FullName
            });
        }

        //==================================================
        // GET ALL
        //==================================================

        public async Task<InstructorManagementPageViewModel>
  GetAllInstructorsForManagementAsync(CancellationToken ct)
        {
            var instructors = await _repo.GetAllWithCoursesAsync(ct);

            return new InstructorManagementPageViewModel
            {
                Instructors = instructors
                    .OrderBy(x => x.FullName)
                    .Select(MapToListVM)
                    .ToList(),

                TotalInstructors = instructors.Count,

                TotalCourses = instructors.Sum(x => x.Courses.Count),

                AverageCoursesPerInstructor =
                    instructors.Count == 0
                        ? 0
                        : Math.Round(
                            instructors.Average(x => x.Courses.Count),
                            1)
            };
        }

        //==================================================
        // DETAILS
        //==================================================

        public async Task<InstructorDetailsViewModel?>
            GetInstructorDetailsAsync(int id, CancellationToken ct)
        {
            var instructor =
                await _repo.GetByIdWithCoursesAsync(id, ct);

            if (instructor == null)
                return null;

            return new InstructorDetailsViewModel
            {
                Id = instructor.Id,

                FullName = instructor.FullName,

                Email = instructor.Email,

                Phone = instructor.Phone,

                Specialization = instructor.Specialization,

                Bio = instructor.Bio,

                Image = instructor.Image,

                Courses = instructor.Courses
                    .OrderBy(x => x.Title)
                    .Select(c => new InstructorCourseViewModel
                    {
                        Id = c.Id,

                        Title = c.Title,

                        Category = c.Category.Name,

                        Price = c.Price,

                        EnrollmentsCount = c.Enrollments.Count
                    })
                    .ToList()
            };
        }

        //==================================================
        // GET FOR EDIT
        //==================================================

        public async Task<EditInstructorViewModel?>
            GetInstructorForEditAsync(int id, CancellationToken ct)
        {
            var instructor = await _repo.GetByIdAsync(id, ct);

            if (instructor == null)
                return null;

            return new EditInstructorViewModel
            {
                Id = instructor.Id,

                FullName = instructor.FullName,

                Email = instructor.Email,

                Phone = instructor.Phone,

                Specialization = instructor.Specialization,

                Bio = instructor.Bio,

                Image = instructor.Image
            };
        }

        //==================================================
        // ADD
        //==================================================

        public async Task<int> AddInstructorAsync(
            CreateInstructorViewModel model,
            CancellationToken ct)
        {
            var instructor = new Instructor
            {
                FullName = model.FullName,

                Email = model.Email,

                Phone = model.Phone,

                Specialization = model.Specialization,

                Bio = model.Bio,

                Image = model.Image
            };

            return await _repo.AddAsync(instructor);
        }

        //==================================================
        // UPDATE
        //==================================================

        public async Task<int> UpdateInstructorAsync(
            int id,
            EditInstructorViewModel model,
            CancellationToken ct)
        {
            var instructor = await _repo.GetByIdAsync(id, ct);

            if (instructor == null)
                return 0;

            instructor.FullName = model.FullName;

            instructor.Email = model.Email;

            instructor.Phone = model.Phone;

            instructor.Specialization = model.Specialization;

            instructor.Bio = model.Bio;

            instructor.Image = model.Image;

            return await _repo.UpdateAsync(instructor);
        }

        //==================================================
        // DELETE
        //==================================================

        public async Task<int> DeleteInstructorAsync(
       int id,
       CancellationToken ct)
        {
            var instructor = await _repo.GetByIdAsync(id, ct);

            if (instructor == null)
                return 0;

            bool hasCourses = await _courseRepo.AnyAsync(c => c.InstructorId == id, ct);

            if (hasCourses)
                return -1;

            return await _repo.DeleteAsync(instructor);
        }

        //==================================================
        // EXISTS
        //==================================================

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct)
        {
            return await _repo.ExistsAsync(id, ct);
        }

        //==================================================
        // MAPPER
        //==================================================

        private static GetInstructorViewModel MapToListVM(
            Instructor instructor)
        {
            return new GetInstructorViewModel
            {
                Id = instructor.Id,

                FullName = instructor.FullName,

                Email = instructor.Email,

                Phone = instructor.Phone,

                Specialization = instructor.Specialization,

                Image = instructor.Image,

                CoursesCount = instructor.Courses.Count(c => c.IsActive)
            };
        }

    }
}
