using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiMove : MonoBehaviour
{
    GameObject _player;
    mainScripts _mainScripts;
    [SerializeField] int _zombiHealt=3  ;
    [SerializeField] GameObject _heart;
    //mesafe belirlemek icin distance atadik
    float _distance;
    NavMeshAgent _nav;
    private void Awake()
    {
        _mainScripts = GameObject.Find("main").GetComponent<mainScripts>();
        _player = GameObject.Find("Player");
        _nav=GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //iki mesafe arasý uzaklýk
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        if(_distance < 10f)
        {
            GetComponentInChildren<Animation>().Play("Zombie_Attack_01");    
        }
        else if (_distance > 10f)
        {
            GetComponentInChildren<Animation>().Play("Zombie_Walk_01");
        }
    }
    void FixedUpdate()
    {
        //oyuncuyu takip etmesi icin
        if(_zombiHealt > 0 && _player.GetComponent<PlayerController>()._healFloat > 0  )
        {
           _nav.SetDestination (_player.transform.position);
        }
    }
    public void TakeDamage(float amount)
    {
        _zombiHealt--;
        if (_zombiHealt == 0)
        {
            Instantiate(_heart, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            _mainScripts.PuanAttir(_mainScripts._scoreInt);

            Destroy(gameObject);
        }
    }
        
}
