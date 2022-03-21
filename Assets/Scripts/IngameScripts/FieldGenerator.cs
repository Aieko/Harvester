using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wheatPrefab;

    [SerializeField] private int wheatNumInCell;

    // Start is called before the first frame update
    void Start()
    {
        var row = (int)GetComponent<BoxCollider>().size.x/2;
        var column = (int)GetComponent<BoxCollider>().size.z/2;

        for (int i = -row; i < row; i++)
        {
            for (int j = -column; j < column; j++)
            {
                for (int k = 0; k < wheatNumInCell; k++)
                {
                    var wheat = Instantiate(wheatPrefab, transform, false);
                    wheat.transform.localPosition = new Vector3(Random.Range((float)i, i+1f), wheatPrefab.transform.position.y, Random.Range((float)j, j+1f));
                    wheat.transform.Rotate(wheat.transform.up, Random.Range(0, 360));
                }
            }
        }    
    }

}
