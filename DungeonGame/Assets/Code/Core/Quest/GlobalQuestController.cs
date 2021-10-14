using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalQuestController : MonoBehaviour
{
    private static GlobalQuestController Instance;
    private static List<ActiveQuest> ActiveQuests = new List<ActiveQuest>();
    private static Dictionary<ActiveQuest, Action> QuestCompleteCallbacks = new Dictionary<ActiveQuest, Action>();

    [SerializeField] private GameObject QuestObjectiveHolderPrefab;
    [SerializeField] private Transform ActiveQuestListRootObject;
    [SerializeField] private GameObject RootQuestPanel;

    public static void BeginQuest(QuestArchetype quest, Action OnQuestComplete = null)
    {
        GlobalDialogueController.ShowDialogue(quest.Dialogue.LinesOfText, OnDialogueComplete);
        ActiveQuest activeQuest = new ActiveQuest(quest);
        ActiveQuests.Add(activeQuest);

        if (OnQuestComplete != null)
        {
            QuestCompleteCallbacks.Add(activeQuest, OnQuestComplete);
        }
    }

    private static void OnDialogueComplete()
    {
        ActiveQuest quest = ActiveQuests[ActiveQuests.Count - 1];

        GameObject questObjectiveHolder = Instantiate(Instance.QuestObjectiveHolderPrefab);
        questObjectiveHolder.transform.SetParent(Instance.ActiveQuestListRootObject); ;
        quest.AssignObjectiveHolder(questObjectiveHolder);

        UpdateObjectiveUIController(quest);
        quest.CurrentObjective.BeginObjective(); // begin first objective.
    }

    public static void UpdateObjectiveUIController(ActiveQuest quest)
    {
        if (quest.ObjectiveHolder.transform.childCount != 0)
        {
            Destroy(quest.ObjectiveHolder.transform.GetChild(0));
        }
        GameObject go = Instantiate(quest.CurrentObjective.Archetype.ActiveQuestListUIPrefab);
        go.transform.SetParent(quest.ObjectiveHolder.transform);

        QuestObjectiveUIController ctr = go.GetComponent<QuestObjectiveUIController>();
        quest.AssignUIController(ctr);
    }

    public static void CompleteQuest(ActiveQuest quest)
    {
        Destroy(quest.ObjectiveHolder);
        // NOTE: THIS COULD BREAK THINGS IF EVER CALLED FROM LOOP OVER ACTIVEQUESTS
        ActiveQuests.Remove(quest);

        if (QuestCompleteCallbacks.ContainsKey(quest))
        {
            QuestCompleteCallbacks[quest]?.Invoke();
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            RootQuestPanel.SetActive(!RootQuestPanel.activeSelf);
        }
    }
}
