using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] GameObject NameBox;
    [SerializeField] Text dialogText;
    [SerializeField] Text NPCName;
    [SerializeField] int LettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnHideDialog;
    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    Dialog dialog;
    int currentLine = 0;
    bool isTyping = false;
    bool skip = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTyping)
        {
            skip = true;
        }
        if (!isTyping)
        {
            skip = false;
        }
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }


    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        if (dialog.name != "")
        {
            NPCName.text = dialog.name;
        }
        else
        {
            NPCName.text = "You"; // This part will be changed to the player's name (in case the player has one)
        }
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            if (!skip) // I have absolutely no idea why keydown does not work there
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(1f / LettersPerSecond);
            }
            else
            {
                skip = false;
                dialogText.text = line;
                break;
            }
        }
        isTyping = false;
    }

    public void HandleUpdate() // of dialog
    {
        if ( (Input.GetKeyDown(KeyCode.E) && !isTyping) || Input.GetKeyDown(KeyCode.Escape))
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count && !Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                dialogBox.SetActive(false);
                currentLine = 0;
                OnHideDialog?.Invoke();
            }
        }
    }
}
