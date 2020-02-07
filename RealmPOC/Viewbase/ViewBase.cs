using Prism.Commands;
using Prism.Mvvm;
using RealmPOC.DatabaseModel;
using RealmPOC.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RealmPOC.Viewbase
{
   public class ViewBase:BindableBase
    {
        public static Realm RealmInstance;
        public ViewBase()
        {
          
            string directory = DbSetUp.CreateDirectorySetup();
            RealmInstance = DbSetUp.CreateDbFile(directory);
            GetEmployeesList();
            SaveRecordCommand = new DelegateCommand(SaveRecord);
            DeleteCommand = new DelegateCommand(DeleteEmployee);
          
        }


        private void DeleteEmployee()
        {
            var employeeList = this.EmployeeList.Where(x => x.IsChecked);
            List<Employee> deletEmpList = new List<Employee>();
            using (var transaction = RealmInstance.BeginWrite())
            {
                foreach (var item in employeeList)
                {
                    var data = RealmInstance.All<Employee>().Where(x => x.Id == item.Id).FirstOrDefault();
                    RealmInstance.Remove(data);
                }
                transaction.Commit();
            }

            GetEmployeesList();


        }



        private void GetEmployeesList()
        {
            var employeeList = RealmInstance.All<Employee>().ToList();
            List<EmployeeModel> employeeModelList = new List<EmployeeModel>();
            foreach (var item in employeeList)
            {
                EmployeeModel empModel = new EmployeeModel();
                empModel.FirstName = item.FirstName;
                empModel.LastName = item.LastName;
                empModel.Department = item.Department;
                empModel.Id = item.Id;
                employeeModelList.Add(empModel);
            }
            this.EmployeeList = employeeModelList;
            this.RaisePropertyChanged(nameof(EmployeeList));
            
        }
        private void SaveRecord()
        {
            try
            {
               int count=  RealmInstance.All<Employee>().Count();
                RealmInstance.Write(() => RealmInstance.Add(
                    new Employee
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Department = DepartmentName,
                        Id = count + 1

                    }
                    ));
                GetEmployeesList();
            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        #region Commands
        public ICommand SaveRecordCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        #region private Fields
        private string _firstName;
        private string _lastName;
        private string _departmentName;
        private List<EmployeeModel> emplist;
        #endregion

        #region public Props

        public List<EmployeeModel> EmployeeList
        {
            get
            {
                return emplist;
            }
            set
            {
                SetProperty(ref emplist, value);
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetProperty(ref _firstName, value);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        public string DepartmentName
        {
            get
            {
                return _departmentName;
            }
            set
            {
                SetProperty(ref _departmentName, value);
            }
        }

        #endregion
    }
}
