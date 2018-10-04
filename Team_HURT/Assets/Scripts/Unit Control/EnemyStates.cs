using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStates : MonoBehaviour
{

    public int hp = 5;
    public int speed = 6;
    public int ap = 2;
    private bool moving;
    public GameObject pointer;
    public Animator anim;

    private Transform _myTransform;
    private Vector3 _lastPosition;


    private void Awake()
    {
        _myTransform = transform;
        _lastPosition = _myTransform.position;
    }

    void Update()
    {
        if (_lastPosition == _myTransform.position)
        {
            Debug.Log("Did not move");
            anim.SetBool("Run", false);
            moving = false;

        }
        else
        {
            Debug.Log("Moved");
            anim.SetBool("Run", true);
            moving = true;
        }
        _lastPosition = _myTransform.position;
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

    public bool CheckMove()
    {
        return moving;
    }

        void Start()
    {

    }

    public int Health()
    {
        return hp;
    }

    public void ResetAP()
    {
        ap = 1;
    }

    public void LoseHP(int lost)
    {
        hp -= lost;
        if (hp <= 0)
        {

            hp = 0;
            this.gameObject.SetActive(false);
            GameObject.Find("Controller Object").GetComponent<LevelControl>().enemycount--;
        }
    }


    public int AP()
    {
        return ap;
    }


    public void LoseAP()
    {
        ap --;
    }

    enum EnemyState
    {
        Idle,
        Shooting,
        Cover,
        Damaged
    }

    public void Move()
    {
        this.GetComponent<Unit>().Move();
    }

    public void DisablePath()// Unit seemed to be FUBAR if more than one unit was active at a time or if two or more units were requesting pathing.
    {

        this.GetComponent<Unit>().enabled = false;
    }

    public void EnablePath()
    {
        this.GetComponent<Unit>().enabled = true;
    }

    public void Turn(GameObject[] units)
    {
        StartCoroutine(EnemyMovement(this.gameObject, units));
    }

    private IEnumerator EnemyMovement(GameObject enemy, GameObject[] units)
    {
        yield return new WaitForSecondsRealtime(2f);
        //enemy.transform.LookAt(selectedUnit.transform);
        GameObject closest = null;
        float d = Mathf.Infinity;

        foreach (GameObject player in units)//what each enemy will do
        {

            
                Vector3 diff = player.transform.position - enemy.transform.position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < d)
                {
                    closest = player;
                    d = curDistance;
                }

        }

        if (enemy.GetComponent<EnemyStates>().AP() > 0)
        {
            if (d <= 10 && enemy.GetComponent<EnemyStates>().AP() > 0)//shoot the player
            {
                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();
                    audio.Play();
                    enemy.transform.LookAt(closest.transform);
                    Attack();
                    enemy.GetComponent<EnemyStates>().LoseAP();
                    closest.GetComponent<UnitStatesRifle>().LoseHP(1);


            }
            else if (d > 10 && enemy.GetComponent<EnemyStates>().AP() > 0)
            {
                enemy.GetComponent<Unit>().enabled = true;
                pointer.transform.position = new Vector3(0.5f, 0, 0);//enemy.transform.position;
                pointer.transform.position = 0.5F * (enemy.transform.position + closest.transform.position);
                //if()
                enemy.GetComponent<Unit>().Path();
                Running();
                enemy.GetComponent<EnemyStates>().LoseAP();
            }
            else
            {
                Idle();
                /*
                if (enemy.GetComponent<Unit>() == null)
                {
                    GameObject temp = new GameObject();
                    temp.transform.position = new Vector3(0.5f, 0, 0);//enemy.transform.position;
                    enemy.AddComponent<Unit>();
                    enemy.GetComponent<Unit>().target = temp.transform;
                    enemy.GetComponent<Unit>().beacon = temp;
                    enemy.GetComponent<Unit>().anim = enemy.GetComponent<Animator>();
                    temp.transform.position = 0.5F * (enemy.transform.position + closest.transform.position);
                }
                */

                //enemy.GetComponent<EnemyStates>().EnablePath();



                //enemy.GetComponent<EnemyStates>().Move();

                //enemy.GetComponent<EnemyStates>().LoseAP();
                /*
                if(enemy.GetComponent<EnemyStates>().CheckMove() == false)
                {
                    Destroy(temp);
                }*/
            }

        }

        //enemy.GetComponent<EnemyStates>().DisablePath();

    }

}