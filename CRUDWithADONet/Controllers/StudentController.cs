using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CRUDWithADONet.DAL;
using CRUDWithADONet.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDWithADONet.Controllers
{
    public class StudentController : Controller
    {
        private readonly Employee_DAL _dal;

        public StudentController(Employee_DAL dal)
        {
            _dal = dal;
        }
        // method for student view
        public IActionResult StudentView()
        {

            List<Student> students = new List<Student>();
            try
            {

                students = _dal.GetStudents();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(students);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Student model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                }
                bool result = _dal.InsertStudent(model);

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to save the data";
                    return View();
                }
                TempData["successMessage"] = "Student details saved";
                return RedirectToAction("StudentView");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public IActionResult Edit(Int64 id)
        {
            //Student student = new Student();
            List<Student> studentList = new List<Student>();

            try
            {
                DataTable dt = new DataTable();

                dt = _dal.GetRecordsById("student_records_by_id", id);


                //dt = GetRecordsbyid("student_records_by_id");

                foreach (DataRow dr in dt.Rows)
                {
                    studentList.Add(
                    new Student
                    {
                        id = Convert.ToInt64(dr["id"]),
                        first_name = Convert.ToString(dr["first_name"]),
                        middle_name = Convert.ToString(dr["middle_name"]),
                        last_name = Convert.ToString(dr["last_name"]),
                        date_of_birth = Convert.ToDateTime(dr["date_of_birth"]),
                        course_name = Convert.ToString(dr["course_name"]),
                        
                    });
                }

                Student existingstudent = studentList.Find(mymodel => mymodel.id == id)!;

                Student student = new Student
                {
                    id = existingstudent.id,
                    first_name = existingstudent.first_name,
                    middle_name = existingstudent.middle_name,
                    last_name = existingstudent.last_name,
                    date_of_birth = existingstudent.date_of_birth,
                    course_name = existingstudent.course_name,
                    

                };
                dt = _dal.GetRecordsById("get_records_by_id", id);
                if (student.id == 0)
                {
                    TempData["errorMessage"] = $"Student details not found with Id : {id}";
                    return RedirectToAction("StudentView");
                }
                return View(student);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            //return studentList;
        }



        [HttpPost]
        public IActionResult Edit(Student model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();

                }
                bool result = _dal.UpdateStudent(model);


                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update the data";
                    return View();
                }
                TempData["successMessage"] = "Student details updated";
                return RedirectToAction("StudentView");


            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            //return studentList;
        }
        [HttpGet]
        public IActionResult Delete(Int64 id)
        {
            //Students students = new Student();
            List<Student> studentList = new List<Student>();

            try
            {
                DataTable dt = new DataTable();

                dt = _dal.GetRecordsById("student_records_by_id", id);


                //dt = GetRecordsbyid("student_records_by_id");

                foreach (DataRow dr in dt.Rows)
                {
                    studentList.Add(
                    new Student
                    {
                        id = Convert.ToInt64(dr["id"]),
                        first_name = Convert.ToString(dr["first_name"]),
                        middle_name = Convert.ToString(dr["middle_name"]),
                        last_name = Convert.ToString(dr["last_name"]),
                        date_of_birth = Convert.ToDateTime(dr["date_of_birth"]),
                        course_name = Convert.ToString(dr["course_name"]),
                    });
                }

                Student existingstudent = studentList.Find(mymodel => mymodel.id == id)!;

                Student student = new Student
                {
                    id = existingstudent.id,
                    first_name = existingstudent.first_name,
                    middle_name = existingstudent.middle_name,
                    last_name = existingstudent.last_name,
                    date_of_birth = existingstudent.date_of_birth,
                    course_name = existingstudent.course_name,


                };
                dt = _dal.GetRecordsById("student_records_by_id", id);
                if (student.id == 0)
                {
                    TempData["errorMessage"] = $"Student details not found with Id : {id}";
                    return RedirectToAction("StudentView");
                }
                return View(student);

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
        public IActionResult DeleteConfirmed(Student model)
        {

            try
            {

                bool result = _dal.Delete(model.id);


                if (!result)
                {
                    TempData["errorMessage"] = "Unable to delete the data";
                    return View();
                }
                TempData["successMessage"] = "Student details deleted";
                return RedirectToAction("StudentView");


            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
                //logger.Error(ex);
                //FileLogHelper.log_message_fields("ERROR", "GetClients | Exception ->" + ex.Message);
            }

            //return studentList;
        }
        
    }
    
}