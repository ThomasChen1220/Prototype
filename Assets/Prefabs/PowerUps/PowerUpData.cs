using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData : ScriptableObject
{
    public PowerUps mType;
    //public AudioClip mSound;
    public GameObject mShipEffect;
}
