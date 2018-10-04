using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatesRifle : MonoBehaviour
    {
        public int hp = 10;
        public int speed = 6; //speed 
        public int ap = 2; //action points
    public Animator anim;
    public float timer = 0.0F;
    private Transform _myTransform;
    private Vector3 _lastPosition;


    private void Awake()
    {
        _myTransform = transform;
        _lastPosition = _myTransform.position;
    }

    // Update is called once per frame


    void Update()
    {
        if (_lastPosition == _myTransform.position)
        {
            Debug.Log("Did not move");
            anim.SetBool("Run", false);

        }
        else
        {
            Debug.Log("Moved");
            anim.SetBool("Run", true);
        }
        _lastPosition = _myTransform.position;

        
        if (AP() == 0) // && _lastPosition != _myTransform.position)can't seem to figure out how to make it so it'll only start disableling path when it's not moving && when AP = 0
        {
            timer = timer - Time.deltaTime;
            if (0 == 0 )
            {
                this.DisablePath();
                timer = 0;// improvised way to disable path
            }
        }
        if (AP() > 0)
        {
            this.EnablePath();
            timer = 2;
        }

    }


    public void Idle()
    {
        anim.SetTrigger("Attack");
    }

    public void Attack()
    {
        anim.SetTrigger("Shoot");
    }

    public void Running()
    {
        anim.SetTrigger("Running");
    }


    public int Health()
    {
        return hp;
    }

    public int AP()
    {
        return ap;
    }


    public void LoseAP()
        {
            ap -= 1;
        }

    public void ResetAP()
    {
        ap = 2;
    }

    public int ReturnAP()
    {
        return ap;
    }

    public void LoseHP(int lost)
    {
        hp -= lost;
        if (hp <= 0)
        {
            hp = 0;
            GameObject.Find("Controller Object").GetComponent<LevelControl>().playercount--;
            Destroy(this.gameObject);
        }
    }

    public void DisablePath()// Unit seemed to be FUBAR if more than one unit was active at a time or if two or more units were requesting pathing.
    {
        
        this.GetComponent<Unit>().enabled = false;
    }

    public void EnablePath()
    {
        this.GetComponent<Unit>().enabled = true;
    }

        enum RifleState
        {
            Idle,
            Shooting,
            Cover,
            Damaged
        }
    }
