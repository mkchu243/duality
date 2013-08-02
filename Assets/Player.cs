using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {
  public GameObject laneprefab;
  private const float initialLife = 5;
  
  private GameObject[] lanes = new GameObject[Reference.NumLanes];
  private float[] laneLimits = new float[Reference.NumLanes + 1];
  private float score;
  private float life;

  private Vector3 lastPos = Vector3.zero;

	// Use this for initialization
	void Start() {
    float diff = (GameManager.MaxY - GameManager.MinY) / Reference.NumLanes;
    float currY = GameManager.MinY + diff / 2;
    float laneLimitY = GameManager.MinY + diff;
    laneLimits[0] = GameManager.MinY;

    for (int i = 0; i < Reference.NumLanes; i++) {
      lanes[i] = (GameObject) Instantiate(
        laneprefab, new Vector3(0f, currY, 15), laneprefab.transform.rotation );
      lanes[i].transform.parent = transform;

      laneLimits[i+1] = laneLimitY;
      laneLimitY += diff;
      currY += diff;
    }

    Restart();
	}

  public void Restart() {
    score = 0;
    life = initialLife;
    for (int i = 0; i < Reference.NumLanes; i++) {
      lanes[i].GetComponent<Plane>().Behavior = Behavior.particle;
    }
  }

  // Update is called once per frame
  void Update() {
    switch( GameManager.state ){
      case GameManager.GameState.running:
        //////MOUSE INPUTS/////
        if (Input.GetMouseButtonDown(0)) { //press left click
          lastPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        }  else if (Input.GetMouseButton(0)) { //hold left click
          Vector3 converted = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

          //@update, determine which lanes to change
          switchLanes(lastPos, converted);

          lastPos = converted; //for mouse inputs
        }
        /////MOUSE INPUTS//////
        if (life <= 0) {
          GameManager.state = GameManager.GameState.gameOver;
        }

        break;
    }
	}

  public void handleEnemy(Enemy enemy) {
    int lane = determineLane(enemy.transform.position);
    if( lanes[lane].GetComponent<Plane>().Behavior == enemy.Behavior ){
      score += 100;
    } else {
      life--;
    }
  }

  private int determineLane(Vector3 point) {
    int lane = -1;
    for (int i = 0; i < Reference.NumLanes; i++) {
      if (laneLimits[i] <= point.y && point.y < laneLimits[i + 1]) {
        lane = i;
        break;
      }
    }
    if (lane == -1)
      Debug.Log("wtf negative lane in determineLane " + point);
    return lane;
  }

  //read the input and switch the lanes to the right behavior
  private void switchLanes(Vector3 start, Vector3 end) {
    //no significant change, don't do anything
    if (Math.Abs(end.x - start.x) < 0.1f ) {
      return;
    }

    //make sure input is valid
    if (validPoint(start) || validPoint(end)) {
      //-0.11f because the max is technically out of range
      start = new Vector3(start.x, Mathf.Clamp(start.y, GameManager.MinY, GameManager.MaxY-0.1f));
      end = new Vector3(end.x, Mathf.Clamp(end.y, GameManager.MinY, GameManager.MaxY-0.1f));
    } else {
      return;
    }
    
    //determine behavior
    //particle is right(+) wave if left(-)
    Behavior b;
    if (end.x - start.x > 0) { //positive
      b = Behavior.particle;
    } else {
      b = Behavior.wave;
    }

    int startLane = determineLane(start);
    int endLane = determineLane(end);
    
    //make sure start lane is smaller one
    if (startLane > endLane) {
      int temp = startLane;
      startLane = endLane;
      endLane = temp;
    }
   
    for (int i = startLane; i <= endLane; i++) {
      lanes[i].GetComponent<Plane>().Behavior = b;
    }
  }

  private bool validPoint(Vector3 point) {
    return GameManager.MinY < point.y && point.y < GameManager.MaxY;
  }

  public float Score {
    get { return score; }
  }

  public float Life {
    get { return life; }
  }
}
