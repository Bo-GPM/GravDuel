using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    [SerializeField] private GameObject playerToSpawn;
    [SerializeField] private GameObject playerSpawnLocation;
    public GameObject playerInstance;
    
    // Record score and game state
    private int playerCurrentScore = 0;
    private int playerHighScore = 0;
    private bool gameState = true;
    
    // Save all Instantiated objects here for reset
    public List<GameObject> resetableGameObjects = new List<GameObject>();

    [Header("UI")] 
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text playingScoreBoard;
    [SerializeField] private Text resultCurrentScoreBoard;
    [SerializeField] private Text resultHighScoreBoard;
    
    [Header("Sound")]
    public AudioSource[] audioFiles;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        playerInstance = Instantiate(playerToSpawn, playerSpawnLocation.transform.position, Quaternion.identity);
    }

    void Start()
    {
        AddToScore(0);  //Initialize the score
        
        // Audio Init
        audioFiles = GetComponents<AudioSource>();
        audioFiles[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug list counting wrong
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.LogWarning($"There are {resetableGameObjects.Count} items in the list");
        }
    }
    
    // Call it when player failed
    public void PlayerFailed()
    {
        // TODO: 1. set scores
        //       2. display failed screen
        //       3. (Optional) make player exploded :)
        if (playerCurrentScore > playerHighScore)
        {
            playerHighScore = playerCurrentScore;
        }

        resultCurrentScoreBoard.text = $"Your Score: {playerCurrentScore}";
        resultHighScoreBoard.text = $"High Score: {playerHighScore}";
        
        SwapPanelActivation();
        
        //It should set gameState to false
        gameState = !gameState;
        
        //Pause the game
        Time.timeScale = 0;

        // Debug list count wrong
        Debug.LogWarning($"At this frame, there are {resetableGameObjects.Count} items in the list");
    }

    public void ResetBattle()
    {
        // TODO: clear battleground, reset player's pos, swap scores
        
        SwapPanelActivation();
        
        // Reload scene here (Trying) (Nope, it's causing a lot problems)
        // string sceneName = SceneManager.GetActiveScene().name;
        // SceneManager.LoadScene(sceneName);
        
        // Try to reset the scene Mk.II plan
        int ActuallyRemovedItems = 0;
        int tempListLength = resetableGameObjects.Count;
        for (int i = 0; i < tempListLength; i++)
        {
            Destroy(resetableGameObjects[0]);
            resetableGameObjects.Remove(resetableGameObjects[0]);
            ActuallyRemovedItems++;
        }
        Debug.LogWarning($"There should be {resetableGameObjects.Count} objs being removed, and {ActuallyRemovedItems} objs removed. tempLength is {tempListLength}");
        
        
        // It should set gameState to true
        gameState = !gameState;
        if (!gameState)
        {
            Debug.LogWarning("GS state is wrong");
        }
        
        // Resume the game
        Time.timeScale = 1;
        
        // Reset black hole's pos
        playerInstance.transform.position = playerSpawnLocation.transform.position;
        
        // Reset Current Score
        playerCurrentScore = 0;
        AddToScore(0);
    }

    private void SwapPanelActivation()
    {
        if (playingPanel.activeInHierarchy)
        {
            resultPanel.SetActive(true);
            playingPanel.SetActive(false);
        }
        else
        {
            resultPanel.SetActive(false);
            playingPanel.SetActive(true);
        }
    }
    
    // Find THIS game object, and remove it from the list
    public void SearchGameObjectToRemoveFromList(GameObject targetObj)
    {
        for (int i = 0; i < resetableGameObjects.Count; i++)
        {
            if (resetableGameObjects[i] == targetObj)
            {
                resetableGameObjects.Remove(targetObj);
            }
        }
    }

    // Use this function to add and update scoreboard
    public void AddToScore(int tempScore)
    {
        playerCurrentScore += tempScore;
        playingScoreBoard.text = $"Score: {playerCurrentScore}";
    }
}
