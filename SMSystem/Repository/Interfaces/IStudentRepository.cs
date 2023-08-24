﻿using SMSystem.Helpers;
using SMSystem.Models.Holiday;
using SMSystem.Models;
using SMSystem.Models.Students;

namespace SMSystem.Repository.Interfaces
{
    public interface IStudentRepository
    {
        BaseResponseViewModel<StudentViewModel> GetAllStudents();
        Task<BaseResponseViewModel<StudentPagedViewModel>> GetStudents(SearchingParaModel para);
        Task<BaseResponseViewModel<StudentViewModel>> GetStudent(int id);
        Task<BaseResponseViewModel<StudentViewModel>> Add(StudentViewModel student);
        Task<BaseResponseViewModel<StudentViewModel>> Update(StudentViewModel student);
        Task<BaseResponseViewModel<StudentViewModel>> Delete(int id);
    }
}
