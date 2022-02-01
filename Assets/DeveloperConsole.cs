using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


[System.Serializable]
public class IntEvent : UnityEvent<int> { }
[System.Serializable]
public class FloatEvent : UnityEvent<float> { }
[System.Serializable]
public class StringEvent : UnityEvent<string> { }
[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }
public class DeveloperConsole : MonoBehaviour
{
    [Header("Lists with functions")]
    public List<VoidCommand> voidCommands;
    public List<IntCommand> intCommands;
    public List<FloatCommand> floatCommands;
    public List<StringCommand> stringCommands;
    public List<BoolCommand> boolCommands;

    [Header("User preferences")]
    [SerializeField]
    private KeyCode openConsole;
    [SerializeField]
    private Color highlightedSuggestionColor;
    [SerializeField]
    private Color underLineColor;
    [SerializeField]
    private Color functionTypeColor;
    [SerializeField]
    private Color successfulCommandColor;
    [SerializeField]
    private Color failedCommandColor;
    [SerializeField]
    private bool underLineSuggestion;
    [SerializeField]
    private bool boldSuggestion;

    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI suggestionsTMP;
    [SerializeField]
    private TMP_InputField consoleInput;
    [SerializeField]
    private GameObject consoleObject;

    List<SuggestedCommand> suggestionsList = new List<SuggestedCommand>();

    private int highlightedSuggestonIndex = 0;
    private string lastInput;
    private bool firstTimeTab = true;
    private bool userTabbed = false;

    private bool consoleActive = false;

    private void Start()
    {
        for (int i = 0; i < voidCommands.Count; i++)
        {
            voidCommands[i].Type = "void";
        }
        for (int i = 0; i < floatCommands.Count; i++)
        {
            floatCommands[i].Type = "float";
        }
        for (int i = 0; i < intCommands.Count; i++)
        {
            intCommands[i].Type = "int";
        }
        for (int i = 0; i < stringCommands.Count; i++)
        {
            stringCommands[i].Type = "string";
        }
        for (int i = 0; i < boolCommands.Count; i++)
        {
            boolCommands[i].Type = "bool";
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(openConsole))
        {
            if(!consoleActive)
            {
                consoleActive = true;
                consoleObject.SetActive(true);
                consoleInput.Select();
                consoleInput.ActivateInputField();
            }

        }

        if (consoleObject.activeSelf)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                consoleActive = false;
                ClearInput();
                firstTimeTab = true;
                consoleObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(suggestionsList.Count > 0)
                {
                    if (!firstTimeTab)
                    {
                        highlightedSuggestonIndex++;
                        if (highlightedSuggestonIndex > suggestionsList.Count - 1)
                            highlightedSuggestonIndex = 0;
                    }

                    userTabbed = true;
                    firstTimeTab = false;
                    ShowSuggestions();
                    consoleInput.text = suggestionsList[highlightedSuggestonIndex].key;
                    consoleInput.caretPosition = suggestionsList[highlightedSuggestonIndex].key.Length;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (suggestionsList.Count > 0)
                {
                    highlightedSuggestonIndex++;
                    if (highlightedSuggestonIndex > suggestionsList.Count - 1)
                        highlightedSuggestonIndex = 0;

                    userTabbed = true;
                    ShowSuggestions();
                    consoleInput.text = suggestionsList[highlightedSuggestonIndex].key;
                    consoleInput.caretPosition = suggestionsList[highlightedSuggestonIndex].key.Length;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (suggestionsList.Count > 0)
                {
                    highlightedSuggestonIndex--;
                    if (highlightedSuggestonIndex < 0)
                        highlightedSuggestonIndex = suggestionsList.Count - 1;

                    userTabbed = true;
                    ShowSuggestions();
                    consoleInput.text = suggestionsList[highlightedSuggestonIndex].key;
                    consoleInput.caretPosition = suggestionsList[highlightedSuggestonIndex].key.Length;
                }
            }

        }
    }


    private void ClearInput()
    {
        consoleInput.text = "";
        suggestionsTMP.text = "";
        suggestionsList.Clear();
        highlightedSuggestonIndex = 0;
    }

    public void ProcessLetters(string input)
    {
        if(userTabbed)
        {
            userTabbed = false;
            return;
        }
        string [] arguments = input.Split(' ');

        suggestionsList.Clear();
        lastInput = input;

        string prefix = arguments[0].ToLower();

        for (int i = 0; i < voidCommands.Count; i++)
        {
            if (voidCommands[i].key.Contains(prefix))
            {
                suggestionsList.Add(new SuggestedCommand(voidCommands[i].key, voidCommands[i].Type));
            }
        }

        for (int i = 0; i < intCommands.Count; i++)
        {
            if (intCommands[i].key.Contains(prefix))
            {
                suggestionsList.Add(new SuggestedCommand(intCommands[i].key, intCommands[i].Type));
            }
        }

        for (int i = 0; i < floatCommands.Count; i++)
        {
            if (floatCommands[i].key.Contains(prefix))
            {
                suggestionsList.Add(new SuggestedCommand(floatCommands[i].key, floatCommands[i].Type));
            }
        }

        for (int i = 0; i < stringCommands.Count; i++)
        {
            if (stringCommands[i].key.Contains(prefix))
            {
                suggestionsList.Add(new SuggestedCommand(stringCommands[i].key, stringCommands[i].Type));
            }
        }

        for (int i = 0; i < boolCommands.Count; i++)
        {
            if (boolCommands[i].key.Contains(prefix))
            {
                suggestionsList.Add(new SuggestedCommand(boolCommands[i].key, boolCommands[i].Type));
            }
        }

        ShowSuggestions();
    }

    private void ShowSuggestions()
    {
        suggestionsTMP.text = "";
        for (int i = 0; i < suggestionsList.Count; i++)
        {
            if (i == highlightedSuggestonIndex)
            {
                string suggestionTmp = AddColorToText(suggestionsList[i].key + AddBracketsToText(suggestionsList[i].type), highlightedSuggestionColor);
                if (boldSuggestion)
                    suggestionTmp = AddBoldToText(suggestionTmp);
                if (underLineSuggestion)
                    suggestionTmp = AddUnderlineToText(suggestionTmp, underLineColor);
                suggestionsTMP.text += suggestionTmp + "\n";
            }
            else
            {
                suggestionsTMP.text += suggestionsList[i].key + AddColorToText(AddBracketsToText(suggestionsList[i].type), functionTypeColor)+ "\n";
            }
        }
    }

    public void ProcessCommand(string input)
    {
        string[] arguments = input.Split(' ');

        bool successfulCommand = true;

        if (suggestionsList.Count <= 0)
            return;

        if (arguments.Length == 1)
        {
            if (!ProcessVoidCommand(arguments[0].ToLower()))
            {
                successfulCommand = false;
            }
        }
        else
        {
            if (!ProcessBoolCommand(arguments[0].ToLower(), arguments[1].ToLower()))
            {
                if (!ProcessIntCommand(arguments[0].ToLower(), arguments[1]))
                {
                    if (!ProcessFloatCommand(arguments[0].ToLower(), arguments[1]))
                    {
                        if (!ProcessStringCommand(arguments[0].ToLower(), arguments[1].ToLower()))
                        {

                            successfulCommand = false;
                        }
                    }
                }
            }
        }

        ClearInput();

        if(successfulCommand)
        {
            suggestionsTMP.text = AddColorToText("Command used succesfully", successfulCommandColor); 

        }
        else
        {
            suggestionsTMP.text = AddColorToText("Command not found", failedCommandColor);

        }

        firstTimeTab = true;
        consoleInput.ActivateInputField();

    }

    private string AddBracketsToText(string text)
    {
        return  " <" + text + ">";

    }

    private string AddColorToText(string text, Color color)
    {
        string tmp = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        return tmp;
    }

    private string AddUnderlineToText(string text, Color underLineColor)
    {
        return "<u color=#" + ColorUtility.ToHtmlStringRGBA(underLineColor) + ">" + text + "</u>";
    }

    private string AddBoldToText(string text)
    {
        return "<b>" + text + "</b>";
    }

    private bool ProcessVoidCommand(string prefix)
    {
        for (int i = 0; i < voidCommands.Count; i++)
        {
            if (voidCommands[i].key.Equals(prefix))
            {
                voidCommands[i].function.Invoke();
                return true;
            }
        }
        return false;
    }

    private bool ProcessIntCommand(string prefix, string argument)
    {
        int intArgument;
        if (!int.TryParse(argument, out intArgument))
            return false;

        for (int i = 0; i < intCommands.Count; i++)
        {
            if (intCommands[i].key.Equals(prefix))
            {
                intCommands[i].function.Invoke(intArgument);
                return true;
            }
        }

        return false;
    }

    private bool ProcessFloatCommand(string prefix, string argument)
    {
        float floatArgument;
        if (!float.TryParse(argument, out floatArgument))
            return false;

        for (int i = 0; i < floatCommands.Count; i++)
        {
            if (floatCommands[i].key.Equals(prefix))
            {
                floatCommands[i].function.Invoke(floatArgument);
                return true;
            }
        }

        return false;
    }

    private bool ProcessStringCommand(string prefix, string argument)
    {
        for (int i = 0; i < stringCommands.Count; i++)
        {
            if (stringCommands[i].key.Equals(prefix))
            {
                stringCommands[i].function.Invoke(argument);
                return true;
            }
        }

        return false;
    }

    private bool ProcessBoolCommand(string prefix, string argument)
    {
        bool boolArgument;

        if (argument == "true" || argument == "1")
            boolArgument = true;
        else if (argument == "false" || argument == "0")
            boolArgument = false;
        else
            return false;

        for (int i = 0; i < boolCommands.Count; i++)
        {
            if (boolCommands[i].key.Equals(prefix))
            {
                boolCommands[i].function.Invoke(boolArgument);
                return true;
            }
        }

        return false;
    }

}

public struct SuggestedCommand
{
    public string key;
    public string type;

    public SuggestedCommand(string key, string type)
    {
        this.key = key;
        this.type = type;
    }
}

[System.Serializable]
public class VoidCommand
{
    public UnityEvent function;
    public string key;
    private string type;
    public string Type { get => type; set => type = value; }
}

[System.Serializable]
public class IntCommand
{
    public IntEvent function;
    public string key;
    private string type;
    public string Type { get => type; set => type = value; }
}

[System.Serializable]
public class FloatCommand
{
    public FloatEvent function;
    public string key;
    private string type;
    public string Type { get => type; set => type = value; }
}

[System.Serializable]
public class StringCommand
{
    public StringEvent function;
    public string key;
    private string type;
    public string Type { get => type; set => type = value; }
}

[System.Serializable]
public class BoolCommand
{
    public BoolEvent function;
    public string key;
    private string type;
    public string Type { get => type; set => type = value; }

}
