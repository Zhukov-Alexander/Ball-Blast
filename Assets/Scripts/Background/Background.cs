using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] public BackgroundSettings backgroundSettings;
    public int PrefabNumber { get; set; }
    public void UpdateProperties()
    {
        PrefabNumber = SaveManager.Instance.SavedValues.ScenePrefabNumber;
    }
}
