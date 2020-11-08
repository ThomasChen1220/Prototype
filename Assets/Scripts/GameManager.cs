using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public float spawnRate;
    public GameObject goal;
    public GameObject currPlayer;
    public GameObject playerPrefab;
    public TextMeshProUGUI scoreText;
    public GameObject restart;
    public Volume postprocessing;
    public float spawnIntervel = 2f;
    public GameObject popUpText;
    public GameStats gameStatsSave;

    private ColorAdjustments colorAdj;

    public static GameManager instance;
    public static float screenWidth, screenHeight;

    private float spawnCounter;
    private int score = 0;
    public List<GameObject> currGoals;
    private int playerLife = 3;
    private bool gameEnded = true;
    private ParticleSystem trail;

    public List<Crash> ships;
    private void Awake()
    {
        ships = new List<Crash>();
        postprocessing.profile.TryGet(out colorAdj);
    }

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = (screenTopRight.x - screenBottomLeft.x) / 2;
        screenHeight = (screenTopRight.y - screenBottomLeft.y) / 2;

        currGoals = new List<GameObject>();
    }
    public void InitGame()
    {
        SoundManager.instance.PlayGameBGM();
        gameEnded = false;
        score = 0;
        playerLife = 3;
        scoreText.text = "" + score;
        currPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        trail = currPlayer.GetComponentInChildren<ParticleSystem>();
        popUpText.GetComponent<StickTo>().target = currPlayer.transform;

        SoundManager.instance.resetPickUp();
        Cursor.visible = false;
        SpawnGoal();
    }
    public void OnPlaceTrap()
    {
        popUpText.SetActive(false);
        gameStatsSave.tutorialShown = true;
    }
    public void OnTrapReady() {
        currPlayer.GetComponent<PlaceTraps>().TurnOnTrail();
        if (gameStatsSave.tutorialShown == false)
        {
            popUpText.SetActive(true);
        }
    }
    public void CleanUpScene()
    {
        for(int i = 0; i < currGoals.Count; i++)
        {
            var g = currGoals[i].gameObject;
            if(g)
                Destroy(g);
        }
        Destroy(trail.gameObject);
    }
    public void RestartGame() {
        //clean up the scene
        CleanUpScene();
        InitGame();
    }
    public void OnPlayerTouchGoal(GameObject g) {
        score++;
        scoreText.text = "" + score;
        currGoals.Remove(g);
        Destroy(g.gameObject);
        SpawnGoal();
    }
    public void OnPlayerMissedGoal(GameObject g) {
        SoundManager.instance.PlayMissedSound();
        currGoals.Remove(g);
        Destroy(g.gameObject);
        popUpText.SetActive(false);
        SpawnGoal();
    }
    public void SpawnGoal()
    {
        GameObject currGoal = Instantiate(goal);
        currGoal.transform.position
            = new Vector2(Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight)) * 0.8f;
        currGoals.Add(currGoal);
    }
    public void OnPlayerNoHealth() {
        currPlayer.GetComponent<Crash>().OnCrash();
    }
    public void OnGameEnd() {
        //scoreText.text = "Wasted";
        spawnCounter = 1000;
        gameEnded = true;
        StartCoroutine(DoEndEffect());
    }
    private IEnumerator DoEndEffect() {
        //do the death effects
        while (ships.Count > 0)
        {
            if (ships[0] != null)
                ships[0].OnCrash();
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }
        yield return new WaitForSeconds(1f);
        restart.gameObject.SetActive(true);
        Cursor.visible = true;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnGoal();
        }    
    }
    public void QuitGame() {
        Application.Quit();
    }
}
