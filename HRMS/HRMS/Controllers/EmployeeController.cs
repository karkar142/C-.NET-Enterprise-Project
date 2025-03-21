using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly HRMSApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeController(HRMSApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
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
            IList<DepartmentViewModel> departments = _dbContext.Departments.Where(w => !w.IsInActive).Select(s => new DepartmentViewModel
            {
                Id = s.Id,
                Code = s.Code + "/" + s.Description,
            }).ToList();
            ViewBag.DepartmentViews = departments;
        }

        private void bindPositionData()
        {
            IList<PositionViewModel> positionViews = _dbContext.Positions.Where(w => !w.IsInActive).Select(s => new PositionViewModel
            {
                Id = s.Id,
                Code = s.Code + "/" + s.Name,
            }).ToList();
            ViewBag.PositionViews = positionViews;
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public async Task<IActionResult> EntryAsync(EmployeeViewModel ui)
        {
            try
            {
                var user=new IdentityUser { UserName=ui.Email, Email=ui.Email };
                var result = await _userManager.CreateAsync(user, "HRMS@prodev@2025");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                }
                EmployeeEntity employee = new EmployeeEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = ui.Code,
                    Name = ui.Name,
                    Email = ui.Email,
                    Gender = ui.Gender,
                    PhoneNo = ui.PhoneNo,
                    DOB = ui.DOB,
                    DOR = ui.DOR,
                    DOE = ui.DOE,
                    Address = ui.Address,
                    BasicSalary = ui.BasicSalary,
                    PositionId = ui.PositionId,
                    DepartmentId = ui.DepartmentId,
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,   
                };
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully save the record to the system";
                ViewData["Status"]=true;
            }
            catch (Exception e)
            {

                ViewData["Infp"]="Error occur when save t he record to the system"+e.Message;
                ViewData["Status"]=false;
            }
            bindPositionData();
            bindDepartmentData();
            return View();
        }
        public IActionResult List()
        {
            IList<EmployeeViewModel>employees=(from e in _dbContext.Employees
                                               join d in _dbContext.Departments
                                               on e.DepartmentId equals d.Id
                                               join f in _dbContext.Positions
                                               on e.PositionId equals f.Id 
                                               where !e.IsInActive && !d.IsInActive && !f.IsInActive
                                               select new  EmployeeViewModel
            {
                Id = e.Id,
                UserId=e.UserId,
                Code=e.Code,
                Name=e.Name,
                Email=e.Email,
                Gender=e.Gender,
                PhoneNo=e.PhoneNo,
                DOB=e.DOB,
                DOR=e.DOR,
                DOE=e.DOE,
                Address=e.Address,
                BasicSalary=e.BasicSalary,
                PositionId=e.PositionId,
                PositionInfo=f.Code+"/"+f.Name,
                DepartmentId=e.DepartmentId,
                DepartmentInfo=d.Code+"/"+d.Description,
                CreatedOn=e.CreatedAt,
                UpdatedOn=e.ModifiedAt,

            }).ToList();
            if (!User.IsInRole("HR"))
            {
                var loginedUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                employees=employees.Where(w=>w.UserId==loginedUser.Id).ToList();
            }
            return View(employees);

        }
        [Authorize(Roles = "HR")]
        public IActionResult Edit(string id)
        {
            EmployeeViewModel employee=_dbContext.Employees.Where(w=>w.Id== w.Id && !w.IsInActive).Select(s=> new EmployeeViewModel
            {
                Id=s.Id,
                Code = s.Code,
                Name = s.Name,
                Email = s.Email,
                Gender = s.Gender,
                PhoneNo = s.PhoneNo,
                DOB = s.DOB,
                DOR = s.DOR,
                DOE = s.DOE,
                Address = s.Address,
                BasicSalary = s.BasicSalary,
                PositionId = s.PositionId,
                DepartmentId = s.DepartmentId,
                CreatedOn=s.CreatedAt,
                UpdatedOn=s.ModifiedAt,
            }).FirstOrDefault();
            bindDepartmentData();
            bindPositionData();
            return View(employee);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(EmployeeViewModel employee)
        {
            try
            {
                EmployeeEntity employeeEntity = new EmployeeEntity()
                {
                    Id = employee.Id,
                    Code=employee.Code,
                    Name = employee.Name,
                    Email = employee.Email,
                    Gender = employee.Gender,
                    PhoneNo = employee.PhoneNo,
                    DOB = employee.DOB,
                    DOR = employee.DOR,
                    DOE = employee.DOE,
                    Address = employee.Address,
                    BasicSalary = employee.BasicSalary,
                    PositionId = employee.PositionId,
                    DepartmentId = employee.DepartmentId,
                    CreatedAt = employee.CreatedOn,
                    ModifiedAt = DateTime.Now,
                };
                _dbContext.Employees.Update(employeeEntity);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully update the record to the system";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {

                ViewData["Info"]="Error occur when update the record to the system"+e.Message;
                ViewData["Status"]=false;
            }  
            return RedirectToAction("List");
        }
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string id)
        {
            EmployeeEntity employee=_dbContext.Employees.Find(id);
            if(employee != null)
            {
                employee.IsInActive=true;
                _dbContext.Employees.Update(employee);
                _dbContext.SaveChanges();
                TempData["Info"] = "Successfully delete the record";
            }
            else
            {
                TempData["Info"] = "Error occur when delete the record";
            }
            return RedirectToAction("List");
        }
    }
}
