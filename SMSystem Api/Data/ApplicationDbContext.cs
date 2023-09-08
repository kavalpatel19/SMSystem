using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Auth;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Model.Holiday;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Model.Subjects;
using SMSystem_Api.Model.Teachers;
using System.Security.Claims;

namespace SMSystem_Api.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option) { }

        //
        //
        // To Get a log of "Created Date" and "Modified Date". //
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntityModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                ((BaseEntityModel)entity.Entity).ModifiedDate = DateTime.Now;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntityModel)entity.Entity).CreatedDate = DateTime.Now;                    
                }           
            }
            return base.SaveChanges();
        }

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<TeacherModel> Teachers { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<HolidayModel>  Holidays { get; set; }
        public DbSet<FeesModel> Fees { get; set; }
        public DbSet<ExamModel> Exams { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
    }
}
