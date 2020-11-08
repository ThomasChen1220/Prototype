﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private GameObject[] powerUps;
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

    [SerializeField]
    private int goalNum = 1;
    [SerializeField]
    private List<Crash> ships;
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
    //TODO: move this part to player
    //public void OnTrapReady() {
    //    currPlayer.GetComponent<PlaceTraps>().TurnOnTrail();
    //    if (gameStatsSave.tutorialShown == false)
    //    {
    //        popUpText.SetActive(true);
    //    }
    //}
    public void CleanUpScene()
    {
        for(int i = currGoals.Count-1; i >= 0; i--)
        {
            var g = currGoals[i].gameObject;
            currGoals.RemoveAt(i);
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
        SoundManager.instance.PlayPickedUpSphereSound();
        if (SoundManager.instance.CheckMaxed())
        {
            SpawnPowerUp();
        }
        else
        {
            SpawnGoal();
        }
    }
    public void OnPlayerMissedGoal(GameObject g) {
        SoundManager.instance.PlayMissedSound();
        currGoals.Remove(g);
        Destroy(g.gameObject);
        popUpText.SetActive(false);
        SpawnGoal();
    }
    public void SpawnPowerUp() {
        Debug.Log("Should place powerup");
        GameObject powerUp = Instantiate(powerUps[Random.Range(0, powerUps.Length)]);
        powerUp.transform.position
            = new Vector2(Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight)) * 0.8f;
        currGoals.Add(powerUp);
        SoundManager.instance.resetPickUp();
    }
    public void SpawnGoal()
    {
        if (currGoals.Count >= goalNum)
        {
            return;
        }
        GameObject currGoal = Instantiate(goal);
        currGoal.transform.position
            = new Vector2(Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight)) * 0.8f;
        currGoals.Add(currGoal);
    }
    public void OnGameEnd() {
        //scoreText.text = "Wasted";
        spawnCounter = 1000;
        gameEnded = true;
        popUpText.SetActive(false);
        StartCoroutine(DoEndEffect());
    }
    public void AddShip(Crash c)
    {
        ships.Add(c);
        goalNum = ships.Count / 5 + 1;
        goalNum = Mathf.Min(3, goalNum);
        if (goalNum > currGoals.Count)
        {
            SpawnGoal();
        }
    }
    public void RemoveShip(Crash c)
    {
        ships.Remove(c);
        goalNum = ships.Count / 5 + 1;
        goalNum = Mathf.Min(3, goalNum);
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
