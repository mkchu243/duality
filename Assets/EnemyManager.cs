using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyManager : MonoBehaviour {
  private static EnemyManager instance;
 
  //game vars const
  private const float SpawnX = 30;
  public const float KillX = -27;
  
  private const int NumEnemies = 20;
  
  private const float InitialSpawnRatio = 0.25f;
  private const float FinalSpawnRatio = 0.95f;
  private const float InitialSpeedMult = 11;//1;
  private const float FinalSpeedMult = 5;
  private const float InitialSpawnTime = 0.5f;//3;
  private const float FinalSpawnTime = 1;
  
  //game vars
  private float spawnRatio;
  private float spawnAttributeRatio = 0.5f;
  private float speedMult;
  private float spawnTime;

  //vars
  private System.Random rng;
  private Vector3[] spawnPoints = new Vector3[Reference.NumLanes];
  private Timer spawnTimer;
  public Enemy enemyPrefab;
  private List<Enemy>[] activeEnemies;
  private Stack<Enemy> inactiveEnemies;

  void Awake(){
    Instance = this;
  }
  
	// Use this for initialization
  void Start() {
    activeEnemies = new List<Enemy>[Reference.NumLanes];
    inactiveEnemies = new Stack<Enemy>();
    
    for( int i = 0; i < Reference.NumLanes; i++){
      activeEnemies[i] = new List<Enemy>();
      spawnPoints[i] = new Vector3(SpawnX, GameManager.laneCenters[i], 0);
    }
    
    for( int i = 0; i < NumEnemies; i++){
      inactiveEnemies.Push(
        (Enemy)Instantiate(enemyPrefab, new Vector3(0f,0f,-100f), Quaternion.identity));
      inactiveEnemies.Peek().transform.parent = transform; //TODO not sure if needed, I think this makes the enemies children of the manager
    }

    spawnTimer = gameObject.AddComponent<Timer>();
    rng = new System.Random();
  }
	
	// Update is called once per frame
  void Update() {
    switch (GameManager.state){
      case GameManager.GameState.running:
        //spawn new enemies
        if (spawnTimer.time <= 0){
          SpawnEnemies();
          spawnTimer.Restart(spawnTime);
        }
        break;
    }
  }
  
  public void Restart() {
    foreach (List<Enemy> l in activeEnemies){
      foreach (Enemy e in l) {
        e.Deactivate();
        inactiveEnemies.Push(e);
      }
      l.Clear();
    }

    spawnTimer.Restart(0);
    spawnRatio = InitialSpawnRatio;
    speedMult = InitialSpeedMult;
    spawnTime = InitialSpawnTime;
  }
  
  public void RemoveEnemy(Enemy e){
    Player.Instance.handleEnemy(e);
    inactiveEnemies.Push(e);
    e.Deactivate();
    activeEnemies[GameManager.DetermineLane(e.transform.position)].Remove(e);
  }

  private void SpawnEnemies() {
    for (int lane = 0; lane < Reference.NumLanes; lane++) {
      if (inactiveEnemies.Count != 0 && spawnRatio < rng.NextDouble()) {
        Enemy e = inactiveEnemies.Pop();

        //generate behavior
        Behavior b;
        if (spawnAttributeRatio < rng.NextDouble()) {
          b = Behavior.particle;
        } else {
          b = Behavior.wave;
        }

        e.Spawn(b, speedMult, spawnPoints[lane]);
        activeEnemies[lane].Add(e);
      }
    }
  }
  
  //setters and getters
  public Behavior[] getAlerts(float cutoff){
    Behavior[] ret = new Behavior[Reference.NumLanes];
    for(int i = 0; i < activeEnemies.Length; i++){
      int j;
      for( j = 0; j < activeEnemies[i].Count; j++){
        if( activeEnemies[i][j].transform.position.x >= cutoff ){
          ret[i] = activeEnemies[i][j].Behavior;
          break;
        }
      }
      
      if( j == activeEnemies[i].Count ){
        ret[i] = Behavior.nonExistant;
      }
    }
    return ret;
  }
	
  public static EnemyManager Instance { get; private set; }
}
