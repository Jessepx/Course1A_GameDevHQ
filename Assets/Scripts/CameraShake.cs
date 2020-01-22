using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    Vector3 maximumTranslationShake;
    Vector3 origPosition;
    [SerializeField]
    float frequency = 25;
    [SerializeField]
    float recoverySpeed = 1;
    private float trauma;
    private float seed;
    private Player _player;
    private GameManager _gameManager;

    private void Start()
    {
        seed = Random.value;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL on Camera");
        }

        if (_gameManager == null)
        {
            Debug.LogError("GM is NULL on Camera");
        }

        origPosition = transform.position;
        maximumTranslationShake = new Vector3(1, 1, 0);
    }

    private void Update()
    {
        if(_player.GetPlayerHit() == true && _gameManager.GetGameOver() == false)
        {
            float shake = 1f;

            transform.localPosition = new Vector3(
                maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1), 
                maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2),
                -10) * shake;

            trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = origPosition;
        }

    }
}
