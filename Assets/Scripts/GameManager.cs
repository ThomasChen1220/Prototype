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
    private GameObject currGoal;
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
        Destroy(currGoal);
        Destroy(trail.gameObject);
    }
    public void RestartGame() {
        //clean up the scene
        CleanUpScene();
        InitGame();
    }
    public void OnPlayerTouchGoal() {
        score++;
        scoreText.text = "" + score;
        SpawnGoal();
    }
    public void OnPlayerMissedGoal() {
        //playerLife--;
        //DOTween.Sequence()
        //    .Append(DOTween.To(() => colorAdj.colorFilter.value, x => colorAdj.colorFilter.value = x, Color.red, 0.4f))
        //    .Append(DOTween.To(() => colorAdj.colorFilter.value, x => colorAdj.colorFilter.value = x, Color.white, 0.8f));
        //lifeText.text = "life: " + playerLife;
        SoundManager.instance.resetPickUp();
        SoundManager.instance.PlayMissedSound();
        if (playerLife <= 0)
        {
            OnPlayerNoHealth();
        }
        popUpText.SetActive(false);
        Destroy(currGoal);
        SpawnGoal();
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
    public void SpawnGoal() {
        spawnCounter = spawnRate;
        currGoal = Instantiate(goal);
        currGoal.transform.position 
            = new Vector2(Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight)) * 0.9f;
    }
    private void Update()
    {
        if (!gameEnded)
        {
            spawnCounter -= Time.deltaTime;
            if (spawnCounter < 0)
            {
                OnPlayerMissedGoal();
            }
            if (spawnCounter >= 5)
            {
                //countDown.text = "Get the Ring!";
            }
            else
            {
                currGoal.GetComponent<Goal>().Blink();
                //countDown.text = "Time: " + (int)(spawnCounter + 1);
            }
        }
        
    }
    public void QuitGame() {
        Application.Quit();
    }
}
