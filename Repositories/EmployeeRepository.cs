using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NetCoreAPI.ApplicationContext;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public int CreateEmployee(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return emp.EmpId;
        }

        public int DeleteEmployee(int id)
        {
            var employee = _context.Employees.SingleOrDefault(e => e.EmpId == id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                return _context.SaveChanges();
            }

            return -1;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.SingleOrDefault(e => e.EmpId == id);
        }

        public int UpdateEmployee(int id, Employee emp)
        {
            var empDb = _context.Employees.SingleOrDefault(e => e.EmpId == id);

            if (empDb != null)
            {
                empDb.EmpName = emp.EmpName;
                empDb.EmpContact = emp.EmpContact;
                empDb.EmpAddress = emp.EmpAddress;
                empDb.EmpEmail = emp.EmpEmail;

                _context.Employees.Update(empDb);
                return _context.SaveChanges();
            }
            
            return -1;
        }
    }
}