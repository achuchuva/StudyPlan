namespace StudyPlan.Data;

public class Scheduler
{
    public TimeSpan studyPeriod = TimeSpan.FromHours(1);
    protected List<StudyTask> tasks;

    public Scheduler(List<StudyTask> tasks)
    {
        this.tasks = new List<StudyTask>(tasks);
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
            // Skip ahead if all tasks are already complete
            if (tasks.Count == 0)
            {
                break;
            }

            // Assign the study blocks of each day
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
                // Ensure there are still some tasks left to complete
                if (tasks.Count == 0)
                {
                    return;
                }

                // Get the time available in the time block
                TimeSpan remainingStudyTime = GetTimeAvailableInTimeBlock(timeBlock.duration);

                // Convert the day date and time block start time to DateTime
                DateTime currentTime = new DateTime(day.date.Year, day.date.Month, day.date.Day, timeBlock.startTime.Hour, timeBlock.startTime.Minute, timeBlock.startTime.Second);

                // Select the task
                StudyTask selectedTask = SelectTask(currentTime);

                // Get the study time based on the time available in the time block and the selected task
                TimeSpan studyTime = GetTimeRemainingInTask(remainingStudyTime, selectedTask);

                // Create a new study block and add it to the day
                day.StudyBlocks.Add(new StudyBlock(selectedTask, timeBlock.startTime, studyTime));

                // Update the time block with the study time
                timeBlock.UpdateTimeBlock(studyTime);

                // Decrease the selected task's estimated time to completion
                selectedTask.DecreaseEstimatedTime(studyTime);

                // Update the task index for future task selection
                UpdateTasks(selectedTask);
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

    public virtual StudyTask SelectTask(DateTime currentDate)
    {
        // Get the task based on index
        double priority = double.NegativeInfinity;
        int taskIndex = 0;

        for (int i = 0; i < tasks.Count; i++)
        {
            TimeSpan remainingTime = tasks[i].dueDate - currentDate;
            double taskPriority = tasks[i].estimatedTime.TotalSeconds / remainingTime.TotalSeconds;

            if (taskPriority > priority)
            {
                priority = taskPriority;
                taskIndex = i;
            }
        }
        return tasks[taskIndex];
    }

    public virtual TimeSpan GetTimeRemainingInTask(TimeSpan studyTime, StudyTask task)
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

    public virtual void UpdateTasks(StudyTask task)
    {
        // Remove task if it has been completed
        if (task.estimatedTime == TimeSpan.Zero)
        {
            tasks.Remove(task);
        }
    }
}