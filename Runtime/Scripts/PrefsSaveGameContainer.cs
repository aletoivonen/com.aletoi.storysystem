using System;
using UnityEngine;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "PrefsSaveGameContainer", menuName = "StorySystem/PrefsContainer", order = 2)]
    public class PrefsSaveGameContainer : ScriptableObject, ISaveGameContainer
    {
        [SerializeField] private string KeyPhase = "currentphase";
        [SerializeField] private string KeyFlagPrefix = "flag_";

        private int _saveIndex = -1;

        private string GetPhaseKey => KeyPhase + "_save" + _saveIndex;
        private string GetFlagKey(string id) => KeyFlagPrefix + id + "_save" + _saveIndex;

        public void LoadGame(int saveIndex)
        {
            Debug.Log("load game");
            _saveIndex = saveIndex;
        }

        public void SaveGame(int saveIndex)
        {
            Debug.Log("save game");
            //TODO overwrite another index, need to read all flags
            _saveIndex = saveIndex;
            PlayerPrefs.Save();
        }

        public string GetCurrentPhase()
        {
            return PlayerPrefs.GetString(KeyPhase);
        }

        public void SetPhase(string id)
        {
            PlayerPrefs.SetString(KeyPhase, id);
            PlayerPrefs.Save();
        }

        public bool GetFlag(string id) { return PlayerPrefs.GetInt(KeyFlagPrefix + id, 0) == 1; }

        public void SetFlag(string id, bool value)
        {
            PlayerPrefs.SetInt(KeyFlagPrefix + id, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}