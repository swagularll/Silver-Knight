using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class backgroundGenerator : MonoBehaviour
{

    public int mapSize;
    public GameObject mapUnit;
    public int seed = 0;
    public bool specialBackground = false;
    private int imageCount = 7;
    private int baseimageCount = 10;

    private List<Sprite> spriteCollection = new List<Sprite>();
    private List<Sprite> spriteBaseCollection = new List<Sprite>();
    private float map_width = 4;

    void Awake()
    {
        if (!specialBackground)
        {
            for (int i = 0; i < imageCount; i++)
            {
                spriteCollection.Add(Resources.Load<Sprite>(ODMVariable.resource.wall_special_file_name + i.ToString("D2")));
            }
            for (int i = 0; i < baseimageCount; i++)
            {
                spriteBaseCollection.Add(Resources.Load<Sprite>(ODMVariable.resource.wall_file_name + i.ToString("D2")));
            }
        }
        for (int i = 0; i < mapSize; i++)
        {
            GameObject g = Instantiate(mapUnit);
            g.transform.position = new Vector3(i * map_width + 2f, 5, 1);
            g.transform.parent = transform;

            if (i % map_width == 0)
            {
                mapUnit.GetComponent<SpriteRenderer>().sprite = spriteCollection[++seed % spriteCollection.Count];
            }
            else
            {
                mapUnit.GetComponent<SpriteRenderer>().sprite = spriteBaseCollection[(i + seed) % spriteBaseCollection.Count];
            }
        }

        float mapSizeFloat = (float)mapSize;

        transform.GetComponent<BoxCollider2D>().offset = new Vector2((mapSizeFloat / 2) * map_width, -1);
        transform.GetComponent<BoxCollider2D>().size = new Vector2(mapSize * map_width, 1);

        ODMVariable.level.activate_range_x = map_width * mapSize;
    }

}
