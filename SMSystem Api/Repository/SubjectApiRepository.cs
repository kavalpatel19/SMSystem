using Microsoft.Data.SqlClient;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Subjects;
using System.Data;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Repository.Interfaces;
using Dapper;

namespace SMSystem_Api.Repository
{
    public class SubjectApiRepository : ISubjectApiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration Configuration;

        public SubjectApiRepository(ApplicationDbContext context,IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public List<SubjectModel> GetAllSubjects()
        {
            var Data = context.Subjects.Where(x => x.IsActive).ToList();
            return Data;
        }

        public async Task<PaggedSubjectModel> GetAll(SearchingPara para)
        {
            string connaction = Configuration.GetConnectionString("connaction");

            string sp = StoredProcedureHelper.SubjetSearchingPaging;

            var parameters = new DynamicParameters();

            parameters.Add(FieldHelper.SID, para.SId, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.Name, para.Name, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.Class, para.Class, dbType: DbType.String, size: 50);
            parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
            parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
            parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var con = new SqlConnection(connaction))
            {
                con.Open();
                var subjects = (await con.QueryAsync<SubjectModel>( sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                var paggedSubject = new PaggedSubjectModel()
                {
                    SubjectModel = subjects,
                    PaggedModel = new PaggedModel()
                    {
                        PageIndex = para.PageIndex,
                        TotalPage = totalPage
                    }
                };

                return paggedSubject;
            }
        }

        public async Task<SubjectModel> Get(int id)
        {
            var subject = await context.Subjects.FindAsync(id).ConfigureAwait(false);
            return subject;
        }

        public async Task Add(SubjectModel subject)
        {
            await context.Subjects.AddAsync(subject).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(SubjectModel subject)
        {
            context.Attach(subject).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var subject = context.Subjects.Find(id);
            subject.IsDelete = true;
            subject.IsActive = false;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
