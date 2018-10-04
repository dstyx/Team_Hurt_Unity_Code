using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



namespace controls
{
    public class PlayerMouseControls : MonoBehaviour
    {


        public GameObject selectedUnit = null;
        public GameObject selectedTile = null;
        public GameObject selectedEnemy = null;
        public GameObject pointer;
        public GameObject pointer2;
        public GameObject[] units;
        public GameObject[] enemies;
        public float distance;
        public string nameE;
        public int APTotal= 0;
        public bool turn;
        public Text unitStats;
        public Text enemyStats;
        public Text turnText;
        public GameObject movement;
        public GameObject attack;
        public int damage = 1;
        private int maxAP;
        public Button yourButton;

        private UnitStatesRifle states;

        private void Start()
        {
            turn = true;
            units = GameObject.FindGameObjectsWithTag("Player");
            enemies = GameObject.FindGameObjectsWithTag("Enemy");


            foreach(GameObject unit in units)// this sets the initial AP values
            {
                APTotal += unit.GetComponent<UnitStatesRifle>().ReturnAP();
                maxAP += unit.GetComponent<UnitStatesRifle>().ReturnAP();
                this.GetComponent<LevelControl>().playercount += 1;
            }

            foreach(GameObject enemy in enemies)//creates the total amount of enemies on the field at the start
            {
                this.GetComponent<LevelControl>().enemycount += 1;
            }

            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            btn.GetComponentInChildren<Text>().text = "End Turn";


        }

        void TaskOnClick()
        {
            Debug.Log("You have clicked the button!");
            APTotal = 0;
        }

        /*
        void OnCollisionEnter(Collision col)// when player and pointer collide it will send the pointer off map to prevent 
        {
            if (col.gameObject.CompareTag ("player"))
            {
                pointer.transform.position = new Vector3(30, 0, 30);
            }
        }
        */


        // Update is called once per frame
        void Update()
        {
            if (this.GetComponent<LevelControl>().enemycount <= 0)
            {
                this.GetComponent<LevelControl>().NextLevel();
            }

            if (this.GetComponent<LevelControl>().playercount <= 0)
            {
                this.GetComponent<LevelControl>().ReloadLevel();
            }

            if (APTotal <= 0)
            {
                turn = false;

            }
            else
            {
                turn = true;
            }



            if (turn == true)
            {
                
                turnText.color = Color.red;
                turnText.text = "Player Turn";
                if (Input.GetMouseButtonDown(0))
                {

                    

                    RaycastHit selector = new RaycastHit();

                    bool selected = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out selector);

                    if (selected)
                    {
                        foreach (GameObject enemy in enemies)
                        {

                            enemy.GetComponent<EnemyStates>().ResetAP();
                            //enemy.GetComponent<EnemyStates>().Turn();
                        }
                        if (selector.transform.gameObject.tag == "Player")
                        {
                            Debug.Log("Player Clicked");
                            selectedUnit = selector.transform.gameObject;
                            unitStats.color = Color.red;
                            unitStats.text = "Player Stats";
                            unitStats.text += "\n" + "Health: " + selectedUnit.GetComponent<UnitStatesRifle>().Health();
                            unitStats.text += "\n" + "AP: " + selectedUnit.GetComponent<UnitStatesRifle>().AP();
                            movement.transform.position = new Vector3(selectedUnit.transform.position.x, 0, selectedUnit.transform.position.z);
                            attack.transform.position = new Vector3(selectedUnit.transform.position.x, 0.1f, selectedUnit.transform.position.z);

                            if(selectedUnit.GetComponent<Unit>() == null)
                            {
                                selectedUnit.AddComponent<Unit>();
                                
                            }
                            
                        }

                        if (selector.transform.gameObject.tag == "Tile")
                        {

                            Debug.Log("Tile Clicked");

                            selectedTile = selector.transform.gameObject;

                            if (selectedUnit != null && Vector3.Distance(selectedUnit.transform.position, selectedTile.transform.position) < 11 && selectedUnit.GetComponent<UnitStatesRifle>().AP() > 0)
                            {
                                /*
                                GameObject temp = new GameObject();
                                temp.transform.position = new Vector3(selectedTile.transform.position.x, 0, selectedTile.transform.position.z);
                                selectedUnit.GetComponent<Unit>().target = temp.transform;
                                selectedUnit.GetComponent<Unit>().beacon = temp;
                                */
                                selectedUnit.GetComponent<Unit>().enabled = true;
                                
                                pointer.transform.position = new Vector3(selectedTile.transform.position.x, 0, selectedTile.transform.position.z);

                                selectedUnit.GetComponent<Unit>().Path();

                                selectedUnit.GetComponent<UnitStatesRifle>().Running();
                                selectedUnit.GetComponent<UnitStatesRifle>().LoseAP();
                                movement.transform.position = new Vector3(10000, 10000, 10000);
                                attack.transform.position = new Vector3(10000, 10000, 10000);
                                selectedUnit = null;
                                unitStats.text = "";
                                
                                selectedTile = null;
                                //Destroy(temp);
                                
                                APTotal = APTotal - 1;
                                



                            }
                            selectedTile = null;
                            Destroy(selectedUnit.GetComponent<Unit>());
                        }

                        //begin shooting enemy

                        if (selector.transform.gameObject.tag == "Enemy")
                        {

                            Debug.Log("Enemy Spotted");
                            if (selectedUnit != null && selectedUnit.GetComponent<UnitStatesRifle>().AP() > 0)
                            {
                                selectedEnemy = selector.transform.gameObject;
                                enemyStats.color = Color.red;
                                enemyStats.text = "Health: " + selectedEnemy.GetComponent<EnemyStates>().Health();
                                enemyStats.text += "\n" + "AP: " + selectedEnemy.GetComponent<EnemyStates>().AP();

                                distance = Vector3.Distance(selectedEnemy.transform.position, selectedUnit.transform.position);
                                if (distance >= 10)
                                {
                                    Debug.Log("Enemy Out of range.");
                                }
                                else
                                {
                                    AudioSource audio = GetComponent<AudioSource>();
                                    audio.Play();
                                    audio.Play();
                                    Debug.Log("Enemy targeted.");
                                    int roll = Random.Range(1, 100);
                                    selectedUnit.GetComponent<UnitStatesRifle>().Attack();
                                    selectedUnit.transform.LookAt(selectedEnemy.transform);
                                    selectedUnit.GetComponent<UnitStatesRifle>().LoseAP();
                                    APTotal = APTotal - 1;
                                    movement.transform.position = new Vector3(10000, 10000, 10000);
                                    attack.transform.position = new Vector3(10000, 10000, 10000);

                                    if (roll < 25)
                                    {
                                        Debug.Log("Shot Missed.");
                                    }
                                    else
                                    {
                                        Debug.Log("Shot hit.");




                                        int rolldamage = Random.Range(2, 5);


                                        selectedEnemy.GetComponent<EnemyStates>().LoseHP(rolldamage);
                                        enemyStats.text = "Health: " + selectedEnemy.GetComponent<EnemyStates>().Health();
                                        /*
                                        nameE = selectedEnemy.transform.name;

                                        selectedEnemy.GetComponent<EnemyStates>().hp;
                                        //selectedEnemy.Getcompnent<EnemyStates>().hp -= rolldamage;
                                        //ap--;
                                        */
                                    }
                                    
                                    //selectedUnit.GetComponent<UnitStatesRifle>().Idle();
                                }
                            }
                           // selectedUnit.GetComponent<UnitStatesRifle>().Attack();
                            if (gameObject.GetComponent<PlayerMouseControls>().selectedEnemy != null)
                            {

                            }
                        }

                        else
                        {
                            Debug.Log("Swing and a miss");

                        }
                        //Destroy(selectedUnit.GetComponent<Unit>());
                    }
                    else
                    {
                        Debug.Log("you didn't click an object dummy");
                    }

                }


            }
            //end of all player interactions


            //begin enemy turn here
            else if(APTotal == 0)//if(no player is moving)
            {
                unitStats.color = Color.red;
                turnText.text = "Enemy Turn";
                unitStats.text = "";
                enemyStats.text = "";

   

                StartCoroutine(EnemySelect());

                
                
            }
        }

        private IEnumerator EnemySelect()
        {
            yield return new WaitForSecondsRealtime(4f);
            
            foreach (GameObject player in units)
            {
                player.GetComponent<Unit>().enabled = false;
                //Destroy(player.GetComponent<Unit>());
            }

            

            //StartCoroutine(EnemyMovement(enemies[0]));
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyStates>().Turn(units);
                //StartCoroutine(wait());
            }
            //enemies[0].GetComponent<EnemyStates>().Turn(units);
            //StartCoroutine(wait());
            //enemies[1].GetComponent<EnemyStates>().Turn(units);
            //StartCoroutine(EnemyMovement(enemies[1]));

            foreach (GameObject enemy in enemies)
            // this'll go through each enemy and do their action, only one action per enemy
            {

                    //StartCoroutine(EnemyMovement(enemy));

                




            }

            //StartCoroutine(EnemyMovement(enemies[0]));
            //StartCoroutine(wait());
            //StartCoroutine(EnemyMovement(enemies[1]));
            StartCoroutine(TurnChange());

            
        }


        private IEnumerator wait()
        {
            yield return new WaitForSecondsRealtime(4f);
        }

        private IEnumerator EnemyMovement(GameObject enemy)
        {
            yield return new WaitForSecondsRealtime(2f);
            //enemy.transform.LookAt(selectedUnit.transform);
            GameObject closest = null;
            float d = Mathf.Infinity;

            foreach (GameObject player in units)//what each enemy will do
            {

                if (enemy.GetComponent<EnemyStates>().AP() > 0)
                {
                    Vector3 diff = player.transform.position - enemy.transform.position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < d)
                    {
                        closest = player;
                        d = curDistance;
                    }


                    if (d <= 10  && enemy.GetComponent<EnemyStates>().AP() > 0)//shoot the player
                    {
                        AudioSource audio = GetComponent<AudioSource>();
                        audio.Play();
                        audio.Play();
                        enemy.transform.LookAt(closest.transform);
                        enemy.GetComponent<EnemyStates>().LoseAP();
                        closest.GetComponent<UnitStatesRifle>().LoseHP(damage);
                        

                    }
                    else if(d>10 && enemy.GetComponent<EnemyStates>().AP() > 0)
                    {
                        enemy.GetComponent<Unit>().enabled = true;
                        pointer2.transform.position = new Vector3(0.5f, 0, 0);//enemy.transform.position;
                        pointer2.transform.position = 0.5F * (enemy.transform.position + closest.transform.position);
                        //if()
                        enemy.GetComponent<Unit>().Path();
                    }
                    else
                    {
                        
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

                        enemy.GetComponent<EnemyStates>().LoseAP();
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

        private IEnumerator TurnChange()
        {
            yield return new WaitForSecondsRealtime(4f);
            foreach (GameObject enemy in enemies)
            {
                //Destroy(enemy.GetComponent<Unit>());
                enemy.GetComponent<Unit>().enabled = false;
            }
            foreach (GameObject player in units)
            {
                player.GetComponent<UnitStatesRifle>().ResetAP();
                APTotal = maxAP;
            }
        }
    }
    /* psuedo code for shooting at enemies
*if (selectedunit ap>=1)
* if keypress 1
* enable select enemy,
* on keypress(left click) enemy, make selected enemy that one
* if distance (whatever the fuck you calculate that in unity)>= range (10) 
* put on UI, enemy out of range
* if distance < 10
* roll dice (0 to 100) base accuracy 65%
* if roll dice >=35 then roll damage between 2-5
* if roll dice <=35 then miss shot, move gun model 15 degrees in random direction(this is advanced shit)
* ap - 1
* end shoot 
* */
}