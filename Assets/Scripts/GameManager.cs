using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public Maze mazeInstance;
    public MazeCell cellPrefab;
    private StoryDisplay storyDisplay;

    private LevelDisplay levelDisplay;

    public GameObject entryRoomPrefab;
    public GameObject exitRoomPrefab;
    public PlayableDirector cutsceneDirector;
    public PlayableDirector cutsceneDirector2;

    public Transform playerSpawnPoint;
    public Player playerPrefab;
    private Player playerInstance;
    private Camera playerCamera;


    private Zombie zombieInstance;
    public Zombie zombiePrefab;
    
    private ZombieFast zombiefInstance;
    public ZombieFast zombiefPrefab;

    public int numberOfZombies = 5;
    public int numberOfFastZombies = 2;

    public GameObject keyPrefab, healthPrefab;
    private GameObject key;
    private GameObject health;
    private GameObject storyItem;
    private GameObject entry, exit;

    public static int numberOfKeys = 3;
    public static int numberOfHealths = 2;

    private SaveLevel saveLevelInstance;

    public GameObject storyItemPrefab; // Assign your story item prefab in the Inspector
    private string[] storyFiles;  // Array of story file names

    public int numberOfStories = 0;
    public int level=1;

    private int distance;
    private float maxSpawnDistanceFromPlayer;
    private float minSpawnDistanceFromPlayer; // Minimum distance from the player to spawn zombies

    private void Start()
    {
        StartCoroutine(BeginGame());
        storyDisplay = FindObjectOfType<StoryDisplay>();
        levelDisplay = FindObjectOfType<LevelDisplay>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.L))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Return))
        {
            //mazeInstance.ToggleDoorsInRoom(false); // Close all doors in the current room
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
			//NewLevel();
		}

    }

    //[SerializeField]
    private MazeNavMeshBuilder nmBuilder;

    private void Awake()
    {
        nmBuilder = GetComponent<MazeNavMeshBuilder>();
        saveLevelInstance = new SaveLevel();
    }


    private void LevelSettings()
    {
        level = saveLevelInstance.LoadLvl();
        Debug.Log($"Level loaded: {level}");
        if(level == 0)
        { 
            level = 1;
            saveLevelInstance.SaveLvl(level);
        }
       //tutorial levels 
        if (level == 1) // Assuming the first level is represented by 1
        {
            mazeInstance.size = new IntVector2(5, 5); 
            mazeInstance.roomExpansionChance = 0.5f; 
            numberOfKeys = 2; 
            numberOfZombies = 0; 
            numberOfFastZombies = 0; 
            numberOfHealths = 0;
            numberOfStories = 0; 
        }
        else if (level == 2) 
        {
            mazeInstance.size = new IntVector2(6, 6);
            mazeInstance.roomExpansionChance = 0.4f; 
            numberOfKeys = 2;
            numberOfZombies = 1;
            numberOfFastZombies = 0;
            numberOfHealths = 3; 
            numberOfStories = 0; 
        }
        else if (level == 3)
        {
            /*
            mazeInstance.size = new IntVector2(7, 7);
            mazeInstance.roomExpansionChance = 0.4f;
            numberOfKeys = 2;
            numberOfZombies = 0;
            numberOfFastZombies = 1;
            numberOfHealths = 6;
            numberOfStories = 0; */
            distance = 7;
            minSpawnDistanceFromPlayer = 3.0f;
            maxSpawnDistanceFromPlayer = (float)(distance - 1);
            mazeInstance.size = new IntVector2(7, 7);
            mazeInstance.roomExpansionChance = 0.4f;
            numberOfKeys = 5;
            numberOfZombies = 3;
            numberOfFastZombies = 3;
            numberOfHealths = 26;
            numberOfStories = 0;
        }
        else if (level == 4) 
        {
            mazeInstance.size = new IntVector2(7, 7);
            mazeInstance.roomExpansionChance = 0.4f;
            numberOfKeys = 2; 
            numberOfZombies = 1; 
            numberOfFastZombies = 1; 
            numberOfHealths = 6; 
            numberOfStories = 1; 
        }
       //other levels
        else
        {
            // For higher levels, increase maze size and number of zombies slightly
            int sizeIncrease = level % 10; // The amount by which to increase the maze size and zombie count
            mazeInstance.size = new IntVector2(5 + (int)(sizeIncrease * 0.1f), 5 + (int)(sizeIncrease * 0.1f));
            numberOfKeys = 3 * (int)(sizeIncrease * Random.Range(0.2f, 2f)); // The default number of keys for higher levels
            numberOfZombies = 1 + (int)(sizeIncrease * 0.5f); // Increase number of zombies
            numberOfFastZombies = 1 + (int)(sizeIncrease * 0.2f); // Increase number of fast zombies, half the rate of regular zombies
            numberOfHealths = (int)(numberOfFastZombies * 0.4f) + (int)(numberOfZombies * 0.2f); // The default number of health pickups for higher levels
            numberOfStories = (int)(Random.Range(0, 7) / sizeIncrease); // The default number of story items for higher levels
            mazeInstance.roomExpansionChance = 0.4f + 0.0005f * (sizeIncrease % 10); // Increase room expansion probability
        }

    }
    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

        
        // Instantiate and generate the maze.
        mazeInstance = Instantiate(mazePrefab) as Maze;

        // Adjust game settings based on the level
        LevelSettings();

        //yield return StartCoroutine(mazeInstance.Generate());
        Coroutine mazeGenerator = StartCoroutine(mazeInstance.Generate());
        //StartCoroutine(mazeInstance.Generate());
        

        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            Debug.Log("Cutscene playing.");
        }
        Debug.Log("Cutscene should now be playing");
        

        // Ensure the maze generation coroutine has finished as well.
        yield return mazeGenerator;

        if (mazeInstance != null)
        {
            PlaceEntryAndExitRooms();
            // Instantiate the player and set location.
            SetupPlayer();
            
            //handle baking of navMesh
            nmBuilder.BuildNavMesh();

            //spawn items
            SpawnKeys(numberOfKeys);
            SpawnHealth(numberOfHealths);
            SpawnStoryItems(numberOfStories);


            SetupCamera();
            Debug.Log("Maze generated!!!");
        }
        
        cutsceneDirector.Stop();
        Debug.Log("First cutscene stopped.");

        // Wait for the second cutscene to finish
        // Begin the second cutscene
        if (cutsceneDirector2 != null)
        {
            Debug.Log("Second cutscene playing.");
            cutsceneDirector2.Play();

            // Wait for the second cutscene to finish
            yield return new WaitUntil(() => cutsceneDirector2.state != PlayState.Playing);
            cutsceneDirector2.Stop();
        }
        
        // Instantiate the zombies
        SpawnZombies(numberOfZombies);
        SpawnFastZombies(numberOfFastZombies);


        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.5f);
        Camera.main.depth = 3;

        //disable cutscene view by switching player camera depth to higher one
        if (playerCamera != null) {
            playerCamera.depth = 2; // Reset the depth to make it the active camera
        }

        
        //storyDisplay.message = "Level: 1, 2, 3, 4, 5, 6, 7, 8, 9 , 0";//+level;
        levelDisplay.message = level.ToString();
        levelDisplay.DisplayMessage();
       
    }

    private void SetupPlayer()
    {
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
        playerInstance.transform.Rotate(0f, 180f, 0f);

        //for disable cutscene view in next steps
        GameObject cameraGameObject = GameObject.FindWithTag("PlayerCamera");
        if (cameraGameObject != null) {
            playerCamera = cameraGameObject.GetComponent<Camera>();
        }
        else {
            Debug.LogError("No camera with the tag 'PlayerCamera' was found in the scene.");
        }
    }

    private void SetupCamera()
    {
        CameraFollow cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollowScript != null)
        {
            cameraFollowScript.StartFollowingPlayer(playerInstance.transform);
        }
        else
        {
            Debug.LogError("CameraFollow script not found on the main camera!");
        }
    }

    public Maze maze;
    public Player player;    
    
    public void SaveGame()
    {
        if (playerInstance == null)
        {
            Debug.LogError("Player instance is null");
            return;
        }

        SaveData saveData = new SaveData();
    //number of level
        saveData.levelNumber = level;
    //player position    
        saveData.playerPositionX = playerInstance.transform.position.x;
        saveData.playerPositionY = playerInstance.transform.position.y;
        saveData.playerPositionZ = playerInstance.transform.position.z;
    //number of keyes
        saveData.numberOfCollectedKeyes = playerInstance.keysCollected;
        
        saveLevelInstance.Save(saveData);

    }
    public void LoadGame()
    {
        SaveData saveData = saveLevelInstance.Load();
        if (saveData != null)
        {
            level = saveData.levelNumber;
            if (playerInstance != null)
            {
                playerInstance.transform.position = new Vector3(saveData.getPositionX(), saveData.getPositionY(), saveData.getPositionZ());
                playerInstance.keysCollected = saveData.getKeyes();     
            }
            else
            {
                Debug.LogError("Player instance is null.");
            }
        }
    }
    /**/

    private void PlaceEntryAndExitRooms() {

        //entry room
        Quaternion entryRotation = Quaternion.Euler(0, 90, 0); // Adjust this as needed
        MazeCell entryCell = mazeInstance.GetCell(new IntVector2(0, 0));
        entry = Instantiate(entryRoomPrefab, entryCell.transform.position, entryRotation);

        //exit room
        Quaternion exitRotation = Quaternion.Euler(0, -90, 0); // Adjust this as needed
        MazeCell exitCell = mazeInstance.GetCornerCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)); // Top-right corner
        exit = Instantiate(exitRoomPrefab, exitCell.transform.position, exitRotation);

        mazeInstance.RemoveWallsAtCoordinates(new IntVector2(1, 0));
        mazeInstance.RemoveWallsAtCoordinates1( new IntVector2(mazeInstance.size.x - 2, mazeInstance.size.z - 1));

    }
    

    
    // Adjustments for spawning and door handling
    

/*
    private void SpawnZombies(int count)
    {

        for (int i = 0; i < count; i++)
        {
            MazeCell cell = null;
            float distance;
            do
            {
                cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
                distance = Vector3.Distance(playerInstance.transform.position, cell.transform.position);
            }
            while (distance < minSpawnDistanceFromPlayer); // Keep looking for a cell that is far enough from the player

            zombieInstance = Instantiate(zombiePrefab) as Zombie;
            // Add a random offset within the cell
            zombieInstance.SetLocation(cell);
            zombieInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }*/

    private void SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //float dist = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
            //Debug.Log($"Dist: {dist}");
            MazeCell cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);

            Zombie zombieInstance = Instantiate(zombiePrefab, cell.transform.position, Quaternion.identity) as Zombie;
            //Debug.Log($"Cell: {cell}");
            //Debug.Log($"Distan: {distan}");
            zombieInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }

    private void SpawnFastZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);

            zombiefInstance = Instantiate(zombiefPrefab, cell.transform.position, Quaternion.identity) as ZombieFast;
            zombiefInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }

    private void SpawnKeys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 keyPosition = randomCell.transform.position;
            float floatHeight = 0.04f; // The height above the ground at which the key will float
            keyPosition.y += floatHeight;

            key = Instantiate(keyPrefab, keyPosition, Quaternion.identity);
            key.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
    }

    private void SpawnHealth(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 healthPosition = randomCell.transform.position;
            float floatHeight = 0.04f; // The height above the ground at which the key will float
            healthPosition.y += floatHeight;

            health = Instantiate(healthPrefab, healthPosition, Quaternion.identity);
            health.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }



    private void SpawnStoryItems(int count)
    {
        storyFiles = new string[] {"story1.txt", "story2.txt", "story3.txt", "story4.txt", "story5.txt", "story6.txt", "story7.txt"};
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 itemPosition = randomCell.transform.position;
            float floatHeight = 0.1f;
            itemPosition.y += floatHeight;

            storyItem = Instantiate(storyItemPrefab, itemPosition, Quaternion.identity);
            storyItem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            StoryTrigger storyTrigger = storyItem.GetComponent<StoryTrigger>();
            if (storyTrigger != null && i < storyFiles.Length)
            {
                storyTrigger.storyFileName = storyFiles[i];
            }
        }
    }

    /**/
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    
    public void NewLevel()
    {
        //save the number of current level
        saveLevelInstance.SaveLvl(level + 1);

        RestartGame();
        /*
        

        // Stop all coroutines to prevent them from affecting the new game instance
        StopAllCoroutines();

        // Destroy the maze instance and all its children (including walls, cells, etc.)
        if (mazeInstance != null)
        { Destroy(mazeInstance.gameObject); }

        // Stop the camera from following the destroyed player
        Camera.main.GetComponent<CameraFollow>().StopFollowingPlayer();

        // Destroy the player instance
        if (playerInstance != null)
        { Destroy(playerInstance.gameObject); }

        // Destroy all zombies in the scene
        foreach (var zombie in FindObjectsOfType<Zombie>())
        { Destroy(zombie.gameObject); }
        
        // Destroy all fast zombies in the scene
        foreach (var ZombieFast in FindObjectsOfType<ZombieFast>())
        { Destroy(ZombieFast.gameObject); }

        // Destroy all keys in the scene
        foreach (var key in GameObject.FindGameObjectsWithTag("Key"))
        { Destroy(key.gameObject); }
        
        // Destroy all keys in the scene
        foreach (var storyItem in GameObject.FindGameObjectsWithTag("Story"))
        { Destroy(storyItem.gameObject); }
        
        // Destroy all hearts in the scene
        foreach (var health in GameObject.FindGameObjectsWithTag("Health"))
        { Destroy(health.gameObject); }

        // Destroy all entry in the scene
        if (entry != null)
        { Destroy(entry.gameObject);  entry = null; }

        // Destroy all exit in the scene
        if (exit != null)
        { Destroy(exit.gameObject); exit = null;}

        


        // Start a new game
        StartCoroutine(BeginGame());*/
        //BeginGame();
    }

   


}