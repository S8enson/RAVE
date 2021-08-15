using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidAdj : MonoBehaviour
{
    //Animator animator;
    public float multiplier;
    private float buffer;
    private float buf1;
    private float buf2;
    public Audio audio;
    Renderer renderer;
    public bool useAmp, useBand;
    public float test, t1, t2;
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
        //kaleid = 0;

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
            // if(audio.audioBandBuffer[1]>0.3){
            //     //kaleid = 1f;
            //     yDivide-=4f;
            // }else{
            //     yDivide*=1.00001f;
            //     //kaleid = 0f;
            // }
            // Debug.Log(audio.audioBandBuffer[1]);
            //buffer =  Time.timeSinceLevelLoad+audio.audioBandBuffer[1];// * multiplier;
            //zoom = Mathf.Lerp(2, 0.07f, audio.amplitudeBuffer);
            buffer += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[1] + audio.audioBandBuffer[0]));
            buf1 += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[6] + audio.audioBandBuffer[7] + audio.audioBandBuffer[5]));
            buf2 += Time.deltaTime * Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[3] + audio.audioBandBuffer[4] + audio.audioBandBuffer[2]));
            // buffer += Time.deltaTime *Mathf.Lerp(0.1f, 3, test);
            // buf1 += Time.deltaTime *Mathf.Lerp(0.1f, 3, t1);
            // buf2 += Time.deltaTime *Mathf.Lerp(0.1f, 3, t2);
            //yDivide = Mathf.Lerp(5f, 0.05f, audio.audioBandBuffer[1]) - 3;
        }
        // Debug.Log("Adjusted"+ buffer);
        // Debug.Log("Normal"+Time.timeSinceLevelLoad);
        Shader.SetGlobalFloat("kaleid", kaleid);
        Shader.SetGlobalFloat("speed", buffer);
        Shader.SetGlobalFloat("speed1", buf1);
        Shader.SetGlobalFloat("speed2", buf2);
        Shader.SetGlobalFloat("PI", PI);
        Shader.SetGlobalFloat("orbs", orbs);
        Shader.SetGlobalFloat("zoom", zoom);
        Shader.SetGlobalFloat("contrast", contrast);
        Shader.SetGlobalFloat("orbSize", orbSize);
        Shader.SetGlobalFloat("radius", radius);
        Shader.SetGlobalFloat("colorShift", colorShift);
        Shader.SetGlobalFloat("sides", sides);
        Shader.SetGlobalFloat("rotation", rotation);
        Shader.SetGlobalFloat("sinMul", sinMul);
        Shader.SetGlobalFloat("cosMul", cosMul);
        Shader.SetGlobalFloat("yMul", yMul);
        Shader.SetGlobalFloat("xMul", xMul);
        Shader.SetGlobalFloat("xSpeed", xSpeed);
        Shader.SetGlobalFloat("ySpeed", ySpeed);
        Shader.SetGlobalFloat("gloop", gloop);
        Shader.SetGlobalFloat("yDivide", yDivide);
        Shader.SetGlobalFloat("xDivide", xDivide);
        //animator.speed = 0.5f;// * audio.amplitudeBuffer;
    }

    public void Reset()
    {
        kaleid = 0f;
        PI = 3.141592f;
        orbs = 20f;
        zoom = 0.07f;
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
        reactivity = 1f;
    }
}
