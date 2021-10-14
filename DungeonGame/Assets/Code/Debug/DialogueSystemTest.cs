using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystemTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalDialogueController.ShowDialogue(
                new List<string>()
                    {
                        "Line 1.",
                        "Line 2."
                    },
                OnDialogueComplete
            );
        }
    }

    private void OnDialogueComplete()
    {
        Debug.Log("Dialogue Completed!");
    }
}
