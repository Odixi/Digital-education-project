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
    public GameObject EndGamePopup;
    public Text EndGameLabel;

    [HideInInspector]
    public bool timerPaused;
    private int currentPlayerNum;
    private bool bothAnswered;
    private float timePassed;
    private float firstAnswerTime;

    public GameObject ballPrefab;
    private ArrayList ballsInPlay;
    private ArrayList ballResults;
	//private BallMover ballMover;

    // Muutin takasin startiksi, jotta Ground generoidaan ensin ja siltä saadaan spawni paikat
    void Start()
    {
        CancelInputs();
        EndGamePopup.SetActive(false);
        bothAnswered = false;
        timerPaused = true;
        firstAnswerTime = 0;

        var ground = FindObjectOfType<GroundGenerator> ();
        players = new Player[2];
		players[0] = CreatePlayerInstance(1, new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x + Random.Range(1,3), ground.GetLeftHeight()+0.6f), 1);
		players[1] = CreatePlayerInstance(-1, new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x - Random.Range(1,3), ground.GetRightHeight()+0.6f), 2); 
        ballsInPlay = new ArrayList();
        ballResults = new ArrayList();
		GetComponent<ObstacleGenerator>().enabled = true; // Players must be initilized first
    }


    void FixedUpdate()
    {
        if (!timerPaused && bothAnswered) timePassed += Time.deltaTime*3;
        else if(!timerPaused) timePassed += Time.deltaTime;

        if (bothAnswered) timerNumber.text = (timePassed - firstAnswerTime).ToString("0.00"); 
        else timerNumber.text = timePassed.ToString("0.00");

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
        if (firstAnswerTime == 0) firstAnswerTime = timePassed;

        // Check whether both players are ready. If they are, move to the next step.
        bothAnswered = true;
        foreach (Player player in players) bothAnswered = bothAnswered && player.answerStatus;


        if (bothAnswered) timePassed = firstAnswerTime;

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
        string winnerText;
        if (countOfSuccesses == 1 || countOfUtterFailures == 1)
        {
            winnerText = "Voittaja löytyi!";
            foreach (Vector2Int i in ballResults)
            {
                if (i.y == 1)
                {
                    winnerText = "Voittaja on " + i.x;                           
                    break;
                }
                else if (i.y == -1)
                {
                    winnerText = i.x == 1 ? "Voittaja on 2" : "Voittaja on 1";   
                    break;
                }
            }
        }
        else
        {
            winnerText = "Uusiks...";                                           
        }

        EndGameLabel.text = winnerText;
        EndGamePopup.SetActive(true);

    }



    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void EndGame()
    {
        Application.LoadLevel("Menu");
    }

    

}
