using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Exam;
using SMSystem_Api.Model.Fees;
using SMSystem_Api.Repository.Interfaces;
using System.Data;

namespace SMSystem_Api.Repository
{
    public class ExamApiRepository : IExamApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public ExamApiRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.Configuration = configuration;
        }

        public List<ExamModel> GetAllExams()
        {
            var Data = context.Exams.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedExamModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            string sp = StoredProcedureHelper.ExamPaging;

            var parameters = new DynamicParameters();

            parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
            parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
            parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = new SqlConnection(connaction))
            {
                con.Open();
                var exams = (await con.QueryAsync<ExamModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                var paggedExam = new PaggedExamModel()
                {
                    ExamModel = exams,
                    PaggedModel = new PaggedModel()
                    {
                        PageIndex = para.PageIndex,
                        TotalPage = totalPage
                    }
                };
                return paggedExam;
            }
        }

        public async Task<ExamModel> Get(int id)
        {
            var exam = await context.Exams.FindAsync(id).ConfigureAwait(false);
            return exam;
        }

        public async Task Add(ExamModel exam)
        {
            await context.Exams.AddAsync(exam).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(ExamModel exam)
        {
            context.Attach(exam).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var exam = context.Exams.Find(id);
            exam.IsDelete = true;
            exam.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
