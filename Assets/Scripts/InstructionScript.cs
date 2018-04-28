using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionScript : MonoBehaviour {

    public GameController controller;
    public Text textLabel;
    public Text numberLabel;
    public List<string> instructionTexts = new List<string>();
    private int instructionNumber;

    // Use this for initialization
    public void Awake () {
        this.gameObject.SetActive(true);
        instructionNumber = 0;
        textLabel.text = instructionTexts[instructionNumber];
        numberLabel.text = (instructionNumber + 1) + "/" + instructionTexts.Count;
    }

    public void NextInstructions()
    {
        if (instructionNumber == instructionTexts.Count-1) return;
        instructionNumber++;
        textLabel.text = instructionTexts[instructionNumber];
        numberLabel.text = (instructionNumber+1) + "/" + instructionTexts.Count;
    }

    public void PrevInstructions()
    {
        if (instructionNumber == 0) return;
        instructionNumber--;
        textLabel.text = instructionTexts[instructionNumber];
        numberLabel.text = (instructionNumber + 1) + "/" + instructionTexts.Count;
    }

    public void StartExerciseFromInstructions()
    {
        controller.timerPaused = false;
        this.gameObject.SetActive(false);
    }
}
