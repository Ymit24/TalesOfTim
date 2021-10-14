using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject TilePrefab;
    
    float _timer = 0;

    private void Start()
    {
        List<Room> rooms = GenerateRoomLayout(UnityEngine.Random.Range(14, 24));
        GenerateRoomVisuals(rooms, 14, 7);
    }

    void Update()
    {
        //_timer -= Time.deltaTime;
        //if (_timer <= 0)
        //{
        //    _timer = 1.7f;

        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        Destroy(transform.GetChild(i).gameObject);
        //    }
        //    List<Room> rooms = GenerateRoomLayout(UnityEngine.Random.Range(14,24));
        //    GenerateRoomVisuals(rooms, 14, 7);
        //}
    }

    public List<Sprite> WallSprites;

    public enum WallState
    {
        horiz,vert,top_left,top_right,bottom_left,bottom_right,cross,t_right,t_left,t_down,t_up,floor
    }
    public void GenerateRoomVisuals(List<Room> rooms, int width, int height)
    {
        Dictionary<Vector2, bool> map = new Dictionary<Vector2, bool>();
        //Func<int, int, int> vec2_to_int = (int x, int y) => {
        //    return y * max_width + x;
        //};

        foreach (Room room in rooms)
        {
            for (int x = 0; x <= width; x++)
            {
                for (int y = 0; y <= height; y++)
                {
                    int xw = room.roomX * width + x;
                    int yw = room.roomY * height + y;

                    if (map.ContainsKey(new Vector2(xw, yw)) == false)
                    {
                        map.Add(new Vector2(xw, yw), x == 0 || x == width || y == 0 || y == height);
                    }
                }
            }
        }
        Func<int, int, bool> is_wall_at = (int x, int y) => {
            if (map.ContainsKey(new Vector2(x, y)) == false) return false;
            return map[new Vector2(x, y)];
        };
        Func<int, int, WallState> get_wall_state_at = (int x, int y) => {
            bool left = is_wall_at(x - 1, y);
            bool right = is_wall_at(x + 1, y);
            bool up = is_wall_at(x, y + 1);
            bool down = is_wall_at(x, y - 1);
            bool is_wall = is_wall_at(x, y);

            if (is_wall)
            {
                if (left && right && up && down)
                {
                    return WallState.cross;
                }
                else if (!left && !right && up && down)
                {
                    return WallState.vert;
                }
                else if (left && right && !up && !down)
                {
                    return WallState.horiz;
                }
                else if (left && !right && up && !down)
                {
                    return WallState.bottom_right;
                }
                else if (!left && right && up && !down)
                {
                    return WallState.bottom_left;
                }
                else if (left && !right && !up && down)
                {
                    return WallState.top_right;
                }
                else if (!left && right && !up && down)
                {
                    return WallState.top_left;
                }
                else if (left && right && up && !down)
                {
                    return WallState.t_up;
                }
                else if (left && right && !up && down)
                {
                    return WallState.t_down;
                }
                else if (left && !right && up && down)
                {
                    return WallState.t_left;
                }
                else if (!left && right && up && down)
                {
                    return WallState.t_right;
                }
                else
                {
                    return WallState.t_up;
                }
            }
            else
            {
                //Debug.LogError("invalid wall states: " + left + "," + right + "," + up + "," + down);
                return WallState.floor;
            }
        };
        foreach (Vector2 loc in map.Keys)
        {
            //Vector2 loc = new Vector2(index % max_width, index / max_width);
            WallState state = get_wall_state_at((int)loc.x, (int)loc.y);
            GameObject tile = Instantiate(TilePrefab);
            tile.transform.SetParent(transform);
            tile.transform.position = loc;
            tile.GetComponent<SpriteRenderer>().sprite = WallSprites[(int)state];
        }
    }
    #region old_gen
    public void GenerateRoomVisualsOld(List<Room> rooms, int width, int height)
    {
        List<Vector2> tilesPlaced = new List<Vector2>();
        Func<int, int, bool> is_wall_at = (int x, int y) => {
            bool isOnWallBounds = x % (width - 1) == 0 || y % (height - 1) == 0;
            //return isOnWallBounds;
            int roomX = Mathf.FloorToInt((float)x / (float)(width));
            int roomY = Mathf.FloorToInt((float)y / (float)(height));
            if (isOnWallBounds == false) return false;
            foreach (Room room in rooms)
            {
                if (room.roomX == roomX && room.roomY == roomY) return true;
            }
            return false;
        };
        Func<int, int, WallState> get_wall_state_at = (int x, int y) => {
            bool left = is_wall_at(x - 1, y);
            bool right = is_wall_at(x + 1, y);
            bool up = is_wall_at(x, y+1);
            bool down = is_wall_at(x, y-1);
            bool is_wall = is_wall_at(x, y);
            //return is_wall ? WallState.cross : WallState.floor;
            if (is_wall)
            {
                if (left && right && up && down)
                {
                    return WallState.cross;
                }
                else if (!left && !right && up && down)
                {
                    return WallState.vert;
                }
                else if (left && right && !up && !down)
                {
                    return WallState.horiz;
                }
                else if (left && !right && up && !down)
                {
                    return WallState.bottom_right;
                }
                else if (!left && right && up && !down)
                {
                    return WallState.bottom_left;
                }
                else if (left && !right && !up && down)
                {
                    return WallState.top_right;
                }
                else if (!left && right && !up && down)
                {
                    return WallState.top_left;
                }
                else if (left && right && up && !down)
                {
                    return WallState.t_up;
                }
                else if (left && right && !up && down)
                {
                    return WallState.t_down;
                }
                else if (left && !right && up && down)
                {
                    return WallState.t_left;
                }
                else if (!left && right && up && down)
                {
                    return WallState.t_right;
                }
                else
                {
                    return WallState.t_up;
                }
            }
            else
            {
                //Debug.LogError("invalid wall states: " + left + "," + right + "," + up + "," + down);
                return WallState.floor;
            }
        };
        foreach (Room room in rooms)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int wXPos = room.roomX * (width-1) + x;
                    int wYPos = room.roomY * (height-1) + y;
                    if (tilesPlaced.Contains(new Vector2(wXPos, wYPos)))
                    {
                        continue;
                    }
                    tilesPlaced.Add(new Vector2(wXPos, wYPos));
                    WallState state = get_wall_state_at(wXPos, wYPos);
                    GameObject tile = Instantiate(TilePrefab);
                    tile.transform.SetParent(transform);
                    tile.transform.position = new Vector2(wXPos, wYPos);
                    tile.GetComponent<SpriteRenderer>().sprite = WallSprites[(int)state];
                }
            }
        }
    }
    #endregion

    public class Room
    {
        public int roomX, roomY;
        public List<Room> neighbors;

        public Room(int x, int y)
        {
            roomX = x;
            roomY = y;
            neighbors = new List<Room>();
        }
    }
    class RoomLocation
    {
        public int x, y;
        public Room parent;

        public RoomLocation(int x, int y, Room parent)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }
    }
    public List<Room> GenerateRoomLayout(int roomCount)
    {
        List<RoomLocation> availableSpaces = new List<RoomLocation>();
        List<Room> rooms = new List<Room>();
        Room rootRoom = new Room(0, 0);
        rooms.Add(rootRoom);

        Func<int, int, Room> get_room_at = (int x, int y) => {
            foreach (Room room in rooms)
            {
                if (room.roomX == x && room.roomY == y) return room;
            }
            return null;
        };

        availableSpaces.Add(new RoomLocation(-1,  0, rootRoom));
        availableSpaces.Add(new RoomLocation( 1,  0, rootRoom));
        availableSpaces.Add(new RoomLocation( 0, -1, rootRoom));
        availableSpaces.Add(new RoomLocation( 0,  1, rootRoom));

        for (int i = 0; i < roomCount; i++)
        {
            int randomQueueIndex = UnityEngine.Random.Range(0, availableSpaces.Count);
            RoomLocation location = availableSpaces[randomQueueIndex];
            availableSpaces.RemoveAt(randomQueueIndex);

            Room newRoom = new Room(location.x, location.y);
            newRoom.neighbors.Add(location.parent);
            rooms.Add(newRoom);

            Room l = get_room_at(location.x - 1, location.y);
            Room r = get_room_at(location.x + 1, location.y);
            Room u = get_room_at(location.x, location.y + 1);
            Room d = get_room_at(location.x, location.y - 1);
            if (l == null)
            {
                availableSpaces.Add(new RoomLocation(location.x - 1, location.y, newRoom));
            }
            if (r == null)
            {
                availableSpaces.Add(new RoomLocation(location.x + 1, location.y, newRoom));
            }
            if (u == null)
            {
                availableSpaces.Add(new RoomLocation(location.x, location.y + 1, newRoom));
            }
            if (d == null)
            {
                availableSpaces.Add(new RoomLocation(location.x, location.y - 1, newRoom));
            }
        }

        Debug.Log(rooms.Count + ":");
        return rooms;
    }
}
