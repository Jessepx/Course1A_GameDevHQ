using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _playerIsDead = false;
    [SerializeField]
    private GameObject[] _powerUpPrefabs;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        while(_playerIsDead == false)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9.0f, 9.0f), 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        while (_playerIsDead == false)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-9.0f, 9.0f), 7.0f, 0);
            int randomPowerUp = Random.Range(0, 6);
            GameObject newPowerup = Instantiate(_powerUpPrefabs[randomPowerUp], spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public bool OnPlayerDeath()
    {
        _playerIsDead = true;

        return _playerIsDead;
    }
}
