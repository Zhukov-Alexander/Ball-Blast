using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BallSpawner))]
public class Tragectory : MonoBehaviour
{
    private PhysicsScene2D scenePhysics;
    [SerializeField] GameConfig gameConfig;
    /*private void OnDrawGizmos()
    {
        Physics2D.autoSimulation = false;
        Handles.DrawPolyLine(GetTrajectoryPoints(ballSpawner.transform.position, 
            new Vector2(ballSpawner.BallSpawnerDiraction * gameConfig.minShotDirection.x, gameConfig.minShotDirection.y)));
        Handles.DrawPolyLine(GetTrajectoryPoints(ballSpawner.transform.position, 
            new Vector2(ballSpawner.BallSpawnerDiraction * gameConfig.maxShotDirection.x, gameConfig.maxShotDirection.y)));
        Physics2D.autoSimulation = true;
    }*/
    public Vector3[] GetTrajectoryPoints(Vector3 posintion, Vector3 force)
    {
        scenePhysics = SceneManager.GetActiveScene().GetPhysicsScene2D();
        GameObject predictionBall = Instantiate(gameConfig.mockBallPrefab, gameObject.transform.position, Quaternion.identity);
        ;
        Rigidbody2D rb2d = predictionBall.GetComponent<Rigidbody2D>();
        rb2d.AddForce(force, ForceMode2D.Impulse);

        Vector3[] points = new Vector3[gameConfig.simulatedIterationesOfTrajectory];
        points[0] = posintion;
        for (int i = 1; i < gameConfig.simulatedIterationesOfTrajectory; i++)
        {
            scenePhysics.Simulate(Time.fixedDeltaTime);
            points[i] = predictionBall.transform.position;
        }
        DestroyImmediate(predictionBall);
        return points;
    }
}