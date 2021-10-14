using UnityEngine;

public class QuestKillObjectiveUIController : QuestObjectiveUIController
{
    [SerializeField] private TMPro.TMP_Text Header, ProgressIndicator;
    [SerializeField] private UnityEngine.UI.Slider ProgressBar;

    public override void Refresh(QuestObjectiveInstance raw_instance)
    {
        QuestKillObjectiveInstance instance = raw_instance as QuestKillObjectiveInstance;
        Header.SetText(instance.Archetype.Title);
        ProgressBar.value = (float)instance.Current / (float)instance.CastedArchetype.Count;
        ProgressIndicator.SetText($"{instance.Current}/{instance.CastedArchetype.Count}");
    }
}