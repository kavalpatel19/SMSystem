using Dapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMSystem_Api.Data;
using SMSystem_Api.Helpers;
using SMSystem_Api.Migrations;
using SMSystem_Api.Model;
using SMSystem_Api.Model.Department;
using SMSystem_Api.Model.Students;
using SMSystem_Api.Repository.Interfaces;
using System.Data;

namespace SMSystem_Api.Repository
{
    public class DepartmentApiRepository : IDepartmentApiRepository
    {
        private readonly ApplicationDbContext context;
        public readonly IConfiguration Configuration;

        public DepartmentApiRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }

        public BaseResponseModel<DepartmentModel> GetAllDepartments()
        {
            var response = new BaseResponseModel<DepartmentModel>();

            try
            {
                var data = context.Departments.Where(x => x.IsActive).ToList();
                if (data.Count == 0)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Results = new List<DepartmentModel>();
                    return response;
                }
                response.ResponseCode = 200;
                response.Results = data;
                return response;

            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Results = new List<DepartmentModel>();
                return response;
            }
        }

        public async Task<BaseResponseModel<PaggedDepartmentModel>> GetAll(SearchingPara para)
        {
            var response = new BaseResponseModel<PaggedDepartmentModel>();

            try
            {

                string connaction = Configuration.GetConnectionString("connaction");

                string sp = StoredProcedureHelper.DepartmentSearchingPaging;

                var parameters = new DynamicParameters();

                parameters.Add(FieldHelper.SID, para.SId, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.Name, para.Name, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.Year, para.Year, dbType: DbType.String, size: 50);
                parameters.Add(FieldHelper.PageIndex, para.PageIndex, dbType: DbType.Int32);
                parameters.Add(FieldHelper.PageSize, para.pagesize, dbType: DbType.Int32);
                parameters.Add(FieldHelper.TotalPages, dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = new SqlConnection(connaction))
                {
                    con.Open();
                    var departments = (await con.QueryAsync<DepartmentModel>(sp, parameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    var totalPage = parameters.Get<int>(FieldHelper.TotalPages);

                    var paggedDepartment = new PaggedDepartmentModel()
                    {
                        DepartmentModel = departments,
                        PaggedModel = new PaggedModel()
                        {
                            PageIndex = para.PageIndex,
                            TotalPage = totalPage
                        }
                    };

                    response.ResponseCode = 200;
                    response.Result = paggedDepartment;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new PaggedDepartmentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<DepartmentModel>> Get(int id)
        {
            var response = new BaseResponseModel<DepartmentModel>();

            try
            {
                var department = await context.Departments.FindAsync(id).ConfigureAwait(false);

                if (department == null)
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not Found!";
                    response.Result = new DepartmentModel();
                    return response;
                }

                response.ResponseCode = 200;
                response.Result = department;
                return response;
            }
            catch(Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new DepartmentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<DepartmentModel>> Add(DepartmentModel department)
        {
            var response = new BaseResponseModel<DepartmentModel>();
            try
            {
                await context.Departments.AddAsync(department).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new DepartmentModel();
                return response;
            }
            catch(Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new DepartmentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<DepartmentModel>> Update(DepartmentModel department)
        {
            var response = new BaseResponseModel<DepartmentModel>();
            try
            {
                context.Attach(department).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new DepartmentModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new DepartmentModel();
                return response;
            }
        }

        public async Task<BaseResponseModel<DepartmentModel>> Delete(int id)
        {
            var response = new BaseResponseModel<DepartmentModel>();
            try
            {
                var department = context.Departments.Find(id);
                department.IsDelete = true;
                department.IsActive = false;
                await context.SaveChangesAsync().ConfigureAwait(false);

                response.ResponseCode = 200;
                response.Result = new DepartmentModel();
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
                response.Result = new DepartmentModel();
                return response;
            }
        }
    }
}
