﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class FireStream : MonoBehaviour
{

    public float speed;
    public float force = 0;
    public float damage = 0.5f;
    private int playerNum;

    void Start()
    {
        //transform.Translate(transform.forward);
    }

    //Set player number of projectile to stop it from colliding with its own player.
    public void setPlayerNo(int x)
    {
        playerNum = x;
    }

    //Set speed projectile is translated.
    public void setSpeed(float _speed)
    {
        speed = _speed;
    }

    //Projectile moves forward per frame update at a rate of speed variable.
    //It is then destroyed after certain time.
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Destroy(this.gameObject, 5);
    }








    //If projectile colides then the other object it forced back with the force specified.
    //Sound is played to indicate this.
    //The object is then destroyed.
    void OnCollisionStay(Collision _col)
    {
        if (_col.gameObject.tag == "Player" && _col.gameObject.GetComponent<PlayerController>().playerNum != playerNum)
        {
            PlayerController player = _col.gameObject.GetComponent<PlayerController>();
            player.Damage(damage, playerNum);
        }
    }
}
