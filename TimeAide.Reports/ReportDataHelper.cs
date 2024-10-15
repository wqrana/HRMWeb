using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Data;
using TimeAide.Web.Models;
using TimeAide.Models;
using TimeAide.Models.ViewModel;

namespace TimeAide.Reports
{
   public class ReportDataHelper
    {
       private SqlConnection dbConnection = null;
        // private TimeAideContext timeAideWebContext = null;
        private TimeAideWindowContext timeAideWindowContext = null;
        
        public ReportDataHelper()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString;
            dbConnection = new SqlConnection(connectionString);
            timeAideWindowContext = DataHelper.GetSelectedClientTAWinEFContext();
        }
        public List<EmployeeAttendanceSchViewModel> getSelfServiceAttendanceRptData(int schId)
        {
            List<EmployeeAttendanceSchViewModel> retRptData = null; 
            try
            {

                var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                    .Where(w => w.ID == schId)
                    .FirstOrDefault();
                if (userSchedule != null)
                {
                    retRptData = timeAideWindowContext.tSchedModDailyDetails.Where(w => w.nUserID == userSchedule.nUserID &&
                                                                   w.dPunchDate >= userSchedule.dStartDate && w.dPunchDate <= userSchedule.dEndDate)
                                              .OrderBy(o => o.dPunchDate).AsEnumerable()
                                              .Select(s => new EmployeeAttendanceSchViewModel
                                              {

                                                  EmployeeId = s.nUserID,
                                                  EmployeeName = s.sUserName,
                                                  sWeekID = s.sWeekID,
                                                  sNote = userSchedule.sNote,
                                                  dblPeriodHours = userSchedule.dblPeriodHours,
                                                  dStartDate = userSchedule.dStartDate,
                                                  dEndDate = userSchedule.dEndDate,
                                                  nPayPeriodType = userSchedule.nPayPeriodType,
                                                  dPunchDate = s.dPunchDate,
                                                  dPunchIn1 = s.dPunchIn1,
                                                  dPunchOut1 = s.dPunchOut1,
                                                  dPunchIn2 = s.dPunchIn2,
                                                  dPunchOut2 = s.dPunchOut2,
                                                  nWorkDayType = s.nWorkDayType,
                                                  sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (s.nWorkDayType ?? 0)).FirstOrDefault().Name,
                                                  nPunchNum = s.nPunchNum,
                                                  dblDayHours = s.dblDayHours,
                                                  nPayWeekNum = userSchedule.nPayWeekNum,
                                                   sNoteDetail = s.sNote
                                                   

                                              }).ToList();
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (timeAideWindowContext != null)
                    timeAideWindowContext.Dispose();

            }
            return retRptData;
        }
        public List<EmployeeAttendanceSchViewModel> getSelfServiceWebWebAttendanceSchRptData(int schId,TimeAideContext dbCxt)
        {
            List<EmployeeAttendanceSchViewModel> retRptData = null;
            try
            {
                var userSchedule = dbCxt.EmployeeWebScheduledPeriod
                    .Where(w => w.Id == schId)
                    .FirstOrDefault();
                if (userSchedule != null)
                {
                    retRptData = dbCxt.EmployeeWebScheduledPeriodDetail.Where(w => w.UserInformationId == userSchedule.UserInformationId &&
                                                                   w.PunchDate >= userSchedule.PeriodStartDate && w.PunchDate <= userSchedule.PeriodEndDate)
                                              .OrderBy(o => o.PunchDate).AsEnumerable()
                                              .Select(s => new EmployeeAttendanceSchViewModel
                                              {

                                                  EmployeeId = s.UserInformation.EmployeeId,
                                                  EmployeeName = s.UserInformation.ShortFullName,
                                                  sWeekID = s.PayWeekNumber.ToString(),
                                                  sNote = userSchedule.Note,
                                                  dblPeriodHours = userSchedule.PeriodHours,
                                                  dStartDate = userSchedule.PeriodStartDate,
                                                  dEndDate = userSchedule.PeriodEndDate,
                                                  nPayPeriodType = userSchedule.PayFrequencyId,
                                                  dPunchDate = s.PunchDate,
                                                  dPunchIn1 = s.TimeIn1,
                                                  dPunchOut1 = s.TimeOut1,
                                                  dPunchIn2 = s.TimeIn2,
                                                  dPunchOut2 = s.TimeOut2,
                                                  nWorkDayType = s.WorkDayTypeId,
                                                  sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (s.WorkDayTypeId ?? 0)).FirstOrDefault().Name,
                                                  nPunchNum = s.NoOfPunch,
                                                  dblDayHours = s.DayHours,
                                                  nPayWeekNum = userSchedule.PayWeekNumber,
                                                  sNoteDetail = s.Note


                                              }).ToList();
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return retRptData;
        }
        public List<EmployeeAttendanceSchViewModel> getAttendanceScheduleMainRptData(string selectedSchIds)
        {
            List<EmployeeAttendanceSchViewModel> retRptData = null;
            if (string.IsNullOrEmpty(selectedSchIds))
            {
                return retRptData =  new List<EmployeeAttendanceSchViewModel> { };
            }
            var selectedSchIdsList = selectedSchIds.Split(',');
            try
            {
                if (selectedSchIdsList.Count() > 0)
                {

                    var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                        .Where(w => selectedSchIdsList.Contains(w.ID.ToString()));
                    //.FirstOrDefault();
                    if (userSchedule != null)
                    {

                        retRptData = userSchedule.Join(timeAideWindowContext.tSchedModDailyDetails,
                            urs => new { p1 = urs.nUserID, p2 = urs.sWeekID },     //Condition from outer table
                             ursd => new { p1 = ursd.nUserID, p2 = ursd.sWeekID },          //Condition from inner table
                             (urs, ursd) => new EmployeeAttendanceSchViewModel
                             {

                                 SchId = urs.ID,
                                 EmployeeId = urs.nUserID,
                                 EmployeeName = urs.sUserName,
                                 sWeekID = urs.sWeekID,
                                 sNote = urs.sNote,
                                 dblPeriodHours = urs.dblPeriodHours,
                                 dStartDate = urs.dStartDate,
                                 dEndDate = urs.dEndDate,
                                 nPayPeriodType = urs.nPayPeriodType,
                                 dPunchDate = ursd.dPunchDate,
                                 dPunchIn1 = ursd.dPunchIn1,
                                 dPunchOut1 = ursd.dPunchOut1,
                                 dPunchIn2 = ursd.dPunchIn2,
                                 dPunchOut2 = ursd.dPunchOut2,
                                 nWorkDayType = ursd.nWorkDayType,
                                 //sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (ursd.nWorkDayType ?? 0)).FirstOrDefault().Name,
                                 nPunchNum = ursd.nPunchNum,
                                 dblDayHours = ursd.dblDayHours,
                                 nPayWeekNum = urs.nPayWeekNum,
                                 sNoteDetail = ursd.sNote

                             }
                             ).AsEnumerable()
                             .OrderBy(o=>o.SchId).
                             Select(s => new EmployeeAttendanceSchViewModel
                             {

                                 SchId = s.SchId,
                                 EmployeeId = s.EmployeeId,
                                 EmployeeName = s.EmployeeName,
                                 sWeekID = s.sWeekID,
                                 sNote = s.sNote,
                                 dblPeriodHours = s.dblPeriodHours,
                                 dStartDate = s.dStartDate,
                                 dEndDate = s.dEndDate,
                                 nPayPeriodType = s.nPayPeriodType,
                                 dPunchDate = s.dPunchDate,
                                 dPunchIn1 = s.dPunchIn1,
                                 dPunchOut1 = s.dPunchOut1,
                                 dPunchIn2 = s.dPunchIn2,
                                 dPunchOut2 = s.dPunchOut2,
                                 nWorkDayType = s.nWorkDayType,
                                 sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (s.nWorkDayType ?? 0)).FirstOrDefault().Name,
                                 nPunchNum = s.nPunchNum,
                                 dblDayHours = s.dblDayHours,
                                 nPayWeekNum = s.nPayWeekNum,
                                 sNoteDetail = s.sNoteDetail

                             }).ToList();

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (timeAideWindowContext != null)
                    timeAideWindowContext.Dispose();

            }
            return retRptData;
        }
        public List<EmployeeAttendanceSchViewModel> getAttendanceWebScheduleMainRptData(string selectedSchIds,TimeAideContext dbCxt)
        {
            List<EmployeeAttendanceSchViewModel> retRptData = null;

            var selectedSchIdsList = selectedSchIds.Split(',');
            try
            {
                if (selectedSchIdsList.Count() > 0)
                {

                    var userSchedule = dbCxt.EmployeeWebScheduledPeriod
                        .Where(w => selectedSchIdsList.Contains(w.Id.ToString()));
                    //.FirstOrDefault();
                    if (userSchedule != null)
                    {

                        retRptData = userSchedule.Join(dbCxt.EmployeeWebScheduledPeriodDetail,
                            urs => new { p1 = urs.UserInformationId, p2 = urs.PayWeekNumber },     //Condition from outer table
                             ursd => new { p1 = ursd.UserInformationId, p2 = ursd.PayWeekNumber },          //Condition from inner table
                             (urs, ursd) => new EmployeeAttendanceSchViewModel
                             {

                                 SchId = urs.Id,
                                 EmployeeId = urs.UserInformation.EmployeeId,
                                 EmployeeName = urs.UserInformation.ShortFullName,
                                 sWeekID = urs.PayWeekNumber.ToString(),
                                 sNote = urs.Note,
                                 dblPeriodHours = urs.PeriodHours,
                                 dStartDate = urs.PeriodStartDate,
                                 dEndDate = urs.PeriodEndDate,
                                 nPayPeriodType = urs.PayFrequencyId,
                                 dPunchDate = ursd.PunchDate,
                                 dPunchIn1 = ursd.TimeIn1,
                                 dPunchOut1 = ursd.TimeOut1,
                                 dPunchIn2 = ursd.TimeIn2,
                                 dPunchOut2 = ursd.TimeOut2,
                                 nWorkDayType = ursd.WorkDayTypeId,
                                 //sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (ursd.nWorkDayType ?? 0)).FirstOrDefault().Name,
                                 nPunchNum = ursd.NoOfPunch,
                                 dblDayHours = ursd.DayHours,
                                 nPayWeekNum = urs.PayWeekNumber,
                                 sNoteDetail = ursd.Note

                             }
                             ).AsEnumerable()
                             .OrderBy(o => o.SchId).
                             Select(s => new EmployeeAttendanceSchViewModel
                             {

                                 SchId = s.SchId,
                                 EmployeeId = s.EmployeeId,
                                 EmployeeName = s.EmployeeName,
                                 sWeekID = s.sWeekID,
                                 sNote = s.sNote,
                                 dblPeriodHours = s.dblPeriodHours,
                                 dStartDate = s.dStartDate,
                                 dEndDate = s.dEndDate,
                                 nPayPeriodType = s.nPayPeriodType,
                                 dPunchDate = s.dPunchDate,
                                 dPunchIn1 = s.dPunchIn1,
                                 dPunchOut1 = s.dPunchOut1,
                                 dPunchIn2 = s.dPunchIn2,
                                 dPunchOut2 = s.dPunchOut2,
                                 nWorkDayType = s.nWorkDayType,
                                 sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (s.nWorkDayType ?? 0)).FirstOrDefault().Name,
                                 nPunchNum = s.nPunchNum,
                                 dblDayHours = s.dblDayHours,
                                 nPayWeekNum = s.nPayWeekNum,
                                 sNoteDetail = s.sNoteDetail

                             }).ToList();

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return retRptData;
        }
        public List<EmployeeTimesheet> getEmployeeTimesheetRptData(string selectedTimesheetIds)
        {
            List<EmployeeTimesheet> retRptData = null;

            var selectedTimesheetIdsList = selectedTimesheetIds.Split(',');
            try
            {
                if (selectedTimesheetIdsList.Count() > 0)
                {

                    var reportWeeks = timeAideWindowContext.tReportWeeks
                        .Where(w => selectedTimesheetIdsList.Contains(w.tpwID.ToString()));
                    //.FirstOrDefault();
                    if (reportWeeks != null)
                    {

                        //retRptData = reportWeeks.Join(timeAideWindowContext.tPunchDates,
                        //    rws => new { p1 = rws.e_id, p2 = rws.sWeekID },     //Condition from outer table
                        //    pds => new { p1 = pds.e_id, p2 = pds.sWeekID },          //Condition from inner table
                        //     (rws, pds) => new EmployeeTimesheet
                        //     {
                        //         Id = rws.tpwID,
                        //         EmployeeId = rws.e_id,
                        //         OldCompanyId = rws.nCompID,
                        //         nUserID = rws.e_id,
                        //         sUserName = rws.e_name,
                        //         sCompanyName = rws.sCompanyName,
                        //         sHoursSummary = rws.sHoursSummary,
                        //         dStartDate = rws.DTStartDate,
                        //         dEndDate = rws.DTEndDate,
                        //         nPayWeekNum = rws.nPayWeekNum,
                        //         dPunchDate = pds.DTPunchDate,
                        //         dblPunchHours = pds.dblPunchHrs,
                        //         sPunchSummary = pds.sPunchSummary,
                        //         sExpections = pds.sExceptions,
                        //         sDaySchedule = pds.sDaySchedule,
                        //         sPunchHoursSummary = pds.sHoursSummary
                        //     }
                        //     );

                        retRptData =
                        (from rws in reportWeeks
                         join pds in timeAideWindowContext.tPunchDates
                             on new { User = rws.e_id, rws.nWeekID } equals new { User = (int?)pds.e_id, pds.nWeekID }
                             into cmpFld
                         from d in cmpFld.DefaultIfEmpty()
                         select new EmployeeTimesheet
                         {
                             Id = rws.tpwID,
                             EmployeeId = rws.e_id,
                             OldCompanyId = rws.nCompID,
                             nUserID = rws.e_id,
                             sUserName = rws.e_name,
                             sCompanyName = rws.sCompanyName,
                             sDeptName = rws.sDeptName,
                             sEmployeeTypeName = rws.sEmployeeTypeName,
                             sPayRuleName= rws.sPayRuleName,
                             sHoursSummary = rws.sHoursSummary,
                             dStartDate = rws.DTStartDate,
                             dEndDate = rws.DTEndDate,
                             nPayWeekNum = rws.nPayWeekNum,
                             dPunchDate = d.DTPunchDate,
                             dblPunchHours = d.dblPunchHrs,
                             sPunchSummary = d.sPunchSummary,
                             sExpections = d.sExceptions,
                             sDaySchedule = d.sDaySchedule,
                             sPunchHoursSummary = d.sHoursSummary
                         }).ToList().OrderBy(o=>o.Id).ToList();
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (timeAideWindowContext != null)
                    timeAideWindowContext.Dispose();

            }
            return retRptData;
        }
        public List<EmployeeTimeAndEffort> getEmployeeTAERptData(string selectedTAEIds)
        {
            List<EmployeeTimeAndEffort> retRptData = null;

            var selectedTAEIdsList = selectedTAEIds.Split(',');
            try
            {
                if (selectedTAEIdsList.Count() > 0)
                {
                    var idsParameter = new SqlParameter("@TAESignIds", selectedTAEIds);
                    retRptData = timeAideWindowContext.Database.SqlQuery<EmployeeTimeAndEffort>("dbo.spTE_GetTimeAndEffortReportData @TAESignIds", idsParameter).ToList();
                    
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                if (timeAideWindowContext != null)
                    timeAideWindowContext.Dispose();

            }
            return retRptData;
        }
        public EmployeePayStubViewModel getEmployeePayStubRptData(string batchId, int employeeId)
        {
            EmployeePayStubViewModel retRptData = new EmployeePayStubViewModel();
            try
            {

                retRptData.EmployeePayStubBatch = timeAideWindowContext.viewPay_UserBatchStatus.
                                                Where(w => w.strBatchID.ToString() == batchId && w.intUserID == employeeId)
                                                .Select(s => new EmployeePayStubBatch
                                                {
                                                    strBatchID = s.strBatchID,
                                                    strCompany = s.strCompany,
                                                    intUserID = s.intUserID,
                                                    EmployeeId = s.intUserID,
                                                    strUserName = s.strUserName,
                                                    sHomeAddressLine1 = s.sHomeAddressLine1,
                                                    sHomeAddressLine2 = s.sHomeAddressLine2,
                                                    strState = s.strState,
                                                    strCity = s.strCity,
                                                    strZipCode = s.strZipCode,
                                                    dtPayDate = s.dtPayDate,
                                                    dtStartDatePeriod = s.dtStartDatePeriod,
                                                    dtEndDatePeriod = s.dtEndDatePeriod,
                                                    intPayWeekNum = s.intPayWeekNum,
                                                    decBatchUserCompensations = s.decBatchUserCompensations,
                                                    decBatchUserWithholdings = s.decBatchUserWithholdings,
                                                    decBatchNetPay = s.decBatchNetPay,
                                                    strCompanyName = s.strCompanyName,
                                                    

                                                }).FirstOrDefault();
                retRptData.EmployeePayStubCompany = timeAideWindowContext.tblCompanyPayrollInformations
                                            .Where(w => w.strCompanyName == retRptData.EmployeePayStubBatch.strCompanyName)
                                            .Select(s => new EmployeePayStubCompany
                                            {
                                                strAddress1 = s.strAddress1,
                                                strAddress2 = s.strAddress2,
                                                strState = s.strState,
                                                strCity = s.strCity,
                                                strZipCode = s.strZipCode,
                                               
                                            })
                                            .FirstOrDefault();

                retRptData.EmployeePayStubCompensations = timeAideWindowContext.fnPay_tblUserCompensationsYTD(batchId)
                                           .Where(w => w.intUserID == employeeId && w.decPayYTD != 0)
                                           .OrderBy(o => o.intReportOrder)
                                           .Select(s => new EmployeePayStubCompensation
                                           {
                                               strCompensationName = s.strCompensationName,
                                               decHours = s.decHours,
                                               decPayRate = s.decPayRate,
                                               decPay = s.decPay,
                                               decPayYTD = s.decPayYTD
                                           }).ToList();
                retRptData.EmployeePayStubWithholdings = timeAideWindowContext.fnPay_tblUserWithholdingsYTD(batchId)
                                            .Where(w => w.intUserID == employeeId && w.decWithholdingsYTD != 0)
                                           .OrderBy(o => o.intReportOrder)
                                           .Select(s => new EmployeePayStubWithholding
                                           {
                                               strWithHoldingsName = s.strWithHoldingsName,
                                               decWithholdingsAmount = s.decWithholdingsAmount,
                                               decWithholdingsYTD = s.decWithholdingsYTD
                                           }).ToList();

            }
            catch (Exception ex)
            {
                Exception exception = new Exception(ex.Message);
                return null;
            }

            return retRptData;
        }
        public DataTable getEmployeeDetailRptData() {

            SqlCommand cmd = null;
            SqlDataAdapter adp = null;
            DataTable dt = new DataTable();

            try
            {
                
                cmd = new SqlCommand();
                cmd.Connection = dbConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * from UserInformation where ClientId=1";

                //cmd.Parameters.Clear();
                //SqlParameter[] param = new SqlParameter[]
                //{
                //    new SqlParameter("@DateFrom", DateFrom),
                //    new SqlParameter("@DateTo", DateTo),
                //    new SqlParameter("@OfficeLevel", OfficeLevel),
                //    new SqlParameter("@BranchId", BranchId)
                //};

                //cmd.Parameters.AddRange(param);

                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }

            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
            }
            finally
            {
               
                if (cmd != null)
                    cmd.Dispose();
                if (adp != null)
                    adp.Dispose();
            }
            return dt;
        }

    }
}
