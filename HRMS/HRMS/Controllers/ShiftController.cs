using HRMS.DAO;
using HRMS.Models.Entities;
using HRMS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class ShiftController : Controller
    {
        public readonly HRMSApplicationDbContext _dbContext;
        public ShiftController(HRMSApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize(Roles = "HR")]
        public IActionResult Entry()
        {
            bindAttendancePolicyData();
            return View();
        }

        private void bindAttendancePolicyData()
        {
            IList<AttendancePolicyViewModel> attendancePolicy = _dbContext.AttendancePolicies.Where(w => !w.IsInActive).Select(s => new AttendancePolicyViewModel
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();
            ViewBag.AttendancePolicy = attendancePolicy;
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Entry(ShiftViewModel Ui)
        {
            try
            {
                ShiftEntity shift = new ShiftEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Ui.Name,
                    InTime = Ui.InTime,
                    OutTime = Ui.OutTime,
                    LateAfter = Ui.LateAfter,
                    EarlyOutBefore = Ui.EarlyOutBefore,
                    AttendancePolicyID= Ui.AttendancePolicyID,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.Shifts.Add(shift);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfuly save the record to the system";
                ViewData["Status"] = true;
                bindAttendancePolicyData();
            }
            catch (Exception e)
            {
                ViewData["Info"] = "Error Occur when save the record to the system" + e.Message;
                ViewData["Status"] = false;

            }
            return View();
        }
        public IActionResult List()
        {
            IList<ShiftViewModel>shiftViews=(from e in _dbContext.Shifts
                                             join f in _dbContext.AttendancePolicies
                                             on e.AttendancePolicyID equals f.Id
                                             where !e.IsInActive && ! f.IsInActive
                                             select new ShiftViewModel
                                             {
                                                 Id=e.Id,
                                                 Name=e.Name,
                                                 InTime=e.InTime,
                                                 OutTime=e.OutTime,
                                                 LateAfter=e.LateAfter,
                                                 EarlyOutBefore=e.EarlyOutBefore,
                                                 AttendancePolicyID=e.AttendancePolicyID,
                                                 AttendancePolicyInfo=f.Name,
                                                 CreatedOn=e.CreatedAt,
                                                 UpdatedOn=e.ModifiedAt,
                                             }).ToList();
            return View(shiftViews);
        }
        [Authorize(Roles = "HR")]
        public IActionResult Edit(string id)
        {
            ShiftViewModel shiftView = _dbContext.Shifts.Where(w => w.Id == id && !w.IsInActive).Select(s => new ShiftViewModel
            {
                Id = s.Id,
                Name = s.Name,
                InTime = s.InTime,
                OutTime = s.OutTime,
                LateAfter = s.LateAfter,
                EarlyOutBefore = s.EarlyOutBefore,
                AttendancePolicyID = s.AttendancePolicyID,
                CreatedOn = s.CreatedAt,
                UpdatedOn = s.ModifiedAt
            }).FirstOrDefault();
            bindAttendancePolicyData();
            return View(shiftView);
        }
        [Authorize(Roles = "HR")]
        [HttpPost]
        public IActionResult Update(ShiftViewModel shiftViewModel)
        {
            try
            {
                ShiftEntity shiftEntity = new ShiftEntity()
                {
                    Id = shiftViewModel.Id,
                    Name = shiftViewModel.Name,
                    InTime = shiftViewModel.InTime,
                    OutTime = shiftViewModel.OutTime,
                    LateAfter = shiftViewModel.LateAfter,
                    EarlyOutBefore = shiftViewModel.EarlyOutBefore,
                    AttendancePolicyID = shiftViewModel.AttendancePolicyID,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = shiftViewModel.CreatedOn
                };
                _dbContext.Shifts.Update(shiftEntity);
                _dbContext.SaveChanges();
                ViewData["Info"] = "Successfully update the record to the system.";
                ViewData["Status"] = true;
            }
            catch (Exception e)
            {
                ViewData["Info"] = "Error occur when the record update to the system." + e.Message;
                ViewData["Status"] = false;
            }
            return RedirectToAction("List");
        }
        /*  public IActionResult Edit(string Id)
          {
              ShiftViewModel shift=_dbContext.Shifts.Where(w=>w.Id==Id && !w.IsInActive).Select(s=> new ShiftViewModel
              {
                  Id = s.Id,
                  Name=s.Name,
                  InTime=s.InTime,
                  OutTime=s.OutTime,
                  LateAfter=s.LateAfter,
                  EarlyOutBefore=s.EarlyOutBefore,
                  AttendancePolicyID=s.AttendancePolicyID,
                  CreatedOn=s.CreatedAt,
                  UpdatedOn=s.ModifiedAt,
              }).FirstOrDefault();
              if(shift==null)
              {
                  return NotFound();
              }
              bindAttendancePolicyData();
              return View(shift);
          }
          [HttpPost]
         public IActionResult Update(ShiftViewModel ui)
          {
              try
              {
                  // 1. Retrieve the existing ShiftEntity from the database.  This is the KEY!
                  var shift = _dbContext.Shifts.Find(ui.Id);

                  if (shift == null)
                  {
                      ViewData["Info"] = "Shift not found!";
                      ViewData["Status"] = false;
                      RedirectToAction("List");
                  }

                  // 2. Update the properties of the *existing* entity.
                  shift.Name = ui.Name;
                  shift.InTime = ui.InTime;
                  shift.OutTime = ui.OutTime;
                  shift.LateAfter = ui.LateAfter;
                  shift.EarlyOutBefore = ui.EarlyOutBefore;
                  shift.AttendancePolicyID = ui.AttendancePolicyID;
                  shift.ModifiedAt = DateTime.Now; // Update ModifiedAt

                  // 3. _dbContext.Shifts.Update(shift);  Not strictly necessary, EF tracks changes.
                  _dbContext.SaveChanges(); // Save changes to the database

                  ViewData["Info"] = "Successfully updated record to the system";
                  ViewData["Status"] = true;
              }
              catch (Exception e)
              {
                  ViewData["Info"] = "Error occur when update the record" + e.Message;
                  ViewData["Status"] = false;
              }

              return RedirectToAction("List");
          }*/
        [Authorize(Roles = "HR")]
        public IActionResult Delete(string Id)
        {
            ShiftEntity shift = _dbContext.Shifts.Find(Id);
            if(shift != null)
            {
                shift.IsInActive = true;
                _dbContext.Shifts.Update(shift);
                _dbContext.SaveChanges();
                TempData["Info"] = "Successfully delete the record";
            }
            else
            {
                TempData["Info"] = "Error occor when delete the record";
            }
            return RedirectToAction("List");
        }
    }
}
