using UnityEngine;

/*
 * HEIGHT SPRITE SORTER
 * Updates sorting order of spriterenderer based on your Y position.
 * Higher Y means lower sort order.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class HeightSpriteSorter : MonoBehaviour
{
    //CACHED SpriteRenderer
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        //Cache spriterenderer
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.sortingOrder = (int)(-transform.position.y * 10f);
    }
}
