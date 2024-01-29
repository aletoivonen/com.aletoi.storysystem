namespace StorySystem
{
    public interface ISaveGameContainer
    {
        public void LoadGame(int saveIndex);
        public void SaveGame(int saveIndex = -1);
        public string GetCurrentPhaseId();

        public void SetCurrentPhaseId(string id);

        public bool GetFlag(string id);

        public void SetFlag(string id, bool value);

        public void SetGoalFinishStatus(string id, GoalStatus status);

        public GoalStatus GetGoalFinishStatus(string id);

        public void SetExitStatus(string id, ExitStatus status);

        public ExitStatus GetExitStatus(string id);
    }
}