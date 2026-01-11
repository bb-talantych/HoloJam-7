using UnityEngine;
using TMPro;
using System.Collections;

public class TextPrinter : MonoBehaviour
{
    [SerializeField] TMP_Text textbox;
    [SerializeField] float charDelay = 0.0003f;

    private void OnEnable()
    {
        CutsceneEvents.OnSpeakRequested += PrintText;        
    }

    private void OnDisable()
    {
        CutsceneEvents.OnSpeakRequested -= PrintText;       
    }

    private void PrintText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeRoutine(text));
    }

    private IEnumerator TypeRoutine(string text)
    {
        Debug.Log("Typing text: " + text);
        textbox.text = "";
        foreach(char c in text)
        {
            textbox.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        CutsceneEvents.OnTextFinished?.Invoke();
    }
}
