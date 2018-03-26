using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGrid : MonoBehaviour
{

    public int col, row;


    void Update()
    {
        RectTransform parent = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();

        grid.cellSize = new Vector2(parent.rect.width / col, parent.rect.height / row);
    }
}
