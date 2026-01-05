using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneEvents : MonoBehaviour
{
    public GameObject fadeInScreen;
    public GameObject charOneIdle;
    public GameObject charTwoIdle;
    public GameObject charTwoPose;

    public GameObject textBox;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    private List<System.Func<IEnumerator>> events;
    private int eventIndex = 0;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        events = new List<System.Func<IEnumerator>>
        {
            EventStarter,
            EventOne,
            EventTwo   
        };
       StartCurrentEvent();
    }

    void StartCurrentEvent()
    {
        IEnumerator routine = events[eventIndex]();
        StartCoroutine(routine);
    }

    IEnumerator EventStarter()
    {
        //event 0
        yield return new WaitForSeconds(1);
        fadeInScreen.SetActive(false);
        yield return new WaitForSeconds(0.4f);

        mainTextObject.SetActive(true);
        textToSpeak = "Witness Meeeeee";
        RunText();
        textBox.SetActive(true);
        charTwoPose.SetActive(true);
        yield return StartCoroutine(TextDelayRoutine());
    }
    
    IEnumerator EventOne()
    {
        nextButton.SetActive(false);
        //event 1
        charName.GetComponent<TMPro.TMP_Text>().text = "Kronii";

        textToSpeak = "Oh boy...";
        RunText();
        yield return StartCoroutine(TextDelayRoutine());
    }

        IEnumerator EventTwo()
    {
        nextButton.SetActive(false);
        //event 1
        charName.GetComponent<TMPro.TMP_Text>().text = "Bae";
        charTwoPose.SetActive(false);
        charTwoIdle.SetActive(true);
        textToSpeak = "Whazzup Kronii!?";
        RunText();
        yield return StartCoroutine(TextDelayRoutine());

        
    }

    public void NextButton()
    {
        if (eventIndex >= events.Count - 1)
            return;

        eventIndex++;
        nextButton.SetActive(false);
        StartCurrentEvent();
    }

    private void RunText()
    {
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

    }

    private IEnumerator TextDelayRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.1f);
        nextButton.SetActive(true);
    }
    
}




