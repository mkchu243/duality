using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
  private Player player;
  private static Dictionary<string, Texture2D> textures;
  private float alertX;

  public GUIStyle centerTextStyle;
  private string centerText;
 
  void Awake(){
    InitStaticVars();
    alertX = 20;
  }
  
	// Use this for initialization
	void Start () {
    player = gameObject.GetComponent<Player>();
    centerText = "";
  }
	
  // Update is called once per frame
  void Update () {
  }

  void OnGUI() {
    if (GUI.Button(new Rect(Screen.width - 50, 10, textures["pause"].width, textures["pause"].height), textures["pause"])) {
      switch (GameManager.state) {
        case GameManager.GameState.paused:
          GameManager.state = GameManager.GameState.running;
          break;
        case GameManager.GameState.running:
          GameManager.state = GameManager.GameState.paused;
          break;
      }
    }

    switch (GameManager.state) {
      case GameManager.GameState.paused:
        centerText = "PAUSED";
        break;
      case GameManager.GameState.gameOver:
        centerText = "GAME OVER";
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 20, 100, 20), "New Game")) { //TODO change the style
          GameManager.state = GameManager.GameState.restart;
        }
        
        break;
      default:
        centerText = "";
        break;
    }
  
    Alerts();
    GUI.Label(new Rect(10, 10, 100, 20), "Score: " + player.Score); //TODO hard coded numbers
    GUI.Label(new Rect(10, 25, 100, 35), "Lives: " + player.Life);
    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), centerText, centerTextStyle);
  }
  
  public void Alerts(){
    Behavior[] alerts = EnemyManager.Instance.getAlerts(alertX);
    for( int i = 0; i < alerts.Length; i++){
      if(alerts[i] == Behavior.nonExistant)
        continue;
      
      Texture2D t = textures["alertParticle"];
      if( alerts[i] == Behavior.wave )
        t = textures["alertWave"];
      Vector3 pos = Camera.main.WorldToScreenPoint( new Vector3 (25, GameManager.laneCenters[i], 0)); //TODO hardcoded 
      //need to do screenheight-a.y because y needs to be inverted for gui
      GUI.Label (new Rect(pos.x - (t.width/2), Screen.height - (pos.y + (t.height/2)), t.width, t.height), t);
    }
  }

  public static void InitStaticVars() {
    textures = new Dictionary<string, Texture2D>();
    Texture2D texture = Resources.Load("Textures/pause") as Texture2D;
    textures.Add("pause", texture);
    texture = Resources.Load("Textures/alertParticle") as Texture2D;
    textures.Add("alertParticle", texture);
    texture = Resources.Load("Textures/alertWave") as Texture2D;
    textures.Add("alertWave", texture);
  }
}
