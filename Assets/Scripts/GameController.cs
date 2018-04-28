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

    [HideInInspector]
    public bool timerPaused;
    private int currentPlayerNum;
    private bool bothAnswered;
    private float timePassed;

    public GameObject ballPrefab;
    private ArrayList ballsInPlay;
    private ArrayList ballResults;
	//private BallMover ballMover;

    // Muutin takasin startiksi, jotta Ground generoidaan ensin ja siltä saadaan spawni paikat
    void Start()
    {
        CancelInputs();
        bothAnswered = false;
        timerPaused = true;

		var ground = FindObjectOfType<GroundGenerator> ();
        players = new Player[2];
		players[0] = CreatePlayerInstance(1, new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x + Random.Range(1,3), ground.GetLeftHeight()+0.6f), 1);
		players[1] = CreatePlayerInstance(-1, new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x - Random.Range(1,3), ground.GetRightHeight()+0.6f), 2); 
        ballsInPlay = new ArrayList();
        ballResults = new ArrayList();
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

                bool testForAWinner = false;
                foreach (Vector2Int i in ballResults) testForAWinner = testForAWinner || Mathf.Abs(i.y) == 1;  

                if (ballResults.Count == 2 || (testForAWinner && ballResults.Count > 0 && ballsInPlay.Count == 0)) // Either both results are ready or one player won before another could even shoot
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
    private Player CreatePlayerInstance(int dir, Vector2 pos, int playerNum)
    {
        Vector3 readyPositions = playerPrefab.transform.position;
        Player createdPlayer = GameObject.Instantiate(playerPrefab, new Vector3(pos.x, pos.y, readyPositions.z), new Quaternion()).GetComponent<Player>();
        createdPlayer.direction = dir; createdPlayer.position = pos.x; createdPlayer.playerNumber.text = playerNum.ToString();
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
        timerPaused = true;
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
