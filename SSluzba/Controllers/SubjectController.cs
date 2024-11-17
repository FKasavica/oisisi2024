using SSluzba.DAO;
using SSluzba.Models;
using System.Collections.Generic;

namespace SSluzba.Controllers
{
    public class SubjectController
    {
        private readonly SubjectDAO _subjectDAO = new();

        public List<Subject> GetAllSubjects()
        {
            return _subjectDAO.GetAll();
        }
    }
}
