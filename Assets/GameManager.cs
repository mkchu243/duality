using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 
  public enum GameState{restart, running, paused, gameOver};
  public static GameState state;

  private EnemyManager enemyManager;
  private Player player;

  public const float MaxY = 18;
  public const float MinY = -18;
  
	// Use this for initialization
	void Start() {
    state = GameState.restart;
    enemyManager = gameObject.GetComponent<EnemyManager>();
    player = gameObject.GetComponent<Player>();

    Plane.InitStaticVars();
    GUIManager.InitStaticVars();
	}
	
	// Update is called once per frame
	void Update() {
    switch( state ){
    case GameState.restart:
      enemyManager.Restart();
      player.Restart();
      state = GameState.running;
      break;
    //case GameState.running:
    //  break;
    }
	}
}
