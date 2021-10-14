using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class DialogueObject : ScriptableObject
{
    public List<string> LinesOfText;

    public virtual void OnDialogueComplete()
    { /*  MAYBE HAVE CUSTOM DERIVED DIALOGUES DO STUFF  */ }
}