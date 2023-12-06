namespace StorySystem
{
    public interface IStoryState
    {
        public void LoadFromSave(ref ISaveGameContainer saveGameContainer);

        public void SetCurrentPhase(StoryPhase phase);

        public bool GetFlag(string id);

        public void SetFlag(string id, bool value);

        public GoalStatus GetGoalFinishStatus(string id);

        public void SetGoalFinishStatus(string id, GoalStatus status);
    }
}