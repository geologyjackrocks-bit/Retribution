using UnityEngine;

public class Room_Generation : MonoBehaviour
{
    public GameObject roomPrefab;
    public Sprite[] roomSprites;
    void SpawnRoom(Vector2 position)
    {
        GameObject newRoom = Instantiate(roomPrefab, position, Quaternion.identity);
        SpriteRenderer sr = newRoom.GetComponent<SpriteRenderer>();
        sr.sprite = roomSprites[Random.Range(0, roomSprites.Length)];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
