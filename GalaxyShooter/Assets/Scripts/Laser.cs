using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable of 8
    [SerializeField]
    private float _speed = 8.0f; // in case we change it to float, we define it as float

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 7f)
        {
            //
            // Check if this object has a parent
            // if so, destroy it too
            //
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
