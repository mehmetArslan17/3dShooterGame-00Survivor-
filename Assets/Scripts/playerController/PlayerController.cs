using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform _bulletPos, _bulletposEnd;
    [SerializeField] Text _magazineText;
    [SerializeField] GameObject _bullet, _Gunparticule, _impacktEffeckt;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] GameObject _deathPanel;
    [SerializeField] float _gunRange;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletTime;
    [Range(30, 200)][SerializeField] public float _startBullet;   //baslangic mermi
    [Range(30, 50)][SerializeField] public float _magazinecapacity; //þarjörün kapasitesi
    [Range(30, 210)][SerializeField] public float _totalAmmo;  // silahýn maximum alinabilen mermiSayisi
    float _addBullet;  //eklenen mermi
    float _reloadTimer;
    Animator _gunAnim;
    [SerializeField] Camera _fpsCam;
    public Image _heltImage;
    public float _healFloat = 100f;
    float _currentTime;
    [SerializeField] private AudioClip _fireclip, _endFireClip, _reload, _reloadfail;
    private AudioSource _audioSource;
    bool _isHaveBullet = true;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _gunAnim = GetComponent<Animator>();
    }
    private void Update()
    {
 
        ReloadMagazine();
    }
    void ReloadMagazine()
    {
        _magazineText.text = "" + _startBullet + " / " + _totalAmmo;
        _currentTime += Time.deltaTime;
        _addBullet = _magazinecapacity - _startBullet;
        if (_addBullet > _totalAmmo)
        {
            _addBullet = _totalAmmo;
        }
        if (_startBullet <= 0)
        {
            _isHaveBullet = false;
        }
        else
        {
            _isHaveBullet = true;

        }
        if (Input.GetKeyDown(KeyCode.R) && _addBullet > 0 && _totalAmmo > 0)
        {
            _gunAnim.SetBool("__reload", true);
            _audioSource.clip = _reload;
            _audioSource.Play();
            if (Time.time > _reloadTimer)
            {
                StartCoroutine(Reload());
                _reloadTimer = Time.time;
            }
        }
        if ((Input.GetKeyDown(KeyCode.R) && _addBullet <= 0) || (Input.GetKeyDown(KeyCode.R) && _totalAmmo <= 0))
        {
            _audioSource.clip = _reloadfail;
            _audioSource.Play();
        }
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.2f);
        _startBullet = _startBullet + _addBullet;
        _totalAmmo = _totalAmmo - _addBullet;
        _gunAnim.SetBool("__reload", false);
    }
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && _bulletTime < _currentTime && _isHaveBullet)
        {
            Shoot();

            _currentTime = 0;
        }

    }
    void FireSound()
    {
        if (Input.GetButton("Fire1") && _bulletTime < _currentTime)
        {
            _audioSource.clip = _fireclip;
            _audioSource.Play();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _audioSource.clip = _endFireClip;
            _audioSource.Play();
        }
    }
    void Shoot()
    {
        _startBullet--;
        FireSound();
        _particleSystem.Play();
        RaycastHit hit;
        if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _gunRange))
        {
            ZombiMove _zombimove = hit.transform.GetComponent<ZombiMove>();
            if (_zombimove != null)
            {
                _zombimove.TakeDamage(-1f);
            }
            GameObject impacktGo = Instantiate(_impacktEffeckt, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impacktGo, 0.5f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.gameObject.tag)
        {
            case "zombi":
                _healFloat -= 10f;
                float x = _healFloat / 100f;
                _heltImage.fillAmount = x;
                //can azaldýkca yeþilden kýrmiziya dogru renk gecisi    
                _heltImage.color = Color.Lerp(Color.red, Color.green, x);
                if (_healFloat <= 0)
                {
                    Time.timeScale = 0;
                    _deathPanel.SetActive(true);
                    Cursor.visible = true;
                }
                break;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Healt":
                if (_healFloat < 100f)
                {
                    _healFloat += 10f;
                    float x = _healFloat / 100f;
                    _heltImage.fillAmount = x;
                    //can azaldýkca yeþilden kýrmiziya dogru renk gecisi    
                    _heltImage.color = Color.Lerp(Color.red, Color.green, x);
                }
                _totalAmmo = _totalAmmo + 15f;
                Destroy(other.gameObject);
                break;
        }
    }
}
