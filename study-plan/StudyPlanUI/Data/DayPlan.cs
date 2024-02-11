namespace StudyPlan.Data;

public class DayPlan
{
    public DateOnly date { get; set; }
    private List<TimeBlock> timeBlocks = new List<TimeBlock>();
    public List<TimeBlock> TimeBlocks
    {
        get { return timeBlocks; }
    }

    private List<StudyBlock> studyBlocks = new List<StudyBlock>();
    public List<StudyBlock> StudyBlocks
    {
        get { return studyBlocks; }
    }

    public DayPlan(DateOnly date)
    {
        this.date = date;
        CreateTimeBlocks();
    }

    public void CreateTimeBlocks()
    {
        // Some default values (9:00am to 12:00pm and 1:00pm to 5:00pm)
        timeBlocks = new List<TimeBlock>
        {
            new TimeBlock(new TimeOnly(9, 0, 0), new TimeSpan(3, 0, 0)),
            new TimeBlock(new TimeOnly(13, 0, 0), new TimeSpan(4, 0, 0)),
        };
    }
}