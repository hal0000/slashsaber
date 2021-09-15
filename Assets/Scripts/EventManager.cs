using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Hit(Vector3 pos);
    public static event Hit OnHit;
    public static void SaberHit(Vector3 pos) => OnHit?.Invoke(pos);

    public delegate void Slash();
    public static event Slash OnSlash;
    public static void SlashButton() => OnSlash?.Invoke();
}