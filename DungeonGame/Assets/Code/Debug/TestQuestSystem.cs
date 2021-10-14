using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestSystem : MonoBehaviour
{
    public QuestArchetype Quest;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalQuestController.BeginQuest(Quest);
            Destroy(this);
        }
    }
}
