using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NoiseFlowField))]
public class AudioFlowField : MonoBehaviour
{
    NoiseFlowField noiseFlowField;
    public Audio audio;
    [Header("Speed")]
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;
    [Header("Scale")]
    public bool useScale;
    public Vector2 scaleMinMax;
    public float maxSize {get; set;}
    [Header("Material")]
    public Material material;
    private Material[] audioMaterial;
    public bool useColour1;
    public string colourName1;
    public Gradient gradient1;
    private Color[] colour1;
    [Range(0f,1f)]
    public float colourThreshold1;
    public float colourMultiplier1;
    public bool useColour2;
    public string colourName2;
    public Gradient gradient2;
    private Color[] colour2;
    //[Range(0f,1f)]
    public float colourThreshold2 {get; set;}
    public float colourMultiplier2;

    // Start is called before the first frame update
    void Start()
    {
        maxSize = 3f;
        noiseFlowField = GetComponent<NoiseFlowField>();
        audioMaterial = new Material[8];
        colour1 = new Color[8];
        colour2 = new Color[8];
        for (int i = 0; i < 8; i++)
        {
            colour1[i] = gradient1.Evaluate((1f / 8f) * i);
            colour2[i] = gradient2.Evaluate((1f / 8f) * i);
            audioMaterial[i] = new Material(material);
        }
        int countBand = 0;
        for (int i = 0; i < noiseFlowField.numOfParticles; i++)
        {
            int band = countBand % 8;
            noiseFlowField.particleMeshRenderer[i].material = audioMaterial[band];
            noiseFlowField.particles[i].audioBand = band;
            countBand++;
        }
    }

    public void updateColour(Gradient newGrad){
for (int i = 0; i < 8; i++)
        {
            colour1[i] = newGrad.Evaluate((1f / 8f) * i);
            colour2[i] = newGrad.Evaluate((1f / 8f) * i);
            audioMaterial[i] = new Material(material);
        }
                int countBand = 0;
        for (int i = 0; i < noiseFlowField.numOfParticles; i++)
        {
            int band = countBand % 8;
            noiseFlowField.particleMeshRenderer[i].material = audioMaterial[band];
            noiseFlowField.particles[i].audioBand = band;
            countBand++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateColour(gradient2);
        scaleMinMax.y = maxSize;
        if(useSpeed){
            noiseFlowField.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audio.amplitudeBuffer);
            noiseFlowField.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audio.amplitudeBuffer);
        }
        for (int i = 0; i < noiseFlowField.numOfParticles; i++)
        {
            if(useScale){
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audio.audioBandBuffer[noiseFlowField.particles[i].audioBand]);
                noiseFlowField.particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if(useColour1){
                if(audio.audioBandBuffer[i] > colourThreshold1){
                    audioMaterial[i].SetColor(colourName1, colour1[i] * audio.audioBandBuffer[i] * colourMultiplier1);
                }
                else{
                    audioMaterial[i].SetColor(colourName1, colour1[i] * 0f);
                }
            }
            if(useColour2){
                if(audio.audioBand[i] > colourThreshold2){
                    audioMaterial[i].SetColor(colourName2, colour2[i] * audio.audioBand[i] * colourMultiplier2);
                }
                else{
                    audioMaterial[i].SetColor(colourName2, colour2[i] * 0f);
                }
            }
        }

    }
    public void setIntensity(float intensity){

        colourMultiplier2 = intensity;
    }


}
