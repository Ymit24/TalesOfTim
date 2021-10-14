using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestKillObjective", menuName = "Quests/Kill Objective")]
public class QuestKillObjective : QuestObjective
{
    public EnemyArchetype Target;
    public int Count;

    public override QuestObjectiveInstance CreateObjectiveInstance(ActiveQuest quest)
    {
        return new QuestKillObjectiveInstance(quest, this);
    }
}

public class QuestKillObjectiveInstance : QuestObjectiveInstance
{
    public QuestKillObjective CastedArchetype
    {
        get
        {
            return Archetype as QuestKillObjective;
        }
    }
    public int Current;

    public QuestKillObjectiveInstance(ActiveQuest quest, QuestKillObjective archetype) : base (quest, archetype)
    {
    }

    public override void BeginObjective()
    {
        EnemyController.OnEnemyKilled += OnEnemyKilled;
    }

    public override void CleanupObjective()
    {
        EnemyController.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(EnemyArchetype archetype)
    {
        if (archetype.Equals(CastedArchetype.Target))
        {
            Current++;
            Quest.RefreshUI();

            if (Current == CastedArchetype.Count)
            {
                // Completed quest
                Quest.CompleteObjective();
            }
        }
    }
}