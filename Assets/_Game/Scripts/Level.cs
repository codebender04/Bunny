using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] characterTransformArray;
  
    public Transform[] GetStartingCharacterTransformArray()
    {
        return characterTransformArray;
    }
}
