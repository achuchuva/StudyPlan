namespace StudyPlan.Data;

public class TimePreferenceScheduler : Scheduler
{
    public TimePreferenceScheduler(List<StudyTask> tasks) : base(tasks) {   }

    public override WeekPlan CreatePlan()
    {
        return base.CreatePlan();
    }

    public override void AssignStudyBlocks(DayPlan day)
    {
        base.AssignStudyBlocks(day);
    }

    public override TimeSpan GetTimeAvailableInTimeBlock(TimeSpan remainingTime)
    {
        return base.GetTimeAvailableInTimeBlock(remainingTime);
    }

    public override StudyTask SelectTask(DateTime currentDate)
    {
        return base.SelectTask(currentDate);
    }

    public override TimeSpan GetTimeRemainingInTask(TimeSpan studyTime, StudyTask task)
    {
        return base.GetTimeRemainingInTask(studyTime, task);
    }

    public override void UpdateTasks(StudyTask task)
    {
        base.UpdateTasks(task);
    }
}