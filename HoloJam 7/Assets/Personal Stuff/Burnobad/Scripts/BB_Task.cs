using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonStuff;
using UnityEngine.UI;

public class BB_Task : MonoBehaviour
{
    [SerializeField]
    private GameObject movePointHolder;

    public Vector3 MovePoint
    { get { return movePointHolder.transform.position; } }
    public bool isAssigned
    {
        get;
        private set;
    }

    public float timeToComplete = 10f;

    [SerializeField]
    private Stats statRequirements = default;

    [SerializeField]
    private Slider progressSlider;

    private void OnEnable()
    {
        isAssigned = false;

        progressSlider.value = 0;

        //testing
        statRequirements = new Stats(-3, 3);
    }

    public void Assign(Stats _talentStats)
    {
        Debug.Log(this.name + " is assigned");
        isAssigned = true;

        CompareStat(_talentStats);
    }
    private void CompareStat(Stats _talentStats)
    {
        float speedModifier = 1f;
        speedModifier += _talentStats.strenght >= statRequirements.strenght ? -0.1f : 0.1f;
        speedModifier += _talentStats.dexterity >= statRequirements.dexterity ? -0.1f : 0.1f;
        speedModifier += _talentStats.intellect >= statRequirements.intellect ? -0.1f : 0.1f;
        speedModifier += _talentStats.charisma >= statRequirements.charisma ? -0.1f : 0.1f;

        float finalTime = timeToComplete * speedModifier;
        StartTask(finalTime);

        Debug.Log("modifierTimeToComplete: " + finalTime);
    }
    private void StartTask(float _timeToComplete)
    {
        isAssigned = true;
        Debug.Log(this.name + ": task started");
        StartCoroutine(ITask(_timeToComplete));
    }

    IEnumerator ITask(float _timeToComplete)
    {
        float timeElapsed = 0;
        while(timeElapsed < _timeToComplete)
        {
            timeElapsed += Time.deltaTime;  
            progressSlider.value = timeElapsed/_timeToComplete;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log(this.name + ": task completed");
    }
}
