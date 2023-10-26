using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CRUDWithADONet.DAL;
using CRUDWithADONet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWithADONet.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Employee_DAL _dal;

        public EmployeeController(Employee_DAL dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            try
            {

                employees = _dal.GetAll();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(employees);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Employee model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                }
                bool result = _dal.Insert(model);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save the data";
                    return View();
                }
                TempData["successMessage"] = "Employee details saved";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(Int64 id)
         {
            //Employee employee = new Employee();
            List<Employee> employeeList = new List<Employee>();

            try
            {
                DataTable dt = new DataTable();

                dt = _dal.GetRecordsById("employee_records", id);


                //dt = GetRecordsbyid("get_employees_by_id");

                foreach (DataRow dr in dt.Rows)
                {
                    employeeList.Add(
                    new Employee
                    {
                        id = Convert.ToInt64(dr["id"]),
                        first_name = Convert.ToString(dr["first_name"]),
                        last_name = Convert.ToString(dr["last_name"]),
                        date_of_birth = Convert.ToDateTime(dr["date_of_birth"]),
                        email = Convert.ToString(dr["email"]),
                        salary = Convert.ToDouble(dr["salary"])
                    });
                }

                Employee existingemployee = employeeList.Find(mymodel => mymodel.id == id)!;

                Employee employee = new Employee
                {
                    id = existingemployee.id,
                    first_name = existingemployee.first_name,
                    last_name = existingemployee.last_name,
                    date_of_birth = existingemployee.date_of_birth,
                    email = existingemployee.email,
                    salary = existingemployee.salary 
                    
                };
                dt = _dal.GetRecordsById("get_records_by_id", id);
                if(employee.id == 0)
                {
                    TempData["errorMessage"] = $"Employee details not found with Id : {id}";
                    return RedirectToAction("Index");
                }
                return View(employee);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }
            
            //return employeeList;
        }



        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Employee model)
        {

            try
            {
                
                bool result = _dal.Delete(model.id);

                
                    if (!result)
                    {
                        TempData["errorMessage"] = "Unable to delete the data";
                        return View();
                    }
                    TempData["successMessage"] = "Employee details deleted";
                    return RedirectToAction("Index");
                

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }
            
            //return employeeList;
        }

        [HttpGet]
        public IActionResult Edit(Int64 id)
        {
            //Employee employee = new Employee();
            List<Employee> employeeList = new List<Employee>();

            try
            {
                DataTable dt = new DataTable();

                dt = _dal.GetRecordsById("employee_records", id);


                //dt = GetRecordsbyid("get_employees_by_id");

                foreach (DataRow dr in dt.Rows)
                {
                    employeeList.Add(
                    new Employee
                    {
                        id = Convert.ToInt64(dr["id"]),
                        first_name = Convert.ToString(dr["first_name"]),
                        last_name = Convert.ToString(dr["last_name"]),
                        date_of_birth = Convert.ToDateTime(dr["date_of_birth"]),
                        email = Convert.ToString(dr["email"]),
                        salary = Convert.ToDouble(dr["salary"])
                    });
                }

                Employee existingemployee = employeeList.Find(mymodel => mymodel.id == id)!;

                Employee employee = new Employee
                {
                    id = existingemployee.id,
                    first_name = existingemployee.first_name,
                    last_name = existingemployee.last_name,
                    date_of_birth = existingemployee.date_of_birth,
                    email = existingemployee.email,
                    salary = existingemployee.salary

                };
                dt = _dal.GetRecordsById("get_records_by_id", id);
                if (employee.id == 0)
                {
                    TempData["errorMessage"] = $"Employee details not found with Id : {id}";
                    return RedirectToAction("Index");
                }
                return View(employee);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            //return employeeList;
        }



        [HttpPost]
        public IActionResult Edit(Employee model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();

                }
                bool result = _dal.Update(model);


                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update the data";
                    return View();
                }
                TempData["successMessage"] = "Employee details updated";
                return RedirectToAction("Index");


            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            //return employeeList;
        }

    }
}