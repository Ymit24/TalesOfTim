using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalContinuityController : MonoBehaviour
{
    private static GlobalContinuityController Instance;
    private static ContinuityState State;

    public static bool CanSpawn(string trackingId)
    {
        foreach (Npc n in State.KilledNpcs)
        {
            if (n.NpcTrackingId == trackingId)
            {
                return false;
            }
        }
        return true;
    }

    public static QuestGiverState GetOrCreateQuestGiverState(string npcTrackingId)
    {
        if (State.NpcQuestGiverStates.ContainsKey(npcTrackingId) == false)
        {
            QuestGiverState state = new QuestGiverState();
            State.NpcQuestGiverStates.Add(npcTrackingId, state);
            return state;
        }
        return State.NpcQuestGiverStates[npcTrackingId];
    }

    public static void KillNpc(Npc npc)
    {
        State.KilledNpcs.Add(npc);
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // later load this
        State = new ContinuityState();
    }
}


public class ContinuityState
{
    public List<Npc> KilledNpcs = new List<Npc>();
    public Dictionary<string, QuestGiverState> NpcQuestGiverStates = new Dictionary<string, QuestGiverState>();
}