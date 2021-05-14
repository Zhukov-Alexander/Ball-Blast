using System;
using System.Collections.Generic;
using UnityEngine;
using static GameConfigContainer;

public abstract class Currency : MonoBehaviour
{
    [SerializeField] protected GameObject floatingText;
    [SerializeField] new Rigidbody2D rigidbody2D;
    public double Weight { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Collect();
        }
    }
    public abstract void Collect();
    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }

}
