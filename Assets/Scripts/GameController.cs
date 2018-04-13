using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private Player[] players;

    public Button playerOneAnswer;
    public Button playerTwoAnswer;
    public AnswerField inputs;
    public Text timerNumber;

    private bool timerPaused;
    private int currentPlayerNum;
    private bool finished;
    private float timePassed;

    public GameObject ball;
	private BallMover ballMover;

    // Use this for initialization
    void Awake()
    {
        CancelInputs();
        finished = false;
        timerPaused = false;
        players = new Player[2]; players[0] = new Player(); players[1] = new Player();
        ballMover = ball.GetComponent<BallMover>();
    }


    //   void Start () {
    //	inputs.answerBtn.onClick.AddListener (onThrowClicked);
    //	ballMover = ball.GetComponent<BallMover> ();
    //}


    void FixedUpdate()
    {
        if (!timerPaused && finished) timePassed += Time.deltaTime*3;
        else if(!timerPaused) timePassed += Time.deltaTime;

        timerNumber.text = timePassed.ToString("0.00");

        // If ready, when each player 
        if (finished) foreach (Player player in players) if (!player.shotLeft && player.answerTime <= timePassed) player.Shoot(ballMover);

    }


    // Give the players answer
    public void OnAnswerClicked(){
        // Set the answers for the player
        Player currentPlayer = players[currentPlayerNum - 1];
        currentPlayer.answerStatus = true;
        currentPlayer.answerVelocity = float.Parse(inputs.velocityField.text);
        currentPlayer.answerAngle = float.Parse(inputs.angleField.text);
        currentPlayer.answerTime = timePassed;


        //// Saved for testing purposes
        //ballMover.ResetPosition();
        //ballMover.ThrowBall(float.Parse(inputs.velocityField.text), float.Parse(inputs.angleField.text));

        CancelInputs();

        // Check whether both players are ready. If they are, move to the next step.
        finished = true;
        foreach (Player player in players) finished = finished && player.answerStatus;

        if (finished) timePassed = 0;

    }


    // Set inputfields active, if the player hasnt given an answer
    public void ShowAnswerInputs(int playerNumber)
    {
        if (players[playerNumber - 1].answerStatus) return;
        timerPaused = true;
        currentPlayerNum = playerNumber;
        inputs.label.text = inputs.label.text.Substring(0, inputs.label.text.Length - 1) + playerNumber;
        inputs.gameObject.SetActive(true);
    }


    // Null the input fields and set them inactive.
    public void CancelInputs()
    {
        timerPaused = false;
        inputs.gameObject.SetActive(false);
        inputs.velocityField.text = "";
        inputs.angleField.text = "";
    }

}
