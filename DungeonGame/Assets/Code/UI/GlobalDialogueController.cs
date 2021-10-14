using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDialogueController : MonoBehaviour
{
    private static GlobalDialogueController Instance;
    public GameObject RootDialogueWindow;
    public TMPro.TMP_Text DialogueBody;

    private float text_index_counter = 0;
    private int currentLine = 0;
    private bool isWriting = false;
    private List<string> lines;

    private bool isPaused = false;
    private Action currentCallback;

    private const float TextWriteSpeed = 64;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ShowDialogue(List<string> lines, Action onCompleteCallback = null)
    {
        Instance.text_index_counter = 0;
        Instance.currentLine = 0;
        Instance.lines = lines;
        Instance.isWriting = true;
        Instance.text_index_counter = 0;
        Instance.RootDialogueWindow.SetActive(true);
        Instance.currentCallback = onCompleteCallback;
    }

    private void Update()
    {
        if (isWriting == false)
            return;
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPaused = false;
                DialogueBody.SetText(" ");
                if (currentLine >= lines.Count)
                {
                    isWriting = false;
                    RootDialogueWindow.SetActive(false);
                    currentCallback?.Invoke();
                }
            }
        }
        else
        {
            text_index_counter += TextWriteSpeed * Time.deltaTime;

            string slice = lines[currentLine].Substring(
                0,
                Mathf.Clamp(
                    Mathf.FloorToInt(text_index_counter),
                    1,
                    lines[currentLine].Length
                )
            );
            DialogueBody.SetText(slice);

            if (text_index_counter >= lines[currentLine].Length)
            {
                text_index_counter = 0;
                isPaused = true;
                currentLine++;
            }

        }
    }
}
