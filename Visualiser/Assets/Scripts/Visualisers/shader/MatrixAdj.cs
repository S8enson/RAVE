using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixAdj : MonoBehaviour
{
    //Animator animator;

    private float buffer;
    private float buf1;
    private float buf2;
    public Audio audio;
    Renderer renderer;
    public bool useAmp, useBand;

    public float kaleid {get; set;}
                public float PI {get; set;} 
            public float orbs {get; set;}
            public float zoom {get; set;}
            public float contrast {get; set;}
            public float orbSize {get; set;}
            public float radius {get; set;}
            public float colourShift {get; set;}
            public float sides {get; set;}
            public float bump {get; set;}
            public float shape {get; set;}
            public float mult {get; set;}
            public float div {get; set;}
            public float centre {get; set;}
            public float reactivity {get; set;}

    
    // Start is called before the first frame update
    void Start()
    {
        

        renderer = gameObject.GetComponent<Renderer>();
        Reset();
        //kaleid = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(useAmp){
        //buffer = Time.timeSinceLevelLoad + audio.amplitudeBuffer;// *  multiplier;Time.deltaTime;
        buffer += Time.deltaTime * Mathf.Lerp(1, 5, audio.amplitudeBuffer);
        }
        else if(useBand){

            buf1 += Time.deltaTime *Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[1] + audio.audioBandBuffer[0]));
            buffer += Time.deltaTime *Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[6] + audio.audioBandBuffer[7] + audio.audioBandBuffer[5]));
            buf2 += Time.deltaTime *Mathf.Lerp(0.1f, reactivity, Mathf.Max(audio.audioBandBuffer[3] + audio.audioBandBuffer[4] + audio.audioBandBuffer[2]));

        }

        Shader.SetGlobalFloat("kaleid2", kaleid);
        Shader.SetGlobalFloat("speed2", buffer);
        Shader.SetGlobalFloat("speed21", buf1);
        Shader.SetGlobalFloat("speed22", buf2);
         Shader.SetGlobalFloat("PI2", PI);
         Shader.SetGlobalFloat("orbs2", orbs);
        Shader.SetGlobalFloat("zoom2", zoom);
        Shader.SetGlobalFloat("contrast2", contrast);
        Shader.SetGlobalFloat("orbSize2", orbSize);
        Shader.SetGlobalFloat("radius2", radius);
        Shader.SetGlobalFloat("colourShift2", colourShift);
        Shader.SetGlobalFloat("sides2", sides);
        Shader.SetGlobalFloat("bump2", bump);
        Shader.SetGlobalFloat("centre2", centre);
        Shader.SetGlobalFloat("shape2", shape);
        Shader.SetGlobalFloat("mult2", mult);
        Shader.SetGlobalFloat("div2", div);

    }

    public void Reset(){
            kaleid = 10f;
             zoom =15f;
            orbs = 10f;
            PI = 3.141592f;
            // speed 0.03
             orbSize =0.01f;
             bump =0.7f;
             contrast =0.6f;
             radius =0.2f;
             colourShift= 6f;
             centre =3f;
             sides =4f;
             shape =2f;
             mult =5f;
             div =10f;
             reactivity = 2f;
    }
}
