using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region EnumCollection
public enum HoverType{None, PickCard, Point, PutDownCard}
public enum CardType{Undefine, Star, Justice, Hermit, Tower, Magician}
public enum HandState{Default, PickCard, PickDice, FlipCard, Uncontrolled}
public enum LookableType{Heart, Feather}
public enum SphereState{Idle, Follow, DetectingWords}
#endregion
