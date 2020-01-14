using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScoreUI(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLivesUI(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }

    public void DrawGameOverTextUI()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverTextFlickerRoutine());
    }

    IEnumerator GameOverTextFlickerRoutine()
    {
        while(_gameOverText.gameObject.activeSelf == true)
        {
            yield return new WaitForSeconds(0.3f);
            _gameOverText.enabled = !_gameOverText.enabled;
        }
    }
}
