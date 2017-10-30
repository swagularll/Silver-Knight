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
    int imageCount = 7;
    int baseimageCount = 10;

    private List<Sprite> spriteCollection = new List<Sprite>();
    private List<Sprite> spriteBaseCollection = new List<Sprite>();

    void Awake()
    {
        if (!specialBackground)
        {
            for (int i = 0; i < imageCount; i++)
            {
                spriteCollection.Add(Resources.Load<Sprite>("Background/" + "Wallspecial" + i.ToString("D2")));
            }
            for (int i = 0; i < baseimageCount; i++)
            {
                spriteBaseCollection.Add(Resources.Load<Sprite>("Background/" + "Wallbase" + i.ToString("D2")));
            }
        }
        for (int i = 0; i < mapSize; i++)
        {
            GameObject g = Instantiate(mapUnit);
            g.transform.position = new Vector3(i * 4 + 2f, 5, 1);
            g.transform.parent = transform;

            if (i % 4 == 0)
            {
                mapUnit.GetComponent<SpriteRenderer>().sprite = spriteCollection[++seed % spriteCollection.Count];
            }
            else
            {
                mapUnit.GetComponent<SpriteRenderer>().sprite = spriteBaseCollection[(i + seed) % spriteBaseCollection.Count];
            }
        }

        float mapSizeFloat = (float)mapSize;

        transform.GetComponent<BoxCollider2D>().offset = new Vector2((mapSizeFloat / 2) * 4, -1);
        transform.GetComponent<BoxCollider2D>().size = new Vector2(mapSize * 4, 1);
    }

}
