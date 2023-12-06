using System;
using UnityEngine;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "PrefsSaveGameContainer", menuName = "StorySystem/PrefsContainer", order = 2)]
    public class PrefsSaveGameContainer : ScriptableObject, ISaveGameContainer
    {
        [SerializeField] private string KeyPhase = "currentphase";
        [SerializeField] private string KeyFlagPrefix = "flag_";
        [SerializeField] private string KeyGoalPrefix = "goalfinished_";

        private int _saveIndex = -1;

        private string GetPhaseKey => KeyPhase + "_save" + _saveIndex;
        private string GetFlagKey(string id) => KeyFlagPrefix + id + "_save" + _saveIndex;
        private string GetGoalKey(string id) => KeyGoalPrefix + id + "_save" + _saveIndex;

        public void LoadGame(int saveIndex)
        {
            Debug.Log("load game");
            _saveIndex = saveIndex;
        }

        public void SaveGame(int saveIndex = -1)
        {
            Debug.Log("save game");
            
            if (saveIndex >= 0)
            {
                _saveIndex = saveIndex;
            }

            PlayerPrefs.Save();
        }

        public string GetCurrentPhaseId() { return PlayerPrefs.GetString(GetPhaseKey); }

        public void SetCurrentPhaseId(string id)
        {
            PlayerPrefs.SetString(GetPhaseKey, id);
            PlayerPrefs.Save();
        }

        public bool GetFlag(string id) { return PlayerPrefs.GetInt(GetFlagKey(id), 0) == 1; }

        public void SetFlag(string id, bool value)
        {
            PlayerPrefs.SetInt(GetFlagKey(id), value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public GoalStatus GetGoalFinishStatus(string id)
        {
            int goal = PlayerPrefs.GetInt(GetGoalKey(id), 0);
            Debug.Log("goal " + id + " pref int: " + goal);
            return (GoalStatus)goal;
        }

        public void SetGoalFinishStatus(string id, GoalStatus status)
        {
            PlayerPrefs.SetInt(GetGoalKey(id), (int)status);
        }
    }
}