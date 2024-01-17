using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public static class AnimationVar
{
    public static readonly string IsRunning = "isRunning";
    public static readonly string IsDashing = "isDashing";
}
public static class AnimationLayers
{
    public static readonly string Carry = "ARMS CATCHED";
    public static readonly string Grab = "ARMS TAKING";
    public static readonly string Throw = "ARMS THROWING";
    public static readonly string Bonk = "ARMS BONKING";

}
public static class AnimationState
{
    public static readonly string Bonk = "BONK";
    public static readonly string Grab = "TAKING";
    public static readonly string Throw = "THROWING";
    public static readonly string Catched = "CATCHED";

}
