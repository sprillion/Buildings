using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBuilding : MonoBehaviour
{
    public int id;
    public int level;
    public bool interactive;
    [HideInInspector] public int maxLevel = 5;
}
