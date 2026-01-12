using UnityEngine;
using TMPro;
using System.Collections;

public class SpeakerNameSet : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    private void OnEnable()
    {
        CutsceneEvents.OnSpeakerChanged += UpdateSpeaker;        
    }

    private void OnDisable()
    {
        CutsceneEvents.OnSpeakerChanged -= UpdateSpeaker;       
    }

    private void UpdateSpeaker(string name)
    {
        Debug.Log("SpeakerStep executed: ");
        nameText.text = name;
    }

}
