using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarVis : MonoBehaviour
{
    public Audio audioVisualiser;

    public float[] transform;

    public Material material;

    private Material[] audioMaterial;

    public Gradient gradient;

    public MeshRenderer[] cubes;

    // Start is called before the first frame update
    void Start()
    {
        audioMaterial = new Material[8];

        transform = new float[8];

        for (int i = 0; i < 8; i++)
        {
            // creates new material for each bar based on specified pre-set material
            audioMaterial[i] = new Material(material);
            cubes[i].material = audioMaterial[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            audioMaterial[i] = new Material(material);
            cubes[i].material = audioMaterial[i];

            // transforms bar scale in y dir and emission cilour amount based upon amplitude of specified audio frequency band
            transform[i] = (audioVisualiser.audioBandBuffer[i] * 10 + 1);
            cubes[i].transform.localScale = new Vector3(1, (int) transform[i], 1);
            audioMaterial[i].SetColor("_EmissionColor", gradient.Evaluate((i+1) / 8f) * audioVisualiser.audioBandBuffer[i]);
        }
    }
}
