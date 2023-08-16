﻿using SMSystem_Api.Model.Students;

namespace SMSystem_Api.Model.Subjects
{
    public class PaggedSubjectModel
    {
        public PaggedSubjectModel()
        {
            SubjectModel = new List<SubjectModel>();
            PaggedModel = new PaggedModel();
        }
        public IList<SubjectModel> SubjectModel { get; set; }
        public PaggedModel PaggedModel { get; set; }
    }
}
