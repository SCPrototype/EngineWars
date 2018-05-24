using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private GameManager gameManager;
    private GameObject player;

    private const int maxActiveRoomAmount = 6;

    public const int roomGridSize = 500;

    public GameObject SolidWall;

    public GameObject[] RoomDesigns;
    private List<Room> rooms = new List<Room>();
    private List<Room> roomPool = new List<Room>();
    private bool[,] roomGrid = new bool[roomGridSize, roomGridSize];

    // Use this for initialization
    void Start() {
        if (GameObject.FindGameObjectWithTag("GameManager") != null)
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() != null) {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        for (int i = 0; i < maxActiveRoomAmount/2; i++)
        {
            AddNewRoom();
        }
        if (rooms.Count > 1)
        {
            player.transform.position = rooms[0].GetRoomPosition() + (0.45f * (rooms[0].GetRoomPosition() - rooms[1].GetRoomPosition())) + new Vector3(0, 18.5f, 3.5f);
            //player.transform.LookAt(rooms[1].GetRoomPosition() + new Vector3(0, 25f, 3.5f));
        }
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(player.transform.position, rooms[rooms.Count-1].GetRoomPosition()) <= 50 && Vector3.Distance(player.transform.position, rooms[0].GetRoomPosition()) >= 50)
        {
            AddNewRoom();
        }

        for (int i = rooms.Count-1; i >= 0; i--)
        {
            if (Vector3.Distance(new Vector3(player.transform.position.x, rooms[i].GetRoomPosition().y, player.transform.position.z), rooms[i].GetRoomPosition()) <= 25)
            {
                player.GetComponent<Respawn>().SetRespawnPos(rooms[i].GetRoomPosition() + (0.45f * (rooms[i].GetRoomPosition() - rooms[i+1].GetRoomPosition())) + new Vector3(0, 18.5f, 3.5f));
                break;
            }
        }
    }

    public void AddNewRoom()
    {
        if (rooms.Count > 0)
        {
            Room lastRoom = rooms[rooms.Count - 1];
            int direction = lastRoom.GetRandomDoorDirection();
            bool[] doors = new bool[] {false,true,false,true};
            int entranceDoor = direction - 2;
            if (entranceDoor < 0)
            {
                entranceDoor = 4 + entranceDoor;
            }
            //doors[entranceDoor] = true;
            Vector2 newRoomPos = new Vector2(Mathf.RoundToInt(lastRoom.GetGridPosition().x + directionToVec2(direction).x), Mathf.RoundToInt(lastRoom.GetGridPosition().y + directionToVec2(direction).y));
            //doors[getRandomDirection(doors, newRoomPos)] = true;

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
                        roomPool[i].RepositionRoom(Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y), doors, entranceDoor);
                        if (gameManager != null)
                        {
                            gameManager.AddToDarknessPath(roomPool[i].GetRoomPosition());
                        }
                        roomPool.RemoveAt(i);
                        roomPlaced = true;
                        break;
                    }
                }
            }

            if (!roomPlaced)
            {
                Room newRoom = new Room();
                newRoom.InitializeRoom(Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y), RoomDesigns[roomDesign], doors, entranceDoor);
                rooms.Add(newRoom);
                if (gameManager != null)
                {
                    gameManager.AddToDarknessPath(newRoom.GetRoomPosition());
                }
            }
            roomGrid[Mathf.RoundToInt(newRoomPos.x), Mathf.RoundToInt(newRoomPos.y)] = true;

            if (rooms.Count > maxActiveRoomAmount)
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
            bool[] doors = new bool[] { false, true, false, false };
            //doors[Random.Range(0, doors.Length)] = true;
            newRoom.InitializeRoom(roomGridSize/2, roomGridSize/2, RoomDesigns[Random.Range(0, RoomDesigns.Length)], doors);
            roomGrid[roomGridSize/2, roomGridSize/2] = true;
            rooms.Add(newRoom);
            Transform wall = GameObject.Instantiate(SolidWall).transform;
            wall.position = newRoom.GetRoomPosition() + new Vector3(-28.5f, 21.8f, 3.36f);
            wall.eulerAngles = new Vector3(0, 90, 0);
            if (gameManager != null)
            {
                gameManager.AddToDarknessPath(newRoom.GetRoomPosition());
            }
        }
    }

    private int getRandomDirection(bool[] doors, Vector2 roomPos)
    {
        int dir = 0;
        int rnd = Random.Range(0, roomGridSize);
        int counter = 0;
        int[] dirPercentages = new int[4];
        //Gives each direction a percentile chance to be chosen, based on the position of the current room. Example: If the room is 20 units on the X-axis from the center, the chance to reduce that number is larger than increasing it.
        dirPercentages[0] = (roomGridSize / 4) - Mathf.RoundToInt((roomPos.y - (roomGridSize / 2)) / 2);
        dirPercentages[1] = (roomGridSize / 4) - Mathf.RoundToInt((roomPos.x - (roomGridSize / 2)) / 2);
        dirPercentages[2] = (roomGridSize / 4) + Mathf.RoundToInt((roomPos.y - (roomGridSize / 2)) / 2);
        dirPercentages[3] = (roomGridSize / 4) + Mathf.RoundToInt((roomPos.x - (roomGridSize / 2)) / 2);

        for (int i = 0; i < 100; i++)
        {
            rnd = Random.Range(0, roomGridSize);
            counter = 0;
            for (int j = 0; j < dirPercentages.Length; j++)
            {
                counter += dirPercentages[j];
                if (rnd < counter)
                {
                    dir = j;
                    break;
                }
            }
            //rnd < Mathf.Abs(roomPos.x - (roomGridSize / 2)) * 2 && roomPos.x - (roomGridSize / 2) >= 0
            if (!doors[dir] && !isGridTileOccupied(Mathf.RoundToInt(roomPos.x + directionToVec2(dir).x), Mathf.RoundToInt(roomPos.y + directionToVec2(dir).y)))
            {
                //Debug.Log("Placing door towards: " + Mathf.RoundToInt(roomPos.x + directionToVec2(dir).x) + " _ " + Mathf.RoundToInt(roomPos.y + directionToVec2(dir).y));
                return dir;
            }
            else
            {
                //Debug.Log("Selected tile is not available: " + Mathf.RoundToInt(roomPos.x + directionToVec2(dir).x) + " _ " + Mathf.RoundToInt(roomPos.y + directionToVec2(dir).y));
            }
        }
        //No good direction found.
        Debug.Log("No good direction found!");
        return 0;
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
