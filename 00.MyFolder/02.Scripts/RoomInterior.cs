using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInterior : MonoBehaviour {
    public GameObject[] Interiors;
    private void Start()
    {
        for (int i = 0; i < (int)E_Interior.MAX; i++)
        {
            Interiors[i].SetActive(false);
        }

        for (int i = 0; i < (int)E_Interior.MAX; i++)
        {
            E_Interior inter = (E_Interior)i;
            if (PhaseManager.GetInstance().IsOpen(inter))
                Interiors[i].SetActive(true);
        }
    }
}
