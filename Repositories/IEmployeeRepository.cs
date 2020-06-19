using System.Collections.Generic;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();

        Employee GetEmployee(int id);

        int CreateEmployee(Employee emp);

        int UpdateEmployee(int id, Employee emp);

        int DeleteEmployee(int id);
    }
}