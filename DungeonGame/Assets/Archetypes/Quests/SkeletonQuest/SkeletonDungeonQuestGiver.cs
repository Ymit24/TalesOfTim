using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDungeonQuestGiver : QuestGiver
{
    public GameObject Gate;
    public Sprite GateOpen;

    protected override void OnQuestRewarded()
    {
        base.OnQuestCompleted();

        Gate.GetComponent<SpriteRenderer>().sprite = GateOpen;
        Destroy(Gate.GetComponent<BoxCollider2D>());
    }
}
