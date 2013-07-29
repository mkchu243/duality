using UnityEngine;
using System.Collections.Generic;

public class Plane : MonoBehaviour {

  private Behavior behavior;
  private static Dictionary<Behavior, Texture> textures;

	// Use this for initialization
	void Start () {
    Texture2D texture = Resources.Load("Textures/Metal_Bolt_Texture_by_FantasyStock") as Texture2D;
    Texture2D texture2 = Resources.Load("Textures/fabric_yikes_texture_by_fantasystock-d46l2xh") as Texture2D;
    textures = new Dictionary<Behavior, Texture>();
    textures.Add(Behavior.particle, texture);
    textures.Add(Behavior.wave, texture2);
  }
	
	// Update is called once per frame
	void Update () {
	  
	}

  public Behavior Behavior {
    get { return behavior; }
    set { 
      behavior = value;
      renderer.material.mainTexture = textures[behavior];
    }
  }
}
