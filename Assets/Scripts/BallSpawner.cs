using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameConfig gameConfig;

    [SerializeField] int sign;
    public void Shoot(float life, int size)
    {
        GameObject ballGO = CreateABall();
        Rigidbody2D ballRigidbody2D = ballGO.GetComponent<Rigidbody2D>();
        ballRigidbody2D.AddForce(GetShotDirection() * gameConfig.ballSpawnerForceToShoot, ForceMode2D.Impulse);

        Vector2 GetShotDirection()
        {
            int randomAngle = Random.Range(gameConfig.minBallSpawnerShootAngle, gameConfig.maxBallSpawnerShootAngle);
            Vector3 rotatedVector = Quaternion.AngleAxis(sign * randomAngle, Vector3.back) * Vector3.up;
            return rotatedVector;
        }
        GameObject CreateABall()
        {
            GameObject ballGameObject = Instantiate(gameConfig.ballPrefab, gameObject.transform.position, Quaternion.identity);
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Constructor(life, size);

            return ballGameObject;
        }
    }
}
