using EIS.BOL;
using EIS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIS.BLL
{
    public class EmployeeBs
    {
        private EmployeeDb ObjDb;

        public List<string> Errors = new List<string>();

        public EmployeeBs()
        {
            ObjDb = new EmployeeDb();
        }
        public IEnumerable<Employee> GetALL()
        {
            return ObjDb.GetALL();
        }
        public Employee GetByID(string Id)
        {
            return ObjDb.GetByID(Id);
        }

        public bool Insert(Employee emp)
        {
            if (IsValidOnInsert(emp))
            {
                ObjDb.Insert(emp);
                string subject = "Your Login Credentials On EIS";
                string body = "User Name : " + emp.Email + "\n" +
                             "Password : " + emp.Password + "\n" +
                             "Login Here : http:\\\\empManagementSystem.com\\login" + "\n" +
                             "Regards," + "\n" +
                             "empManagementSystem.com";
                Utility.SendEmail(emp.Email, subject, body);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete(string Id)
        {
            ObjDb.Delete(Id);
        }
        public bool Update(Employee emp)
        {
            if (IsValidOnUpdate(emp))
            {
                ObjDb.Update(emp);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetByEmail(ref Employee emp)
        {
            try
            {
                var employee = ObjDb.GetByEmail(emp.Email);
                if (employee == null)
                {
                    Errors.Add("Email id Does not Exist");
                }
                else if (employee.Password != emp.Password)
                {
                    Errors.Add("Invalid Password");
                }

                if (Errors.Count() == 0)
                {
                    emp = employee;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool RecoverPasswordByEmail(ref Employee emp)
        {
            var employee = ObjDb.GetByEmail(emp.Email);
            if (employee == null)
            {
                Errors.Add("Email id Does not Exist");
            }
            else
            {
                string subject = "Your Login Credentials On EIS";
                string body = "User Name : " + emp.Email + "\n" +
                             "Password : " + emp.Password + "\n" +
                             "Login Here : http:\\\\empManagementSystem.com\\login" + "\n" +
                             "Regards," + "\n" +
                             "www.empManagementSystem.com";
                Utility.SendEmail(emp.Email, subject, body);
            }
            if (Errors.Count() == 0)
            {
                emp = employee;
                return true;
            }
            else
                return false;
        }

        public bool IsValidOnInsert(Employee emp)
        {
            //Unique Employee Id Validation
            string EmployeeIdValue = emp.EmployeeId.ToString();
            int count = GetALL().Where(x => x.EmployeeId == EmployeeIdValue).ToList().Count();
            if (count != 0)
            {
                Errors.Add("EmployeeId Already Exist");
            }

            //Unique Email Validation
            string EmailValue = emp.Email.ToString();
            count = GetALL().Where(x => x.Email == EmailValue).ToList().Count();
            if (count != 0)
            {
                Errors.Add("Email Already Exist");
            }

            //You own BR validations

            if (Errors.Count() == 0)
                return true;
            else
                return false;
        }

        public bool IsValidOnUpdate(Employee emp)
        {
            //Total Exp should be greater than Relevant Exp
            var TotalExpValue = emp.TotalExp;
            var RelevantExpValue = emp.RelevantExp;

            if (RelevantExpValue > TotalExpValue)
            {
                Errors.Add("Total Exp should be greater than Relevant Exp");
            }

            if (Errors.Count() == 0)
                return true;
            else
                return false;
        }

        public bool RemindEmployee(string id, string msgStr)
        {
            var employee = ObjDb.GetByID(id);
            if (employee == null)
            {
                Errors.Add("Email id Does not Exist");
            }
            else
            {
                string subject = "Admin - Alert";
                string body = msgStr + "\n" +
                         "Regards," + "\n" +
                         "http:\\\\empManagementSystem.com";
                Utility.SendEmail(employee.Email, subject, body);
            }
            if (Errors.Count() == 0)
            {
                return true;
            }
            else
                return false;
        }
    }

}
