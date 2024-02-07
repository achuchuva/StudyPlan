namespace StudyPlanService.Tests;

public class TimeBlockTest
{

    [Fact]
    public void TestUpdateTimeBlock()
    {
        TimeBlock timeBlock = new TimeBlock(new TimeOnly(9, 0, 0), new TimeSpan(3, 0, 0));

        timeBlock.UpdateTimeBlock(new TimeSpan(1, 30, 0));

        Assert.Equal(new TimeBlock(new TimeOnly(12, 0, 0), TimeSpan.Zero), timeBlock);
    }
}