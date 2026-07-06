using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the lines of codes commented out are inserting a sprint to our Player
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    /*
    [SerializeField]
    private float _higherSpeed;
    [SerializeField]
    private float _lowerSpeed;
    */
    [SerializeField]
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private SpawnManager _spawnManager;

    // variable for isTripleShotActive
    [SerializeField]
    private bool _isTripleShotActive = false;
    // variable for Triple Shot Prefab
    [SerializeField]
    private GameObject _tripleShotPrefab;

    /* [SerializeField]
    private bool _isSpeedBoostActive = false; */

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _shieldVisualizer = this.transform.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            Debug.LogError("AudioSource on the player is null.");
        else
            _audioSource.clip = _laserSoundClip;

        /*
        _lowerSpeed = _speed;
        _higherSpeed = _speed * 1.5f;
        */
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        // Find the game object
        // get the component

        /*
          if(_spawnManager == null)
                Debug.LogError("The Spawn Manager is NULL !");
        */

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
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

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        /*
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _higherSpeed;
        }
        else
        {
            _speed = _lowerSpeed;
        }
        */

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.98f, 6.07f), 0);

        if (transform.position.x >= 11.41f)
            transform.position = new Vector3(-11.56f, transform.position.y, 0);
        else if (transform.position.x <= -11.56f)
            transform.position = new Vector3(11.41f, transform.position.y, 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if(_isTripleShotActive)
        {
            // instantiate 3 lasers (Triple Shot)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // instantiate 1 laser (One Shot)
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        //if shield is active
        // deactivate shield
        //do nothing, i.e. , return;
        if(_isShieldActive)
        {
            _shieldVisualizer.SetActive(false);
            _isShieldActive = false;
            return;
        }

        // else
        _lives--;

        if(_lives == 2) 
        {
            _leftEngine.SetActive(true);
        }
        
        else if(_lives == 1) 
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            // Communicate with Spawn Manager
            // Let them know to stop spawning
            if(_spawnManager != null) 
                _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        // start the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //   wait 5 second
    //   set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        //_isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        //_isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        // turn visualizer on
        _shieldVisualizer.SetActive(true);
        _isShieldActive = true;
    }

    //method to add 10 to the score!
    //Communicate with the UI to update the score!
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
