using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Npc
{
    public float InteractionRange;
    public QuestArchetype quest;
    public QuestGiverState State;

    private void Awake()
    {
        State = GlobalContinuityController.GetOrCreateQuestGiverState(NpcTrackingId);
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (Input.GetKeyDown(KeyCode.T) && distanceToPlayer <= InteractionRange)
        {
            if (State.hasGivenQuest == false)
            {
                GlobalQuestController.BeginQuest(quest, OnQuestCompleted);
                State.hasGivenQuest = true;
            }
            else if (State.hasCompletedQuest == false)
            {
                GlobalDialogueController.ShowDialogue(quest.AwaitingCompletionDialogue.LinesOfText);
            }
            else if (State.hasGivenReward == false)
            {
                GlobalDialogueController.ShowDialogue(quest.RewardDialogue.LinesOfText);
                State.hasGivenReward = true;

                OnQuestRewarded();
            }
            else {
                GlobalDialogueController.ShowDialogue(quest.AlreadyRewardedDialogue.LinesOfText);
            }
        }
    }

    protected virtual void OnQuestRewarded()
    {
        PlayerController.Instance.Info.Gold += quest.RewardGold;
        PlayerController.Instance.Info.GainXp(quest.RewardXp);
        PlayerController.Instance.RefreshStatUI();
    }

    protected virtual void OnQuestCompleted()
    {
        GlobalDialogueController.ShowDialogue(quest.CompletionDialogue.LinesOfText);
        State.hasCompletedQuest = true;
    }
}

public class QuestGiverState
{

    public bool hasGivenQuest = false;
    public bool hasCompletedQuest = false;
    public bool hasGivenReward = false;
}