using UnityEngine;

public class StaticBackground : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
        Vector3 scale = transform.localScale;
        scale.x = worldScreenWidth / GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        scale.y = worldScreenHeight / GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
