namespace StorySystem
{
    public interface ISaveGameContainer
    {
        public void LoadGame(int saveIndex);
        public void SaveGame(int saveIndex);
        public string GetCurrentPhase();

        public void SetPhase(string id);

        public bool GetFlag(string id);

        public void SetFlag(string id, bool value);
    }
}