using UnityEngine;
using System.Collections.Generic;

public enum Behavior { particle, wave };

public struct Attributes{
  public float speed;

  public Attributes(float speed){
    this.speed = speed;
  }
}

static class Reference {
  public const int NumLanes = 5;

  public const float BasePSpeed = -5.0f;
  public const float BaseWSpeed = -5.0f;

  public static Dictionary<Behavior, Attributes> behaviors = 
    new Dictionary <Behavior, Attributes>{
    { Behavior.particle, new Attributes(BasePSpeed) },
    { Behavior.wave,     new Attributes(BaseWSpeed) }
  };

}