using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    private Transform room;
    private Transform roof;
    private bool[] doorDirections = new bool[4]; //North, East, South, West
    private Transform[] walls = new Transform[4];
    private Vector2 gridPosition;
    private int roomType;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeRoom(int gridXPos, int gridYPos, GameObject pRoom, bool[] pDoorDirections, GameObject pWall, GameObject pDoorWall, GameObject pRoof, int pEntrance = -1)
    {
        gridPosition = new Vector2(gridXPos, gridYPos);
        room = GameObject.Instantiate(pRoom).transform;
        roof = GameObject.Instantiate(pRoof, room).transform;
        room.position = new Vector3((gridPosition.x - LevelGenerator.roomGridSize / 2) * 50, 0, (gridPosition.y - LevelGenerator.roomGridSize / 2) * 50);
        roof.position = new Vector3((gridPosition.x - LevelGenerator.roomGridSize / 2) * 50, 50, (gridPosition.y - LevelGenerator.roomGridSize / 2) * 50);
        doorDirections = pDoorDirections;
        roomType = int.Parse(pRoom.name.Remove(0,5));

        for (int i = 0; i < doorDirections.Length; i++)
        {
            if (doorDirections[i])
            {
                walls[i] = GameObject.Instantiate(pDoorWall, room).transform;
            }
            else
            {
                walls[i] =  GameObject.Instantiate(pWall, room).transform;
            }
            if (i == 0)
            {
                walls[i].position = room.position + new Vector3(0, 25, 24.5f);
            }
            else if (i == 1)
            {
                walls[i].position = room.position + new Vector3(24.5f, 25, 0);
                walls[i].eulerAngles = new Vector3(0, 90, 0);
            }
            else if (i == 2)
            {
                walls[i].position = room.position + new Vector3(0, 25, -24.5f);
            }
            else if (i == 3)
            {
                walls[i].position = room.position + new Vector3(-24.5f, 25, 0);
                walls[i].eulerAngles = new Vector3(0, 90, 0);
            }
        }
        if (pEntrance >= 0)
        {
            doorDirections[pEntrance] = false;
        }
    }

    public void RepositionRoom(int gridXPos, int gridYPos, bool[] pDoorDirections, GameObject pWall, GameObject pDoorWall, int pEntrance = -1)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            Destroy(walls[i].gameObject);
        }
        walls = new Transform[4];

        gridPosition = new Vector2(gridXPos, gridYPos);
        room.position = new Vector3((gridPosition.x - LevelGenerator.roomGridSize / 2) * 50, 0, (gridPosition.y - LevelGenerator.roomGridSize / 2) * 50);
        roof.position = new Vector3((gridPosition.x - LevelGenerator.roomGridSize / 2) * 50, 50, (gridPosition.y - LevelGenerator.roomGridSize / 2) * 50);
        doorDirections = pDoorDirections;

        for (int i = 0; i < doorDirections.Length; i++)
        {
            if (doorDirections[i])
            {
                walls[i] = GameObject.Instantiate(pDoorWall, room).transform;
            }
            else
            {
                walls[i] = GameObject.Instantiate(pWall, room).transform;
            }
            if (i == 0)
            {
                walls[i].position = room.position + new Vector3(0, 25, 24.5f);
            }
            else if (i == 1)
            {
                walls[i].position = room.position + new Vector3(24.5f, 25, 0);
                walls[i].eulerAngles = new Vector3(0, 90, 0);
            }
            else if (i == 2)
            {
                walls[i].position = room.position + new Vector3(0, 25, -24.5f);
            }
            else if (i == 3)
            {
                walls[i].position = room.position + new Vector3(-24.5f, 25, 0);
                walls[i].eulerAngles = new Vector3(0, 90, 0);
            }
        }
        if (pEntrance >= 0)
        {
            doorDirections[pEntrance] = false;
        }
    }

    public int GetRoomType()
    {
        return roomType;
    }

    public Vector2 GetGridPosition()
    {
        return gridPosition;
    }
    public Vector3 GetRoomPosition()
    {
        return new Vector3(room.transform.position.x, 0, room.transform.position.z);
    }

    public int GetRandomDoorDirection()
    {
        List<int> doors = new List<int>();
        for (int i = 0; i < doorDirections.Length; i++)
        {
            if (doorDirections[i])
            {
                doors.Add(i);
            }
        }
        return doors[Random.Range(0, doors.Count)];
    }

    public void ToggleRoomActive(bool toggle)
    {
        room.gameObject.SetActive(toggle);
    }
}
