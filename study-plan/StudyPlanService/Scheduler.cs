namespace StudyPlanService;

public class Scheduler
{
    public TimeSpan studyPeriod = TimeSpan.FromHours(1);
    public TimeSpan breakPeriod = TimeSpan.Zero;
    public bool weekendsOff;
    private List<Task> tasks;

    public Scheduler(List<Task> tasks)
    {
        this.tasks = new List<Task>(tasks);
    }

    public Scheduler(List<Task> tasks, TimeSpan studyPeriod, TimeSpan breakPeriod, bool weekendsOff)
    {
        this.tasks = new List<Task>(tasks);
        this.studyPeriod = studyPeriod;
        this.breakPeriod = breakPeriod;
        this.weekendsOff = weekendsOff;
    }

    public virtual WeekPlan CreatePlan()
    {
        // The basic Scheduler has a constant study block of 1 hour
        // In addition, the student completes work in two periods daily
        // From 9am to 12pm followed by an hour break and then from 1pm to 5pm

        // Create the week with starting date of tomorrow
        WeekPlan plannedWeek = new WeekPlan(DateOnly.FromDateTime(DateTime.Today).AddDays(1));

        // Loop through the days of the week
        foreach (DayPlan day in plannedWeek.Days)
        {
            // Assign the study blocks of each day
            // Check if the given day isn't on the weekend if the user wants a break on the weekend
            if (weekendsOff)
            {
                if (day.date.DayOfWeek == DayOfWeek.Saturday || day.date.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
            }
            AssignStudyBlocks(day);
        }

        // Return the planned week
        return plannedWeek;
    }

    public virtual void AssignStudyBlocks(DayPlan day)
    {
        // Loop through the time blocks of each day
        foreach (TimeBlock timeBlock in day.TimeBlocks)
        {
            // While there is still time for studying left in the TimeSpan
            while (timeBlock.duration > TimeSpan.Zero)
            {
                // Get the time available in the time block
                TimeSpan remainingStudyTime = GetTimeAvailableInTimeBlock(timeBlock.duration);

                // Convert the day date and time block start time to DateTime
                DateTime currentTime = new DateTime(day.date.Year, day.date.Month, day.date.Day, timeBlock.startTime.Hour, timeBlock.startTime.Minute, timeBlock.startTime.Second);

                // Select the task
                Task? selectedTask = SelectTask(currentTime);
                if (selectedTask == null)
                {
                    // Break out if the tasks have 0 priority
                    // Either they're already completed or aren't relevant enough to be scheduled
                    break;
                }

                if (selectedTask is ExamStudy && IsTaskInDay(selectedTask, day))
                {
                    // The task is an exam study variation
                    // We only want one exam study block per day
                    break;
                }

                // Get the study time based on the time available in the time block and the selected task
                TimeSpan studyTime = GetTimeRemainingInTask(remainingStudyTime, selectedTask);

                // Create a new study block and add it to the day
                day.StudyBlocks.Add(new StudyBlock(selectedTask, timeBlock.startTime, studyTime));

                // Update the time block with the study time and the break period
                // This way, the next study block will acknowledge the break period
                timeBlock.UpdateTimeBlock(studyTime + breakPeriod);

                // Decrease the selected task's estimated time to completion
                selectedTask.DecreaseEstimatedTime(studyTime);
            }
        }
    }

    public virtual TimeSpan GetTimeAvailableInTimeBlock(TimeSpan remainingTime)
    {
        // Return the minimum of remaining time and a normal study period
        if (remainingTime >= studyPeriod)
        {
            return studyPeriod;
        }
        else
        {
            return remainingTime;
        }
    }

    public virtual Task? SelectTask(DateTime currentDate)
    {
        // Get the task based on index
        double priority = double.NegativeInfinity;
        int taskIndex = 0;

        for (int i = 0; i < tasks.Count; i++)
        {
            double taskPriority = tasks[i].GetPriority(currentDate);

            if (taskPriority > priority)
            {
                priority = taskPriority;
                taskIndex = i;
            }
        }

        if (priority <= 0.0)
        {
            return null;
        }

        return tasks[taskIndex];
    }

    public bool IsTaskInDay(Task task, DayPlan day)
    {
        foreach (StudyBlock studyBlock in day.StudyBlocks)
        {
            if (studyBlock.task == task)
            {
                return true;
            }
        }
        return false;
    }

    public virtual TimeSpan GetTimeRemainingInTask(TimeSpan studyTime, Task task)
    {
        // Return the minimum of study time and the task's estimated time
        if (task.estimatedTime >= studyTime)
        {
            return studyTime;
        }
        else
        {
            return task.estimatedTime;
        }
    }
}