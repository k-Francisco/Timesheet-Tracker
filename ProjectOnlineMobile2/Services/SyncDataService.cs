using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using ProjectsRoot = ProjectOnlineMobile2.Models2.Projects.RootObject;
using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using PeriodsModel = ProjectOnlineMobile2.Models2.TimesheetPeriodsModel.PeriodsModel;
using LineModel = ProjectOnlineMobile2.Models2.LineModel.LineModel;
using LineWorkModel = ProjectOnlineMobile2.Models2.LineWorkModel.LineWorkModel;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;

namespace ProjectOnlineMobile2.Services
{
    public class SyncDataService 
    {
        private Realm realm { get; set; }

        public SyncDataService()
        {
            if (realm == null)
                realm = Realm.GetInstance();
        }

        public bool SyncProjects(ProjectsRoot projects, List<ProjectsModel> localProjects, ObservableCollection<ProjectsModel> displayedProjects)
        {
            try
            {
                //PARTS:
                //1.remove the projects that are deleted/removed form the server on the local database
                //2.add the projects that are added from the server on the local database
                //3.sync the changes that are made to the server on the local database

                //*Part 1
                var localProjectsClone = localProjects;
                foreach (var item in localProjectsClone)
                {
                    var temp = projects.D.Results
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if (temp == null)
                    {
                        realm.Write(() =>
                        {
                            realm.Remove(item);
                            displayedProjects.Remove(item);
                            localProjects.Remove(item);
                        });
                    }
                }
                realm.Refresh();

                foreach (var item in projects.D.Results)
                {
                    var temp = localProjects
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if (temp == null)
                    {
                        //*Part 2
                        realm.Write(() =>
                        {
                            realm.Add(item);
                            displayedProjects.Add(item);
                            localProjects.Add(item);
                        });
                    }
                    else
                    {
                        //*Part 3
                        realm.Write(() =>
                        {
                            temp.ProjectName = item.ProjectName;
                            temp.ProjectDescription = item.ProjectDescription;
                            temp.ProjectStartDate = item.ProjectStartDate;
                            temp.ProjectFinishDate = item.ProjectFinishDate;
                            temp.ProjectDuration = item.ProjectDuration;
                            temp.ProjectPercentComplete = item.ProjectPercentComplete;
                            temp.ProjectWork = item.ProjectWork;
                            temp.ProjectActualWork = item.ProjectActualWork;
                            temp.ProjectRemainingWork = item.ProjectRemainingWork;
                            temp.ProjectStatus = item.ProjectStatus;
                            temp.ProjectOwner = item.ProjectOwner;
                        });
                    }
                }

                realm.Refresh();

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncProjectsService", e.Message);
                return false;
            }
        }

        public bool SyncUserTasks(List<AssignmentsModel> localTasks, List<AssignmentsModel> tasksFromServer, ObservableCollection<AssignmentsModel> displayedTasks)
        {
            //PARTS
            //1. Remove the tasks that were deleted/removed in the server on the local database
            //2. add the tasks that are added from the server on the local database
            //3. sync the changes that are made to the server on the local database

            try
            {
                //PART 1
                var localTasksClone = localTasks;
                foreach (var item in localTasksClone)
                {
                    var temp = tasksFromServer
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if (temp == null)
                    {
                        realm.Write(() =>
                        {
                            realm.Remove(item);
                            localTasks.Remove(item);
                            displayedTasks.Remove(item);
                        });
                    }
                }
                realm.Refresh();

                foreach (var item in tasksFromServer)
                {
                    var temp = localTasks
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    //PART 2 
                    if (temp == null)
                    {
                        realm.Write(() =>
                        {
                            realm.Add(item);
                            localTasks.Add(item);
                            displayedTasks.Add(item);
                        });
                    }
                    else
                    {
                        //PART 3
                        realm.Write(() =>
                        {
                            temp.TaskActualWork = item.TaskActualWork;
                            temp.TaskFinishDate = item.TaskFinishDate;
                            temp.TaskPercentComplete = item.TaskPercentComplete;
                            temp.ProjectDetails = item.ProjectDetails;
                            temp.TaskRemainingWork = item.TaskRemainingWork;
                            temp.ResourceName = item.ResourceName;
                            temp.TaskStartDate = item.TaskStartDate;
                            temp.TaskName = item.TaskName;
                            temp.TaskWork = item.TaskWork;
                        });
                    }
                }
                realm.Refresh();

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncUserTasksService", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetPeriods(List<PeriodsModel> localPeriods, List<PeriodsModel> periodsFromServer, ObservableCollection<PeriodsModel> displayedPeriods)
        {
            try
            {
                //PARTS
                //1. Remove the periods that were deleted/removed in the server on the local database
                //2. add the periods that are added from the server on the local database
                //3. sync the changes that are made to the server on the local database

                //PART 1
                var localPeriodsClone = localPeriods;
                foreach (var item in localPeriods)
                {
                    var temp = periodsFromServer
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if (temp == null)
                    {
                        realm.Write(() =>
                        {
                            realm.Remove(item);
                            localPeriods.Remove(item);
                            displayedPeriods.Remove(item);
                        });
                    }
                }
                realm.Refresh();

                foreach (var item in periodsFromServer)
                {
                    var temp = localPeriods
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    //PART 2
                    if (temp == null)
                    {
                        realm.Write(() =>
                        {
                            realm.Add(item);
                            localPeriods.Add(item);
                            displayedPeriods.Add(item);
                        });
                    }
                    else
                    {
                        //PART 3
                        realm.Write(() =>
                        {
                            temp.End = item.End;
                            temp.PeriodName = item.PeriodName;
                            temp.Start = item.Start;
                        });
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetPeriods", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetLines(List<LineModel> localLines, List<LineModel> linesFromServer, ObservableCollection<LineModel> displayedLines, int periodId)
        {
            try
            {
                var localLinesClone = localLines;
                foreach (var item in localLinesClone)
                {
                    var temp = linesFromServer
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Remove(item);
                            localLines.Remove(item);
                        });
                    }
                }
                realm.Refresh();

                foreach (var item in linesFromServer)
                {
                    var temp = localLines
                        .Where(p=>p.ID == item.ID)
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(item);
                            localLines.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(() => {
                            temp.Comment = item.Comment;
                            temp.ProjectDetails = item.ProjectDetails;
                            temp.Status = item.Status;
                            temp.TaskDetails = item.TaskDetails;
                            temp.TotalWork = item.TotalWork;
                            temp.LinePeriodId = item.LinePeriodId;
                        });
                    }
                }
                realm.Refresh();

                //for displayed timesheet lines
                foreach (var item in localLines)
                {
                    if (item.LinePeriodId == periodId)
                    {
                        var temp = displayedLines
                            .Where(p => p.ID == item.ID)
                            .FirstOrDefault();

                        if(temp == null)
                        {
                            displayedLines.Add(item);
                        }
                        else
                        {
                            realm.Write(() => {
                                temp.Comment = item.Comment;
                                temp.ProjectDetails = item.ProjectDetails;
                                temp.Status = item.Status;
                                temp.TaskDetails = item.TaskDetails;
                                temp.TotalWork = item.TotalWork;
                                temp.LinePeriodId = item.LinePeriodId;
                            });
                        }
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLinesServer", e.Message);
                return false;
            }
        }

        public bool SyncTimesheetLineWork(List<LineWorkModel> localLineWorkModels, List<LineWorkModel> lineWorkModelsFromServer, ObservableCollection<LineWorkModel> displayedLines, int periodId, int lineId)
        {
            try
            {
                var localLineWorkModelsClone = localLineWorkModels;
                foreach (var item in localLineWorkModelsClone)
                {
                    var temp = lineWorkModelsFromServer
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Remove(item);
                            localLineWorkModels.Remove(item);
                        });
                    }
                }
                realm.Refresh();

                foreach (var item in lineWorkModelsFromServer)
                {
                    var temp = localLineWorkModels
                        .Where(p => p.ID == item.ID)
                        .FirstOrDefault();

                    if(temp == null)
                    {
                        realm.Write(()=> {
                            realm.Add(item);
                            localLineWorkModels.Add(item);
                        });
                    }
                    else
                    {
                        realm.Write(()=> {
                            temp.ActualWork = item.ActualWork;
                            temp.TimesheetLineId = item.TimesheetLineId;
                            temp.TimesheetPeriodId = item.TimesheetPeriodId;
                            temp.PlannedWork = item.PlannedWork;
                            temp.WorkDate = item.WorkDate;
                        });
                    }
                }
                realm.Refresh();

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLineWorkServer", e.Message);
                return false;
            }
        }
    }
}
