using System.Collections.Generic;
using System.Linq;
using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Controllers
{
    public class IndexController
    {
        private IndexDAO _indexDAO;

        public IndexController()
        {
            _indexDAO = new IndexDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _indexDAO.Subscribe(observer);
        }

        public void AddIndex(string majorCode, int enrollmentNumber, int enrollmentYear)
        {
            var newIndex = new Models.Index
            {
                MajorCode = majorCode,
                EnrollmentNumber = enrollmentNumber,
                EnrollmentYear = enrollmentYear
            };

            _indexDAO.Add(newIndex);
        }

        public void UpdateIndex(Models.Index updatedIndex)
        {
            _indexDAO.Update(updatedIndex);
        }

        public void DeleteIndex(int id)
        {
            var index = _indexDAO.GetAll().FirstOrDefault(i => i.Id == id);
            if (index != null)
            {
                _indexDAO.Remove(index);
            }
        }

        public List<Models.Index> GetAllIndices()
        {
            return _indexDAO.GetAll();
        }
        public int GetIndexId(string majorCode, int enrollmentNumber, int enrollmentYear)
        {
            return _indexDAO.GetIndexId(majorCode, enrollmentNumber, enrollmentYear);
        }

        public Models.Index GetIndexById(int id)
        {
            return _indexDAO.GetAll().FirstOrDefault(index => index.Id == id);
        }

    }
}
