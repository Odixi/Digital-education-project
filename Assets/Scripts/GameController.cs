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
    public GameObject playerPrefab;

    private bool timerPaused;
    private int currentPlayerNum;
    private bool bothAnswered;
    private float timePassed;

    public GameObject ballPrefab;
    private ArrayList ballsInPlay;
    private ArrayList ballResults;
	//private BallMover ballMover;

    // Use this for initialization
    void Awake()
    {
        CancelInputs();
        bothAnswered = false;
        timerPaused = false;
        players = new Player[2]; players[0] = CreatePlayerInstance(1, -7, 1); players[1] = CreatePlayerInstance(-1, 7, 2); //TODO: Randomize the positions a little
        ballsInPlay = new ArrayList();
        ballResults = new ArrayList();
        //ballMover = ballPrefab.GetComponent<BallMover>();
    }


    //   void Start () {
    //	inputs.answerBtn.onClick.AddListener (onThrowClicked);
    //	ballMover = ball.GetComponent<BallMover> ();
    //}


    void FixedUpdate()
    {
        if (!timerPaused && bothAnswered) timePassed += Time.deltaTime*3;
        else if(!timerPaused) timePassed += Time.deltaTime;

        timerNumber.text = timePassed.ToString("0.00");

        // If ready, when each player 
        if (bothAnswered) foreach (Player player in players) {

                if (ballResults.Count > 0 && ballsInPlay.Count == 0)
                {
                    StopGameAndShowResults();
                }

                if (!player.shotLeft && player.answerTime <= timePassed)
                {
                    BallMover ballShot = player.Shoot(ballPrefab);
                    ballsInPlay.Add(ballShot);
                    ballShot.controller = this;
                }
        }

    }


    // Give the players answer
    public void OnAnswerClicked(){
        // Set the answers for the player
        Player currentPlayer = players[currentPlayerNum - 1];
        currentPlayer.answerStatus = true;
        currentPlayer.answerVelocity = float.Parse(inputs.velocityField.text);
        currentPlayer.answerAngle = float.Parse(inputs.angleField.text);
        currentPlayer.answerTime = timePassed;

        CancelInputs();

        // Check whether both players are ready. If they are, move to the next step.
        bothAnswered = true;
        foreach (Player player in players) bothAnswered = bothAnswered && player.answerStatus;

        if (bothAnswered) timePassed = 0;

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

    // Instantiate a player based on the data given
    private Player CreatePlayerInstance(int dir, float posX, int playerNum)
    {
        Vector3 readyPositions = playerPrefab.transform.position;
        Player createdPlayer = GameObject.Instantiate(playerPrefab, new Vector3(posX, readyPositions.y, readyPositions.z), new Quaternion()).GetComponent<Player>();
        createdPlayer.direction = dir; createdPlayer.position = posX; createdPlayer.playerNumber.text = playerNum.ToString();
        if (dir < 0)
        {
            Vector3 throwPointInverted = createdPlayer.throwPoint.transform.localPosition;
            createdPlayer.throwPoint.transform.localPosition = new Vector3(throwPointInverted.x*dir, throwPointInverted.y, throwPointInverted.z);
        }
        return createdPlayer;
    }


    public void BallDestroyed(BallMover destructed, int hitSuccess)
    {
        ballResults.Add(new Vector2Int(destructed.thrower, hitSuccess));
        ballsInPlay.Remove(destructed);
        GameObject.Destroy(destructed.gameObject);
    }

    private void StopGameAndShowResults()
    {
        int countOfSuccesses = 0;
        int countOfUtterFailures = 0;
        foreach (Vector2Int i in ballResults) {
            if (i.y == 1) countOfSuccesses++;
            else if (i.y == -1) countOfUtterFailures++;
        } 
        if (countOfSuccesses == 1 || countOfUtterFailures == 1)
        {
            timerNumber.text = "Voittaja löytyi!";
            foreach (Vector2Int i in ballResults)
            {
                if (i.y == 1)
                {
                    timerNumber.text = "Voittaja on " + i.x;                            //TODO: An actual banner for showing the winner
                    break;
                }
                else if (i.y == -1)
                {
                    timerNumber.text = i.x == 1 ? "Voittaja on 2" : "Voittaja on 1";    //TODO: An actual banner for showing the winner
                    break;
                }
            }
        }
        else
        {
            timerNumber.text = "Uusiks...";                                             //TODO: An actual banner for showing the winner
        }
    }

}
