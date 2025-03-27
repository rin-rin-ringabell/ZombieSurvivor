using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObject/ZombieData")]
public class ZombieSO : ScriptableObject
{
    public float maxHealth;
    public float speed;
    public float attack;
    public Color skinColor;
}
