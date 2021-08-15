using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phyllo : MonoBehaviour
{
    public Audio audio;
    private Material trailMat;
    public Color trailColor;

    public float degree;
    public float scale;
    public int numStart;
    public int stepSize;
    public int maxIteration;

    // Lerping
    public bool useLerping;
    private bool isLerping;
    private Vector3 startPos, endPos;
    private float lerpPosTimer, lerpPosSpeed;
    public Vector2 lerpPosSpeedMinMax;
    public AnimationCurve lerpPosAnimCurve;
    public int lerpPosBand;
    //
    private int num;
    private int currentIteration;
    private TrailRenderer trailRender;

    private bool forward;
    public bool repeat, invert;

    // Scaling
    public bool useScaleAnim, useScaleCurve;
    public Vector2 scaleAnimMinMax;
    public AnimationCurve scaleAnimCurve;
    public float scaleAnimSpeed;
    public int scaleBand;
    private float scaleTimer, currentScale;

    public void setDegree(float deg){
        degree = deg;
    }

        public  void setScale(bool use){
        useScaleAnim = use;
    }

    public void setStepSize(float ss){
        stepSize = (int)ss;
    }
    
    private Vector2 CalculatePhyllo(float degree, float scale, int number)
    {
        double angle = number * (degree * Mathf.Deg2Rad);

        float radius = scale * Mathf.Sqrt(number);

        float x = radius * (float)System.Math.Cos(angle);

        float y = radius * (float)System.Math.Sin(angle);

        Vector2 vec2 = new Vector2(x, y);

        return vec2;
    }

    private Vector2 phylloPos;

    void SetLerpPos(){

        phylloPos = CalculatePhyllo(degree, currentScale, num);
        startPos = this.transform.localPosition;
        endPos = new Vector3(phylloPos.x, phylloPos.y, 0);
    }
    
    void Awake()
    {
        currentScale = scale;
        forward = true;
        trailRender = GetComponent<TrailRenderer>();
        trailMat = new Material(trailRender.material);
        trailMat.SetColor("TintColor", trailColor);
        trailRender.material = trailMat;
        num = numStart;
        transform.localPosition = CalculatePhyllo(degree, currentScale, num);
        if(useLerping){
            isLerping = true;
            SetLerpPos();
        }

    }

private void Update(){
    if(useScaleAnim){
        if(useScaleCurve){
            scaleTimer += (scaleAnimSpeed * audio.audioBand[scaleBand]) * Time.deltaTime;
            if(scaleTimer >= 1){
                scaleTimer -= 1;
            }
            currentScale = Mathf.Lerp(scaleAnimMinMax.x, scaleAnimMinMax.y, scaleAnimCurve.Evaluate(scaleTimer));

        }
        else{
            currentScale = Mathf.Lerp(scaleAnimMinMax.x, scaleAnimMinMax.y, audio.audioBand[scaleBand]);
        }
    }
    if(useLerping){
        if(isLerping){
            lerpPosSpeed = Mathf.Lerp(lerpPosSpeedMinMax.x, lerpPosSpeedMinMax.y, lerpPosAnimCurve.Evaluate(audio.audioBand[lerpPosBand]));
            lerpPosTimer += Time.deltaTime * lerpPosSpeed;
            transform.localPosition = Vector3.Lerp(startPos, endPos, lerpPosTimer);
            if(lerpPosTimer >= 1){
                lerpPosTimer -= 1;
                if(forward){
                num += stepSize;
                currentIteration++;
                }
                else{
                num -= stepSize;
                currentIteration--;   
                }
                if((currentIteration > 0) && (currentIteration < maxIteration)){
                SetLerpPos();
                }
                else{
                    if(repeat){
                        if(invert){
                            forward = !forward;
                            SetLerpPos();
                        }
                        else{
                            num = numStart;
                            currentIteration = 0;
                            SetLerpPos();
                        }
                    }
                    else{
                        isLerping = false;
                    }
                }
               }
            }
        }
    if (! useLerping) {
        //public speed?
        
    phylloPos = CalculatePhyllo(degree, currentScale, num);
    transform.localPosition = new Vector3(phylloPos.x, phylloPos.y, 0);
    num+= stepSize;
    currentIteration++;
    }
}


}
