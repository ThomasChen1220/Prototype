using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float spawnRate;
    public GameObject goal;
    public TextMeshProUGUI countDown;
    public GameObject player;

    public static GameManager instance;
    public static float screenWidth, screenHeight;

    private float spawnCounter;
    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = (screenTopRight.x - screenBottomLeft.x) / 2;
        screenHeight = (screenTopRight.y - screenBottomLeft.y) / 2;

        spawnCounter = spawnRate;
    }
    public void OnPlayerTouchGoal() {

    }
    public void OnPlayerMissedGoal() {
        spawnCounter = spawnRate;
    }
    public void SpawnGoal() {

    }
    private void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter < 0)
        {
            OnPlayerMissedGoal();
        }
        if (spawnCounter >= 5)
        {
            countDown.text = "Get the Ring!";
        }
        else
        {
            countDown.text = "Time: "+(int)(spawnCounter+1);
        }
        
    }
}
