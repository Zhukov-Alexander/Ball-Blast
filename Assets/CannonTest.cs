using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTest : MonoBehaviour
{
    Cannon cannon;
    void Start()
    {
        cannon = FindObjectOfType<Cannon>();
        cannon.CanMove = true;
        cannon.UpdateProperties();
        cannon.StartShooting();
    }
}
