namespace StorySystem
{
    public interface IStoryGoal
    {
        public GoalStatus GetStatus(bool skipInvoke = false);
    }
}