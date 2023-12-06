using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem
{
    public class StoryStateViewer : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void OnEnable()
        {
            StorySingleton.OnFlagChanged += OnFlagChanged;
            StorySingleton.OnPhaseChanged += OnPhaseChanged;
            StorySingleton.OnGameLoaded += OnGameLoaded;
        }

        private void OnDisable()
        {
            StorySingleton.OnFlagChanged -= OnFlagChanged;
            StorySingleton.OnPhaseChanged -= OnPhaseChanged;
            StorySingleton.OnGameLoaded -= OnGameLoaded;
        }

        private void OnFlagChanged(string arg1, bool arg2) => RefreshView();

        private void OnGameLoaded() => RefreshView();

        private void OnPhaseChanged(StoryPhase obj) => RefreshView();


        private void RefreshView()
        {
            string flagText = "";
            string exitText = "";
            string goalText = "";
            string phaseText = "";

            string fullText = "";

            string currentPhaseId = StorySingleton.Instance.GetCurrentPhase().PhaseId;

            bool currentPassed = false;

            foreach (StoryPhase phase in StorySingleton.Instance.Configuration.Phases)
            {
                string phaseTemplate = "";

                if (!currentPassed && phase.PhaseId != currentPhaseId)
                {
                    phaseTemplate = "<color=green>{0}</color>";
                }
                else if (phase.PhaseId == currentPhaseId)
                {
                    currentPassed = true;
                    phaseTemplate = "<color=yellow>{0}</color>";
                }
                else
                {
                    phaseTemplate = "<color=red>{0}</color>";
                }

                phaseText += string.Format(phaseTemplate, phase.PhaseId) + "\n";
                fullText += string.Format(phaseTemplate, phase.PhaseId) + "\n";

                foreach (StoryExit exit in phase.Exits)
                {
                    string exitTemplate = "";

                    ExitStatus exitStatus = exit.GetStatus(true);

                    if (exitStatus == ExitStatus.Complete)
                    {
                        exitTemplate = "<color=green>{0}</color>";
                    }
                    else if (exitStatus == ExitStatus.InProgress)
                    {
                        exitTemplate = "<color=yellow>{0}</color>";
                    }
                    else if (exitStatus == ExitStatus.Failed)
                    {
                        exitTemplate = "<color=red>{0}</color>";
                    }
                    else if (exitStatus == ExitStatus.Locked)
                    {
                        exitTemplate = "<color=orange>{0}</color>";
                    }


                    exitText += string.Format(exitTemplate, exit.ExitId) + "\n";
                    fullText += string.Format(exitTemplate, exit.ExitId) + "\n";

                    foreach (StoryGoal goal in exit.Goals)
                    {
                        string goalTemplate = "";

                        GoalStatus goalStatus = goal.GetStatus(true);

                        if (goalStatus == GoalStatus.Complete)
                        {
                            goalTemplate = "<color=green>{0}</color>";
                        }
                        else if (goalStatus == GoalStatus.Failed)
                        {
                            goalTemplate = "<color=red>{0}</color>";
                        }
                        else if (goalStatus == GoalStatus.Locked)
                        {
                            goalTemplate = "<color=orange>{0}</color>";
                        }
                        else
                        {
                            goalTemplate = "<color=yellow>{0}</color>";
                        }

                        goalText += string.Format(goalTemplate, goal.GoalId) + "\n";
                        fullText += string.Format(goalTemplate, goal.GoalId) + "\n";

                        if (goal.Prerequisites.Count > 0)
                        {
                            goalText += "Prereq conditions: ";
                            fullText += "Prereq conditions: ";

                            foreach (var condition in goal.Prerequisites)
                            {
                                //goalText
                                fullText += string.Format((condition.IsFulfilled()
                                        ? "<color=green>{0}</color>"
                                        : "<color=yellow>{0}</color>"),
                                    condition.name + ", ");
                            }
                        }

                        goalText += "Complete conditions: ";
                        fullText += "Complete conditions: ";

                        foreach (var condition in goal.CompleteConditions)
                        {
                            //goalText
                            fullText += string.Format((condition.IsFulfilled()
                                    ? "<color=green>{0}</color>"
                                    : "<color=yellow>{0}</color>"),
                                condition.name + ", ");
                        }


                        if (goal.FailConditions.Count > 0)
                        {
                            goalText += "Fail conditions: ";
                            fullText += "Fail conditions: ";

                            foreach (var condition in goal.FailConditions)
                            {
                                //goalText
                                fullText += string.Format((condition.IsFulfilled()
                                        ? "<color=red>{0}</color>"
                                        : "<color=green>{0}</color>"),
                                    condition.name + ", ");
                            }
                        }

                        goalText += "\n";
                        fullText += "\n";
                    }
                }

                fullText += "\n";
            }

            foreach (KeyValuePair<string, bool> pair in StorySingleton.Instance.GetAllFlags())
            {
                flagText += pair.Key + ": " + pair.Value + "\n";
                fullText += pair.Key + ": " + pair.Value + "\n";
            }

            _text.text = fullText;
            /*string.Format("Phases:\n{0}\nExits:\n{1}\nFlags\n{2}\nGoals:\n{3}", phaseText, exitText,
            flagText, goalText);*/
        }
    }
}