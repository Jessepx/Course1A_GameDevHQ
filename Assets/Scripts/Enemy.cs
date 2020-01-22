﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    private Player _player;
    private Animator _animator;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if(_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (var laser in lasers)
            {
                laser.AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                _audioSource.clip = _explosionSoundClip;
                player.Damage();
                _animator.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }
        else if (other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);
            

            if (_player != null)
            {
                _player.UpdateScore(10);
            }
            _audioSource.clip = _explosionSoundClip;
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    private void CalculateMovement()
    {
        float yLowerBounds = -5.0f;
        float yUpperBounds = 7.0f;
        Vector3 enemyDirection = new Vector3(0, -1, 0);

        transform.Translate(enemyDirection * _speed * Time.deltaTime);

        if (transform.position.y < yLowerBounds)
        {
            Vector3 wrapSpawnPosition = new Vector3(Random.Range(-9.0f, 9.0f), yUpperBounds, 0);
            transform.position = wrapSpawnPosition;
        }
    }
}
