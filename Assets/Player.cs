using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

  private Behavior[] behaviors = new Behavior[Reference.NumLanes];
  public GameObject laneBGprefab;
  private GameObject[] laneBG = new GameObject[Reference.NumLanes];
  private float[] laneLimits = new float[Reference.NumLanes + 1];

  private Vector3 lastPos = Vector3.zero;
  //private Vector3 delta = Vector3.zero;

	// Use this for initialization
	void Start() {
    float diff = (GameManager.MaxY - GameManager.MinY) / Reference.NumLanes;
    float currY = GameManager.MinY + diff / 2;
    float laneLimitY = GameManager.MinY + diff;
    laneLimits[0] = GameManager.MinY;

    for (int i = 0; i < Reference.NumLanes; i++) {
      laneBG[i] = (GameObject) Instantiate(
        laneBGprefab, new Vector3(0f, currY, 15), laneBGprefab.transform.rotation );
      laneLimits[i+1] = laneLimitY;
      laneLimitY += diff;
      currY += diff;
    }
	}

  // Update is called once per frame
  void Update() {

    //////MOUSE INPUTS/////
    if (Input.GetMouseButtonDown(0)) { //press left click
      lastPos = Camera.main.ScreenToWorldPoint(
        new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

    }  else if (Input.GetMouseButton(0)) { //hold left click
      Vector3 converted = Camera.main.ScreenToWorldPoint(
        new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

      switchLanes(lastPos, converted);
      lastPos = converted;
    }
    /////MOUSE INPUTS//////

	}

  private int determineLane(Vector3 point) {
    int lane = -1;
    for (int i = 0; i < Reference.NumLanes; i++) {
      if (laneLimits[i] <= point.y && point.y < laneLimits[i + 1]) {
        lane = i;
        break;
      }
    }

    if (lane == -1) {
      Debug.Log("wtf negative lane in determineLane " + point);
    }

    return lane;
  }

  private bool validPoint(Vector3 point) {
    return GameManager.MinY < point.y && point.y < GameManager.MaxY;
  }

  private void switchLanes(Vector3 start, Vector3 end) {

    //no significant change
    if (Math.Abs(end.x - start.x) < 0.1f ) {
      return;
    }

    //make sure input is valid
    if (validPoint(start) || validPoint(end)) {
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
    
    //swap for the for loop
    if (startLane > endLane) {
      int temp = startLane;
      startLane = endLane;
      endLane = temp;
    }
   
    for (int i = startLane; i <= endLane; i++) {
      behaviors[i] = b;
      laneBG[i].GetComponent<Plane>().Behavior = b;
    }
  }

}
