using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SlingShot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    public float maxLength;

    public float bottomBoundary;

    bool isMouseDown;

    public GameObject birdPrefab;

    public float birdPositionOffset;

    Rigidbody2D bird;
    Collider2D birdCollider;

    GameObject youLost;

    public float force;

    int totalBirds = 3; // Total number of birds available
    int pigsRemaining;

    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        youLost = GameObject.FindWithTag("YouLost");

        pigsRemaining = GameObject.FindGameObjectsWithTag("Pig").Length;

        CreateBird();
    }

    void CreateBird()
    {
        pigsRemaining = GameObject.FindGameObjectsWithTag("Pig").Length;

        if (totalBirds > 0 && pigsRemaining > 0)
        {
            bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
            birdCollider = bird.GetComponent<Collider2D>();
            birdCollider.enabled = false;

            bird.isKinematic = true;

            ResetStrips();
        }
        else
        {

            Invoke(nameof(CheckWinLoseCondition), 4);

        }
    }

    void CheckWinLoseCondition()
    {
        if (pigsRemaining > 0)
        {
            // Player loses
            UnityEngine.Debug.Log("You lose!");

            if (youLost != null)
            {
                youLost.SetActive(true); // Перевірка на null перед викликом методу
            }
            else
            {
                UnityEngine.Debug.LogError("GameObject 'YouLost' is null.");
            }
        }
        else
        {
            // Player wins
            UnityEngine.Debug.Log("You win!");
            Invoke(nameof(GoToNextLevelOrRestart), 2);
        }
    }

    void RestartLevel()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void GoToNextLevelOrRestart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        UnityEngine.Debug.Log(currentScene.buildIndex);

        if (currentScene.buildIndex < SceneManager.sceneCountInBuildSettings) SceneManager.LoadScene(currentScene.buildIndex + 1);
        else SceneManager.LoadScene(0);
    }

    void ReturnToMenu()
    {
        // Assuming your menu scene is at index 0, modify it accordingly
        SceneManager.LoadScene(0);
    }

    void Update()
    {


        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        currentPosition = idlePosition.position;
    }

    int successfulHits = 0; // Variable to count successful hits

    void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;

        bird.GetComponent<Bird>().Release();

        bird = null;
        birdCollider = null;

        successfulHits++; // Increment successful hits

        pigsRemaining = GameObject.FindGameObjectsWithTag("Pig").Length;

        if (successfulHits >= 3) // Check if three successful hits have been made
        {
            Invoke(nameof(CheckWinLoseCondition), 5);
        }
        else
        {
            Invoke(nameof(CreateBird), 5);
        }
    }

    void ReloadScene()
    {
        // Reload the scene
        SceneManager.LoadScene(0);
    }

    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}
