using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameConfigContainer;

public class BallSpawner : MonoBehaviour
{

    [SerializeField] int sign;
    public void Shoot(float life, int type, int number)
    {
        GameObject ballGO = CreateABall();
        Rigidbody2D ballRigidbody2D = ballGO.GetComponent<Rigidbody2D>();
        ballRigidbody2D.AddForce(GetShotDirection() * GetForceToShoot(), ForceMode2D.Impulse);
        List<int> rand = HelperClass.GetRandomObjectsFromList(new List<int>() { -1, 1 });
        ballRigidbody2D.AddTorque(rand[0] * Random.Range(gameConfig.torqueAmountMinMax.x, gameConfig.torqueAmountMinMax.y), ForceMode2D.Impulse);

        float GetForceToShoot()
        {
            return Random.Range(gameConfig.ballSpawnerForceToShoot.x, gameConfig.ballSpawnerForceToShoot.y);
        }
        Vector2 GetShotDirection()
        {
            int randomAngle = sign * Random.Range(gameConfig.ballSpawnerShootAngles.x, gameConfig.ballSpawnerShootAngles.y);
            Vector3 rotatedVector = HelperClass.VectorFromAngle(randomAngle);
            return rotatedVector;
        }
        GameObject CreateABall()
        {
            GameObject ballGameObject = Instantiate(gameConfig.ballPrefabs[type], gameObject.transform.position, Quaternion.identity, transform);
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Constructor(life, type, number);

            return ballGameObject;
        }
    }
}
