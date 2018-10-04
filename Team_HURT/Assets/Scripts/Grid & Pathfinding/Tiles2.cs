using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles2 : MonoBehaviour
{
    public Transform SpawnGrid;
    public bool playerTurn = true;

    public void Start()
    {
        //intitalize the level by spawning grid, setting up array and setting player turn to true
        //first the x axis
        for (int i = 0; i < 51; i++)
        {// z axis to instantiate

            for (int j = 0; j < 51; j++)
            {
                float xpos = transform.position.x - i;
                float zpos = transform.position.z + j;
                Object instanceObj = Instantiate(SpawnGrid, new Vector3(xpos, 0, zpos), Quaternion.identity);

            }
        }
        /*
        //for the t junction at the end of the hallway
        for (int i = 25; i < 51; i++)
        {// z axis to instantiate

            for (int j = 0; j < 51; j++)
            {
                float xpos = transform.position.x - i;
                float zpos = transform.position.z + j;
                Object instanceObj = Instantiate(SpawnGrid, new Vector3(xpos, 0, zpos), Quaternion.identity);


            }
        }
        */

    }

}
