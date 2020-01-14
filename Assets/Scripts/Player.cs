﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private int _speedMultiplier = 2;
    private Vector3 _startPosition = new Vector3(0, 0, 0);
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject[] _engines;

    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldBoostActive = false;

    [SerializeField]
    private int _score;
    private UIManager _uiManager;

    //variable to store audio clip
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _powerupSoundClip;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPosition;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    public void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float yUpperBounds = 0;
        float yLowerBounds = -3.8f;
        float xLeftBounds = -11.3f;
        float xRightBounds = 11.3f;
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);


        if (transform.position.y > yUpperBounds)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < yLowerBounds)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > xRightBounds)
        {
            transform.position = new Vector3(xLeftBounds, transform.position.y, 0);
        }
        else if (transform.position.x < xLeftBounds)
        {
            transform.position = new Vector3(xRightBounds, transform.position.y, 0);
        }
    }

    public void FireLaser()
    {
        Vector3 laserOffset = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);
        _canFire = Time.time + _fireRate;
        _audioSource.clip = _laserSoundClip;
        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Instantiate(_laserPrefab, laserOffset, Quaternion.Euler(0, 0, 0));
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_shieldBoostActive == true)
        {
            _shieldBoostActive = false;
            _shield.SetActive(false);

            return;
        }
        else
        {
            _lives--;

            _uiManager.UpdateLivesUI(_lives);

            if (_lives == 2)
            {
                var randomEngine = Random.Range(0, 2);
                _engines[randomEngine].SetActive(true);
            }
            else if (_lives == 1)
            {
                foreach(var engine in _engines)
                {
                    if(!engine.activeSelf)
                    {
                        engine.SetActive(true);
                    }
                }
            }
            else if (_lives < 1)
            {
                _audioSource.clip = _explosionSoundClip;
                _spawnManager.OnPlayerDeath();
                _uiManager.DrawGameOverTextUI();
                AudioSource.PlayClipAtPoint(_explosionSoundClip, transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    public int GetLives()
    {
        return _lives;
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        _audioSource.clip = _powerupSoundClip;
        _audioSource.Play();
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        _audioSource.clip = _powerupSoundClip;
        _audioSource.Play();
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldBoostActive()
    {
        _shieldBoostActive = true;
        _shield.SetActive(true);
        _audioSource.clip = _powerupSoundClip;
        _audioSource.Play();
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreUI(_score);
    }
}