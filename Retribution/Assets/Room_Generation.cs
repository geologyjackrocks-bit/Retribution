using Unity.Properties;
using UnityEngine;
// none of this stuff actually works btw
public class Room_Generation : MonoBehaviour
{
    public GameObject roomPrefab;
    public Sprite[] roomSprites;
    public int roomCount = 5;
    public float roomSpacing = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateRooms(); 
    }

    void GenerateRooms()
    {
        Vector2 currentPosition = Vector2.zero;

        for (int i = 0; i < roomCount; i++)
        {
            GameObject newRoom = Instantiate(roomPrefab, currentPosition, Quaternion.identity);
            SpriteRenderer sr = newRoom.GetComponent<SpriteRenderer>();
            sr.sprite = roomSprites[Random.Range(0, roomSprites.Length)];
            currentPosition += Vector2.right * roomSpacing;
        }
    }  

    // Update is called once per frame
    void Update()
    {
        while (true) { }
    }
}
