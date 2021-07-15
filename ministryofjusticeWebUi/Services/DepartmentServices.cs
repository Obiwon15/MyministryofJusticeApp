using System;
using System.Collections.Generic;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeDomain.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IDepartmentUnitOfWork _departmentUnitOfWork;

        public DepartmentServices(IDepartmentUnitOfWork departmentUnitOfWork) => _departmentUnitOfWork = departmentUnitOfWork;

        public ManageDepartmentViewModel ManageDepartment()
        {
            var md = new ManageDepartmentViewModel()
            {
                Departments = _departmentUnitOfWork.DepartmentRepo.GetDepartments(),
                Users = _departmentUnitOfWork.UserManagerRepo.GetAllUsers()
            };
            return md;
        }

        public bool CreateDepartment(DepartmentViewModel departmentViewModel)
        {
            var department = Mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
            _departmentUnitOfWork.DepartmentRepo.Insert(department);
            _departmentUnitOfWork.DepartmentRepo.Save();
            return true;
        }

        public bool UpdateDepartment(DepartmentViewModel model)
        {
            try
            {
                var departmentInDb = _departmentUnitOfWork.DepartmentRepo.GetById(model.Id);
                if (departmentInDb == null)
                    throw new Exception("Id didn't match");
                var department = Mapper.Map(model, departmentInDb);
                _departmentUnitOfWork.DepartmentRepo.Update(department);
                _departmentUnitOfWork.DepartmentRepo.Save();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Id mismatch {e}");
            }
        }

        public Department GetDepartmentById(int id)
        {
            var departmentId = _departmentUnitOfWork.DepartmentRepo.GetById(id);

            if (departmentId == null)
                throw new Exception("Id Mismatch");

            return departmentId;
        }


        public IEnumerable<Department> GetAllDepartments() => _departmentUnitOfWork.DepartmentRepo.GetDepartments();
    }
}