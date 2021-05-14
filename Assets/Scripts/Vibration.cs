using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static GameConfigContainer;
[RequireComponent(typeof(Button))]
public class Vibration : MonoBehaviour
{
    static List<KeyValuePair<GameObject, Tween>> tweens = new List<KeyValuePair<GameObject, Tween>>();
    Tween tween;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Vibrate);
    }
    public static void Vibrate(GameObject gameObject)
    {
        tweens.Where(a => a.Key == gameObject).ToList().ForEach(a => {
            a.Value.Complete();
            tweens.Remove(a);
            });
        Vector3 scale = gameObject.transform.localScale;
        Tween tween = DOTween.To(a => gameObject.transform.localScale = scale * gameConfig.ScaleEnemyAnimationCurve.Evaluate(a), 0, 1, gameConfig.timeToDownScaleEnemy)
            .OnComplete(() => gameObject.transform.localScale = scale);
        tweens.Add(new KeyValuePair<GameObject, Tween>(gameObject, tween));
    }
    void Vibrate()
    {
        tween.Complete();
        Vector3 scale = gameObject.transform.localScale;
        tween = DOTween.To(a => gameObject.transform.localScale = scale * gameConfig.ScaleEnemyAnimationCurve.Evaluate(a), 0, 1, gameConfig.timeToDownScaleEnemy)
            .OnComplete(() => gameObject.transform.localScale = scale);
        tweens.Add(new KeyValuePair<GameObject, Tween>(gameObject, tween));
    }
}
