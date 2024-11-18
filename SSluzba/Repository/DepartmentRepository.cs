using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class DepartmentRepository
    {
        private readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "departments.csv");

        public List<Department> LoadDepartments()
        {
            List<Department> departments = new List<Department>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Department department = new Department();
                    department.FromCSV(values);
                    departments.Add(department);
                }
            }
            return departments;
        }

        public void SaveDepartments(List<Department> departments)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var department in departments)
                    {
                        sw.WriteLine(string.Join(",", department.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving departments: {ex.Message}");
            }
        }
    }
}
