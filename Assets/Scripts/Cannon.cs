using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//предполагается значительное расширение функциональных параметров пушки:
//1.Движение зависит от веса, трения и прилагаемой силы +
//2.Стрельба позволяет менять количество единовременных залпов, периодичность выстрелов, точность выстрелов
//3.Пули отличаются уроном, размером, скоростью

public class Cannon : MonoBehaviour
{
    [SerializeField] Rigidbody2D cannonRigidbody2D;
    [SerializeField] ParticleSystem particleSystem;
    Camera mainCamera;
    [SerializeField] GameConfig gameConfig;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                StartShooting();
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                StopShooting();
            }

            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out float enter);
            Vector3 touchInWorld = ray.GetPoint(enter);
            if (cannonRigidbody2D.position.x < touchInWorld.x - gameConfig.moveBuffer)
            {
                Move(1);
            }
            else if (cannonRigidbody2D.position.x > touchInWorld.x + gameConfig.moveBuffer)
            {
                Move(-1);
            }
        }
    }
    void Move(int direction)
    {
        cannonRigidbody2D.AddForce(direction * gameConfig.forceToMoveCannon, ForceMode2D.Force);
    }
    public void StartShooting()
    {
        particleSystem.Play();
    }
    public void StopShooting()
    {
        particleSystem.Stop();
    }
}
