using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
  private int id;
  private float speed;
  private Behavior behavior;

	// Use this for initialization
	void Start () {
    gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
  	switch (GameManager.state) {
      case GameManager.GameState.running:
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
        if( transform.position.x <= EnemyManager.KillX ){
          EnemyManager.Instance.RemoveEnemy(this);
        }
        break;
    }

    Vector3 rotationVelocity = new Vector3(45, 90, 1);
    transform.Rotate(rotationVelocity * Time.deltaTime);
    //transform.RotateAround();//rotationVelocity);// * Time.deltaTime);
	}

  void setModel() {
    switch (behavior) {
      case Behavior.particle:
        transform.FindChild("ParticleModel").gameObject.SetActive(true);
        transform.FindChild("WaveModel").gameObject.SetActive(false);
        break;
      case Behavior.wave:
        transform.FindChild("ParticleModel").gameObject.SetActive(false);
        transform.FindChild("WaveModel").gameObject.SetActive(true);
        break;
    }
  }

  public void Spawn(Behavior b, float speedMult, Vector3 pos) {
    behavior = b;
    speed = speedMult * Reference.behaviors[b].speed;
    transform.position = pos;
    gameObject.SetActive(true);
    setModel();
  }

  public void Deactivate() {
    gameObject.SetActive(false);
  }

  //setters and getter
  public float Speed {
    get { return speed; }
    set { speed = value; }
  }
  public Behavior Behavior {
    get { return behavior; }
    set { behavior = value; }
  }
}
