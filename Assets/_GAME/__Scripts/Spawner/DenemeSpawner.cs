using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenemeSpawner : MonoBehaviour
{
    public GameObject zeminSablonu;
    [SerializeField] private float xBaslangic;
    [SerializeField] private float yBaslangic;
    [SerializeField] private int sutunBoyutu;
    [SerializeField] private int satirBoyutu;
    [Range(1, 2)] [SerializeField] private int xBosluk;
    [Range(1, 2)] [SerializeField] private int yBosluk;


    [SerializeField] private int katman; 
    [SerializeField] private float katmanRate; 


    private void Start()
    {
        HaritayiOlustur();
    }

    public void HaritayiOlustur()
    {
        for (int j = 0; j < katman; j++)
        {
            for (int i = 0; i < sutunBoyutu * satirBoyutu; i++)
            {
                Instantiate(zeminSablonu, 
                    new Vector3(xBaslangic + (xBosluk * (i % sutunBoyutu)), j * katmanRate, yBaslangic + (yBosluk * (i / sutunBoyutu))), 
                    Quaternion.identity);
            }
        }
        
    }
}
