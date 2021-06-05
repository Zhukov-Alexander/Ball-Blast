using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static GameConfigContainer;

public class BonusManager : Singleton<BonusManager>
{
    [SerializeField] public List<GameObject> bonusPrefabs;
    public List<List<KeyValuePair<Sequence, GameObject>>> activeBonuses = new List<List<KeyValuePair<Sequence, GameObject>>>();

    private void Awake()
    {
        for (int i = 0; i < bonusPrefabs.Count; i++)
        {
            activeBonuses.Add(new List<KeyValuePair<Sequence, GameObject>>());
        }
        Ball.OnDestroy += (a,b) => TrySpawnBonus(b);
        LevelMenu.AddToEnd(CompleteAll);
        LevelMenu.AddToEnd(() => GameObject.FindGameObjectsWithTag("Bonus").ToList().ForEach(a=>Destroy(a)));
    }
    public void TrySpawnBonus(Vector2 position)
    {
        if(Random.value <= CannonManager.Cannon.BonusProb)
        {
            int index = Random.Range(0, bonusPrefabs.Count);
            GameObject gameObject = Instantiate(bonusPrefabs[index], position + HelperClass.RandomDirecton() * 0.5f, Quaternion.identity);
            gameObject.GetComponent<Rigidbody2D>().AddForce(HelperClass.RandomDirecton(), ForceMode2D.Impulse);
            gameObject.GetComponent<Bonus>().Type = index;
        }
    }
    public void Complete(int type)
    {
        if (activeBonuses[type].Count > 0)
        {
            activeBonuses[type].ForEach(a => {
                a.Key?.Complete(true);
                Destroy(a.Value);
                });
            activeBonuses[type].Clear();
        }
    }
    void CompleteAll()
    {
        for (int i = 0; i < bonusPrefabs.Count; i++)
        {
            Complete(i);
        }
    }
}
