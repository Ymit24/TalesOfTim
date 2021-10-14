using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestArchetype : ScriptableObject
{
    public DialogueObject Dialogue;
    public DialogueObject AwaitingCompletionDialogue;
    public DialogueObject CompletionDialogue;
    public DialogueObject RewardDialogue;
    public DialogueObject AlreadyRewardedDialogue;
    public List<QuestObjective> Objectives;

    public int RewardGold;
    public int RewardXp;
}

public class ActiveQuest
{
    public QuestArchetype Archetype;
    public List<QuestObjectiveInstance> ObjectiveInstances;
    public QuestObjectiveUIController ObjectiveUIController;
    public GameObject ObjectiveHolder;

    public QuestObjectiveInstance CurrentObjective
    {
        get
        {
            return ObjectiveInstances[0];
        }
    }

    public ActiveQuest(QuestArchetype archetype)
    {
        Archetype = archetype;
        ObjectiveInstances = new List<QuestObjectiveInstance>();
        foreach (QuestObjective obj in Archetype.Objectives)
        {
            ObjectiveInstances.Add(obj.CreateObjectiveInstance(this));
        }
    }

    public void AssignUIController(QuestObjectiveUIController controller)
    {
        ObjectiveUIController = controller;
        RefreshUI();
    }

    public void AssignObjectiveHolder(GameObject objectiveHolder)
    {
        ObjectiveHolder = objectiveHolder;
    }

    public void RefreshUI()
    {
        ObjectiveUIController.Refresh(CurrentObjective);
    }

    public void CompleteObjective()
    {
        CurrentObjective.CleanupObjective();
        ObjectiveInstances.RemoveAt(0);
        if (ObjectiveInstances.Count == 0)
        {
            GlobalQuestController.CompleteQuest(this);
            return;
        }
        GlobalQuestController.UpdateObjectiveUIController(this);
        CurrentObjective.BeginObjective();
        RefreshUI();
    }
}
public abstract class QuestObjectiveInstance
{
    public ActiveQuest Quest;
    public QuestObjective Archetype;

    public QuestObjectiveInstance(ActiveQuest quest, QuestObjective archetype)
    {
        Quest = quest;
        Archetype = archetype;
    }

    public abstract void BeginObjective();
    public abstract void CleanupObjective();
}

public abstract class QuestObjective : ScriptableObject
{
    // Default Quest Objective is for polymorphism
    public abstract QuestObjectiveInstance CreateObjectiveInstance(ActiveQuest quest);

    public string Title;
    public GameObject ActiveQuestListUIPrefab;
}