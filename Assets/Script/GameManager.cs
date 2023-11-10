using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _playerHp = 10;
    [SerializeField] Slider _slider;
    [SerializeField] UnityEvent _onGameOver;
    [SerializeField] Image _image;
    [SerializeField] GameObject _gameOverPanel;
    public int _grapLong;
    // Start is called before the first frame update
    void Start()
    {
        _slider.maxValue = _playerHp;
        _slider.value = _playerHp;
        _image.color = Color.clear;
        _gameOverPanel.SetActive(false);
        _grapLong = 50;
    }

    public void Hit(int damage)
    {
        _playerHp -= damage;
        _slider.DOValue(_playerHp, 1f);
        _image.color = new Color(0.7f, 0f, 0f, 0.7f);
        _image.DOFade(endValue: 0f, duration: 1f);
        if(_playerHp <= 0)
        {
            _onGameOver.Invoke();
        }
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GrapUp(int up)
    {
        _grapLong += up;
    }
}
