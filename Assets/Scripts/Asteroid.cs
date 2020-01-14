using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _speed = 15.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on asteroid is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1.0f) * _speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            _spawnManager.StartSpawning();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(this.gameObject, 1f);
        }
    }

}
