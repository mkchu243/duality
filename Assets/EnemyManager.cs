using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyManager : MonoBehaviour {
 
  //game vars const
  private const float SpawnX = 30;
  private const float KillX = -27;
  
  private const int NumEnemies = 20;
  
  private const float InitialSpawnRatio = 0.25f;
  private const float FinalSpawnRatio = 0.95f;
  private const float InitialSpeedMult = 1;//1;
  private const float FinalSpeedMult = 5;
  private const float InitialSpawnTime = 3.5f;//3;
  private const float FinalSpawnTime = 1;
  
  //game vars
  private float spawnRatio;
  private float spawnAttributeRatio = 0.5f;
  private float speedMult;
  private float spawnTime;

  //vars
  private System.Random rng;
  private Vector3[] spawnY = new Vector3[Reference.NumLanes];
  private Timer spawnTimer;
  public Enemy enemyPrefab;
  private Queue<Enemy> activeEnemies;
  private Stack<Enemy> inactiveEnemies;

  private Player player;

	// Use this for initialization
	void Start() {
	  float diff = (GameManager.MaxY - GameManager.MinY)/Reference.NumLanes;
    float currY = GameManager.MinY + diff/2;
    
    //calculate the spawn locations
    for( int i = 0; i < Reference.NumLanes; i++ ){
      spawnY[i] = new Vector3(SpawnX,currY,0);
      currY += diff;
    }
    
    activeEnemies = new Queue<Enemy>();
    inactiveEnemies = new Stack<Enemy>();
    for( int i = 0; i < NumEnemies; i++){
      inactiveEnemies.Push(
        (Enemy)Instantiate(enemyPrefab, new Vector3(0f,0f,-100f), Quaternion.identity));
      inactiveEnemies.Peek().transform.parent = transform; //TODO not sure if needed, I think this makes the enemies children of the manager
    }
    
    spawnTimer = gameObject.AddComponent<Timer>();
    rng = new System.Random();
    player = gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update() {
    switch (GameManager.state){
      case GameManager.GameState.running:
        //reclaim enemies
        while (activeEnemies.Count > 0 &&
          activeEnemies.Peek().transform.position.x <= KillX) {
            Enemy e = activeEnemies.Dequeue();
            player.handleEnemy(e);
            e.Deactivate();
            inactiveEnemies.Push(e);
        }

        //spawn new enemies
        if (spawnTimer.time <= 0){
          SpawnEnemies();
          spawnTimer.Restart(spawnTime);
        }
        break;
    }
	}
  
  public void Restart() {
    while (activeEnemies.Count > 0) {
      Enemy e = activeEnemies.Dequeue();
      e.Deactivate();
      inactiveEnemies.Push(e);
    }

    spawnTimer.Restart(0);
    spawnRatio = InitialSpawnRatio;
    speedMult = InitialSpeedMult;
    spawnTime = InitialSpawnTime;
  }

  private void SpawnEnemies() {
    for (int i = 0; i < Reference.NumLanes; i++) {
      if (inactiveEnemies.Count != 0 && spawnRatio < rng.NextDouble()) {
        Enemy e = inactiveEnemies.Pop();

        //generate behavior
        Behavior b;
        if (spawnAttributeRatio < rng.NextDouble()) {
          b = Behavior.particle;
        } else {
          b = Behavior.wave;
        }

        e.Spawn(b, speedMult, spawnY[i]);
        activeEnemies.Enqueue(e);
      }
    }
  }
}
