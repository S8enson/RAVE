using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAdj : MonoBehaviour
{
    //Animator animator;
    public float multiplier;
    private float buffer;
    private float buf1;
    private float buf2;
    public Audio audio;
    Renderer renderer;
    public bool useAmp, useBand;
    public float kaleid { get; set; }
    public float PI { get; set; }
    public float orbs { get; set; }
    public float zoom { get; set; }
    public float contrast { get; set; }
    public float orbSize { get; set; }
    public float radius { get; set; }
    public float colorShift { get; set; }
    public float sides { get; set; }
    public float rotation { get; set; }
    public float sinMul { get; set; }
    public float cosMul { get; set; }
    public float yMul { get; set; }
    public float xMul { get; set; }
    public float xSpeed { get; set; }
    public float ySpeed { get; set; }
    public float gloop { get; set; }
    public float yDivide { get; set; }
    public float xDivide { get; set; }
    public float reactivity { get; set; }

    // Start is called before the first frame update
    void Start()
    {


        renderer = gameObject.GetComponent<Renderer>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (useAmp)
        {
            //buffer = Time.timeSinceLevelLoad + audio.amplitudeBuffer;// *  multiplier;Time.deltaTime;
            buffer += Time.deltaTime * Mathf.Lerp(1, 5, audio.amplitudeBuffer);
        }
        else if (useBand)
        {
            //buffer =  Time.timeSinceLevelLoad+audio.audioBandBuffer[1];// * multiplier;
            buffer += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[1] + audio.audioBandBuffer[0]));
            buf1 += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[6] + audio.audioBandBuffer[7] + audio.audioBandBuffer[5]));
            buf2 += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[3] + audio.audioBandBuffer[4] + audio.audioBandBuffer[2]));
        }
        // Debug.Log("Adjusted"+ buffer);
        // Debug.Log("Normal"+Time.timeSinceLevelLoad);
        Shader.SetGlobalFloat("kaleidf", kaleid);
        Shader.SetGlobalFloat("speedf", buffer);
        Shader.SetGlobalFloat("speed1f", buf1);
        Shader.SetGlobalFloat("speed2f", buf2);
        Shader.SetGlobalFloat("PIf", PI);
        Shader.SetGlobalFloat("orbsf", orbs);
        Shader.SetGlobalFloat("zoomf", zoom);
        Shader.SetGlobalFloat("contrastf", contrast);
        Shader.SetGlobalFloat("orbSizef", orbSize);
        Shader.SetGlobalFloat("radiusf", radius);
        Shader.SetGlobalFloat("colorShiftf", colorShift);
        Shader.SetGlobalFloat("sidesf", sides);
        Shader.SetGlobalFloat("rotationf", rotation);
        Shader.SetGlobalFloat("sinMulf", sinMul);
        Shader.SetGlobalFloat("cosMulf", cosMul);
        Shader.SetGlobalFloat("yMulf", yMul);
        Shader.SetGlobalFloat("xMulf", xMul);
        Shader.SetGlobalFloat("xSpeedf", xSpeed);
        Shader.SetGlobalFloat("ySpeedf", ySpeed);
        Shader.SetGlobalFloat("gloopf", gloop);
        Shader.SetGlobalFloat("yDividef", yDivide);
        Shader.SetGlobalFloat("xDividef", xDivide);

    }

    public void Reset()
    {
        PI = 3.141592f;
        orbs = 20f;
        zoom = 1f;
        contrast = 0.13f;
        orbSize = 6.46f;
        radius = 11f;
        colorShift = 10.32f;
        sides = 6f;
        rotation = 1f;
        sinMul = 0f;
        cosMul = 2.38f;
        yMul = 0f;
        xMul = 0.28f;
        xSpeed = 0;
        ySpeed = 0f;
        gloop = 0.003f;
        yDivide = 4.99f;
        xDivide = 6.27f;
        kaleid = 0;
        reactivity = 2;
    }
}
