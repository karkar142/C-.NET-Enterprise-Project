using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HRMSApplicationDbContext _dbContext;
        public DepartmentController(HRMSApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            bindPositionData();
            bindDepartmentData();
            return View();
        }

        private void bindDepartmentData()
        {
            IList<DepartmentViewModel>departments=_dbContext.Departments.Where(w=>!w.IsInActive).Select(s=>new DepartmentViewModel
            {
                Code = s.Code,
                Description = s.Description+"/"+s.ExtensionPhone,
            }).ToList();    
            ViewBag.Departments = departments;  
        }

        private void bindPositionData()
        {
            IList<PositionViewModel> positions = _dbContext.Positions.Where(w => !w.IsInActive).Select(s => new PositionViewModel
            {
                Code = s.Code,
                Name = s.Name+"/"+s.Level

            }).ToList();
            ViewBag.Positions = positions;  
        }

        [HttpPost]
        public IActionResult Entry(DepartmentViewModel departmentViewModel)
        {
            try
            {
                DepartmentEntity departmentEntity = new DepartmentEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = departmentViewModel.Code,
                    Description = departmentViewModel.Description,
                    ExtensionPhone = departmentViewModel.ExtensionPhone,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.Departments.Add(departmentEntity);
                _dbContext.SaveChanges();
                ViewData["info"] = "Successfully save the record to the system.";
                ViewData["Status"]=true;
            }
            catch (Exception e)
            {
                ViewData["info"] = "Error occur when record save to the system"+e.Message;
                ViewData["Status"] = false;
            }
            return View();
        }
        public IActionResult List()
        {
            IList<DepartmentViewModel> departments = _dbContext.Departments.Where(a => !a.IsInActive).Select(s => new DepartmentViewModel
            {
                Id =s.Id,
                Code = s.Code,
                Description = s.Description,
                ExtensionPhone = s.ExtensionPhone,
                TotalEmployees=_dbContext.Employees.Count(e=>e.DepartmentId==s.Id),
            }).ToList();
            return View(departments);
        }
        [Authorize(Roles = "HR")]
        public IActionResult Details(string id)
        {
            var department = _dbContext.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Code = d.Code,
                    Description = d.Description,  // Department Name
                    ExtensionPhone = d.ExtensionPhone,
                    TotalEmployees = _dbContext.Employees.Count(e => e.DepartmentId == d.Id),
                    Employees = _dbContext.Employees
                        .Where(e => e.DepartmentId == d.Id)
                        .Select(e => new EmployeeViewModel
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Email = e.Email,
                            PositionInfo = _dbContext.Positions
                                .Where(p => p.Id == e.PositionId)
                                .Select(p => p.Name)  // Fetching Position Name
                                .FirstOrDefault()
                        }).ToList()
                }).FirstOrDefault();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [Authorize(Roles = "HR")]
        public IActionResult Edit(string id)
        {
            DepartmentViewModel departmentViewModel=_dbContext.Departments.Where(w=>w.Id == id && !w.IsInActive).Select(d=> new DepartmentViewModel
            {
                Id = d.Id,
                Code = d.Code,
                Description = d.Description,
                ExtensionPhone = d.ExtensionPhone,
                CreatedOn=d.CreatedAt,
                UpdatedOn=d.ModifiedAt,
            }).FirstOrDefault();
            return View(departmentViewModel);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(DepartmentViewModel department)
        {
            try
            {
                DepartmentEntity departmentEntity = new DepartmentEntity()
                {   
                    Id=department.Id,
                    Code = department.Code,
                    Description = department.Description,
                    ExtensionPhone = department.ExtensionPhone,
                    CreatedAt=department.CreatedOn,
                    ModifiedAt = DateTime.Now,
                };
                _dbContext.Departments.Update(departmentEntity);
                _dbContext.SaveChanges();
                //return RedirectToAction("List");
                ViewData["info"] = "Sucessfully update the record to the system.";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {
                ViewData["info"] = "Error occur when the upate to the system."+e.Message;
                ViewData["Status"]=false;
                
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string id)
        {
            try
            {
                DepartmentEntity department = _dbContext.Departments.Find(id);
                if (department != null)
                {
                    department.IsInActive = true;
                    _dbContext.Departments.Update(department);
                    _dbContext.SaveChanges();
                    TempData["Info"] = "Delete success!";
                }
            }
            catch (Exception e)
            {

                TempData["Info"] = "Error occur when delete the record!"+e.Message;
            }
            return RedirectToAction("List");
        }
    }
}
