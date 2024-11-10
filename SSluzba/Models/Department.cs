using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSluzba.Models
{
    public class Department : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _departmentCode;
        public string DepartmentCode
        {
            get => _departmentCode;
            set
            {
                if (value != _departmentCode)
                {
                    _departmentCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                if (value != _departmentName)
                {
                    _departmentName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _headOfDepartmentId;
        public int HeadOfDepartmentId
        {
            get => _headOfDepartmentId;
            set
            {
                if (value != _headOfDepartmentId)
                {
                    _headOfDepartmentId = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<int> _professorIdList;
        public List<int> ProfessorIdList
        {
            get => _professorIdList ??= new List<int>();
            set
            {
                if (value != _professorIdList)
                {
                    _professorIdList = value;
                    OnPropertyChanged();
                }
            }
        }

        public Department() { }

        public Department(int id, string departmentCode, string departmentName, int headOfDepartmentId, List<int> professorIdList)
        {
            Id = id;
            DepartmentCode = departmentCode;
            DepartmentName = departmentName;
            HeadOfDepartmentId = headOfDepartmentId;
            ProfessorIdList = professorIdList;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                DepartmentCode,
                DepartmentName,
                HeadOfDepartmentId.ToString(),
                string.Join(",", ProfessorIdList)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            DepartmentCode = values[1];
            DepartmentName = values[2];
            HeadOfDepartmentId = int.Parse(values[3]);
            ProfessorIdList = new List<int>(Array.ConvertAll(values[4].Split(','), int.Parse));
        }

        public override string ToString()
        {
            return $"ID: {Id}, Department Code: {DepartmentCode}, Department Name: {DepartmentName}, Head of Department ID: {HeadOfDepartmentId}, Professors: {string.Join(", ", ProfessorIdList)}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
