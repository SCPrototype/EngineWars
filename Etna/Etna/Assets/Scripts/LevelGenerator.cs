using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private const float spawnInterval = 0.1f;
    private float lastSpawn;

    public const int roomGridSize = 100;

    public GameObject SolidWall;
    public GameObject DoorWall;
    public GameObject Roof;

    public GameObject[] RoomDesigns;
    private List<Room> rooms = new List<Room>();
    private List<Room> roomPool = new List<Room>();
    private bool[,] roomGrid = new bool[roomGridSize, roomGridSize];

    // Use this for initialization
    void Start() {
        addNewRoom();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - lastSpawn >= spawnInterval)
        {
            addNewRoom();
            lastSpawn = Time.time;
        }
    }

    private void addNewRoom()
    {
        if (rooms.Count > 0)
        {
            Room lastRoom = rooms[rooms.Count - 1];
            int direction = lastRoom.GetRandomDoorDirection();
            bool[] doors = new bool[] {false,false,false,false};
            int entranceDoor = direction - 2;
            if (entranceDoor < 0)
            {
                entranceDoor = 4 + entranceDoor;
            }
            doors[entranceDoor] = true;
            Vector2 newRoomPos = new Vector2(Mathf.RoundToInt(lastRoom.GetGridPosition().x + directionToVec2(direction).x), Mathf.RoundToInt(lastRoom.GetGridPosition().y + directionToVec2(direction).y));

            for (int i = 0; i < 1; i++)
            {
                int rnd = Random.Range(0, 4);
                Debug.Log(directionToVec2(rnd));
                if (!doors[rnd] && !isGridTileOccupied(Mathf.RoundToInt(newRoomPos.x + directionToVec2(rnd).x), Mathf.RoundToInt(newRoomPos.y + directionToVec2(rnd).y)))
                {
                    Debug.Log("Placing door towards: " + Mathf.RoundToInt(newRoomPos.x + directionToVec2(rnd).x) + " _ " + Mathf.RoundToInt(newRoomPos.y + directionToVec2(rnd).y));
                    doors[rnd] = true;
                } else
                {
                    Debug.Log("Selected tile is not available: " + Mathf.RoundToInt(newRoomPos.x + directionToVec2(rnd).x) + " _ " + Mathf.RoundToInt(newRoomPos.y + directionToVec2(rnd).y));
                    i--;
                }
            }

            int roomDesign = Random.Range(0, RoomDesigns.Length);
            bool roomPlaced = false;
            if (roomPool.Count > 0)
            {
                for (int i = 0; i < roomPool.Count-1; i++)
                {
                    if (roomPool[i].GetRoomType() == roomDesign)
                    {
                        roomPool[i].ToggleRoomActive(true);
                        rooms.Add(roomPool[i]);
                        roomPool[i].RepositionRoom(Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y), doors, SolidWall, DoorWall, entranceDoor);
                        roomPool.RemoveAt(i);
                        roomPlaced = true;
                        break;
                    }
                }
            }

            if (!roomPlaced)
            {
                Room newRoom = new Room();
                newRoom.InitializeRoom(Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y), RoomDesigns[roomDesign], doors, SolidWall, DoorWall, Roof, entranceDoor);
                rooms.Add(newRoom);
            }
            roomGrid[Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y)] = true;

            if (rooms.Count > 6)
            {
                roomPool.Add(rooms[0]);
                rooms[0].ToggleRoomActive(false);
                roomGrid[Mathf.RoundToInt(rooms[0].GetGridPosition().x), Mathf.RoundToInt(rooms[0].GetGridPosition().y)] = false;
                rooms.RemoveAt(0);
            }
        }
        else
        { 
            Room newRoom = new Room();
            newRoom.InitializeRoom(roomGridSize/2, roomGridSize/2, RoomDesigns[Random.Range(0, RoomDesigns.Length)], new bool[] { true, false, false, false }, SolidWall, DoorWall, Roof);
            roomGrid[roomGridSize/2, roomGridSize/2] = true;
            rooms.Add(newRoom);
        }
    }

    private bool isGridTileOccupied(int x, int y)
    {
        return roomGrid[x,y];
    }

    private Vector2 directionToVec2(int direction)
    {
        switch (direction)
        {
            case 0:
                return new Vector2(0, 1);
            case 1:
                return new Vector2(1, 0);
            case 2:
                return new Vector2(0, -1);
            case 3:
                return new Vector2(-1, 0);
            default:
                break;
        }
        Debug.Log("No correct direction input.");
        return new Vector2(0, 0);
    }

    private int vec2ToDirection(Vector2 direction)
    {
        if (direction == new Vector2(0, 1))
        {
            return 0;
        }
        else if (direction == new Vector2(1, 0))
        {
            return 1;
        }
        else if (direction == new Vector2(0, -1))
        {
            return 2;
        }
        else if (direction == new Vector2(-1, 0))
        {
            return 3;
        }
        Debug.Log("No correct direction input.");
        return 0;
    }
}
