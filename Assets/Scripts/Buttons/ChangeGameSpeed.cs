using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeGameSpeed : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    List<int> speed = new List<int>() { 1, 3, 10 };
    int currentSpeed = 0;
    public void ChangeSpeed()
    {
        if (currentSpeed < speed.Count - 1)
            currentSpeed++;
        else
            currentSpeed = 0;
        Time.timeScale = speed[currentSpeed];
        textMesh.text = "Speed x" + speed[currentSpeed];
    }
}
