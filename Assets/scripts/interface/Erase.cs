using UnityEngine;
using System.Collections;

public class Erase : MonoBehaviour {

    public void ClickON()
    {
        if (SquareCreator.pool.Count != 0)
        {
            Destroy(SquareCreator.pool[SquareCreator.pool.Count - 1]);
            SquareCreator.pool.RemoveAt(SquareCreator.pool.Count - 1);
        }
    }
}
