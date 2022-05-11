using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainScripts : MonoBehaviour
{
    [SerializeField] GameObject _zombie;
    [SerializeField] GameObject _deathPanel;
    [SerializeField] GameObject _player;
    [SerializeField] Transform _zombieParent;
    [SerializeField] float _zombiInstantianteTime;
    [SerializeField] Text _scoreText;
    public int _scoreInt;
    int _score;
    float _currentTime;
   
    // Update is called once per frame
    void Update()
    {
        _currentTime +=Time.deltaTime;
        if(_currentTime >= _zombiInstantianteTime)
        {
            Vector3 pos = new Vector3(Random.Range(180,350), 30, Random.Range(220,320));
            Instantiate(_zombie, pos, Quaternion.identity,_zombieParent);
            _currentTime = 0;
        }
    }
    public void PuanAttir(int p)
    {
        _score += p;
        _scoreText.text = " " +  _score;
    }
    public void ReplayButton()
    {
        Cursor.visible = false;
            Time.timeScale = 1;
            _score = 0;
            for (int i = 0; i < _zombieParent.childCount; i++)
            {
                Destroy(_zombieParent.GetChild(i).gameObject);
            }
            _player.GetComponent<PlayerController>()._healFloat = 100;
            _player.GetComponent<PlayerController>()._startBullet = 30;
            _player.GetComponent<PlayerController>()._magazinecapacity = 30;
            _player.GetComponent<PlayerController>()._totalAmmo = 50;
            _deathPanel.SetActive(false);
        
    }
}
