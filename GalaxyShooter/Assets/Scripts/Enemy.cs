using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Animator _anim;

    private Player _player;

    private bool _isCollisionOccurred = false;

    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
            Debug.LogError("AudioSource is null.");

        _anim = this.gameObject.GetComponent<Animator>();

        if(_anim == null) 
        {
            Debug.LogError("Animator is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.48f)
        {
            float randomX = Random.Range(-9.25f, 9.25f);
            transform.position = new Vector3(randomX, 9, 0);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_isCollisionOccurred == false)
        {
            //Debug.Log("Hit: " + other.transform.name);

            /* if other is player
                * Destroy us (enemy)
                * Damage the player */

            if (other.tag == "Player")
            {
                Player player = other.gameObject.GetComponent<Player>();
                if (player != null) // Checking whether Player component/script exists to dodge the errors if the script is unattached somehow
                {
                    player.Damage();
                }
                // trigger anim
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _isCollisionOccurred = true;
                Destroy(this.gameObject, 2.8f);
            }

            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.AddScore(10);
                }
                // we could have assigned _player here through .Find but it is an expensive operator.
                // Doing it everytime we hit an enemy just doesn't make sense
                // so we define _player globally

                // trigger anim
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _isCollisionOccurred = true;
                Destroy(this.gameObject, 2.8f);
            }

            /* if other is laser
                * destroy laser
                * destroy us (enemy) 
         
            If we destroy us (enemy) and then the laser, we can't destroy the laser since the enemy destroyed first, meaning the script
            ,which is inside enemy, is inactive
                */
        }
    }
}
