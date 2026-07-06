using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("AudioSource is null.");

        _audioSource.Play();
        Destroy(this.gameObject, 3.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
