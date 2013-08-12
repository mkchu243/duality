using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 
  public enum GameState{restart, running, paused, gameOver};
  public static GameState state;
  public static float[] laneLimits = new float[Reference.NumLanes + 1]; //the Y lane limits
  public static float[] laneCenters = new float[Reference.NumLanes]; //these go from bottom to top

  public const float MaxY = 18;
  public const float MinY = -18;
  
  void Awake(){
    //do calculations for lane limits
    float diff = (MaxY - MinY) / Reference.NumLanes;
    float currY = MinY + diff / 2;
    float laneLimitY = MinY + diff;
    laneLimits[0] = MinY;

    for (int i = 0; i < Reference.NumLanes; i++) {
      laneCenters[i] = currY;
      laneLimits[i+1] = laneLimitY;
      
      laneLimitY += diff;
      currY += diff;
    }
  }
  
	// Use this for initialization
  void Start() {
    state = GameState.restart;
  }
	
  // Update is called once per frame
  void Update() {
    switch( state ){
    case GameState.restart:
      EnemyManager.Instance.Restart();
      Player.Instance.Restart();
      state = GameState.running;
      break;
    //case GameState.running:
    //  break;
    }
  }
  
  public static int DetermineLane(Vector3 point) {
    int lane = -1;
    for (int i = 0; i < Reference.NumLanes; i++) {
      if (laneLimits[i] <= point.y && point.y < laneLimits[i + 1]) {
        lane = i;
        break;
      }
    }
    if (lane == -1)
      Debug.Log("wtf negative lane in DetermineLane " + point);
    return lane;
  }
}
