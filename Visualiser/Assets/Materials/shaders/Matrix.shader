Shader "Unlit/Matrix"
{
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
        //_Color ("Tint Color", Color) = (1,1,1,1)
        
    }
    SubShader
    {
        //Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }
        //LOD 100
        Pass
        {
            //ZWrite Off
            //Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            

            float4 vec4(float x,float y,float z,float w){return float4(x,y,z,w);}
            float4 vec4(float x){return float4(x,x,x,x);}
            float4 vec4(float2 x,float2 y){return float4(float2(x.x,x.y),float2(y.x,y.y));}
            float4 vec4(float3 x,float y){return float4(float3(x.x,x.y,x.z),y);}


            float3 vec3(float x,float y,float z){return float3(x,y,z);}
            float3 vec3(float x){return float3(x,x,x);}
            float3 vec3(float2 x,float y){return float3(float2(x.x,x.y),y);}

            float2 vec2(float x,float y){return float2(x,y);}
            float2 vec2(float x){return float2(x,x);}

            float vec(float x){return float(x);}
            
            
            //#include "UnityCG.cginc"
            //#define t _Time.y
            #define ru _ScreenParams.xy
            #define mod(x, y) (x-y*floor(x/y))



            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (
            float4 vertex : POSITION, // vertex position input
            float2 uv : TEXCOORD0 // first texture coordinate input
            )
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.uv = uv;
                return o;
            }
            
 



            // max values - min 0 for all but orbsize(0.01)
            // #define PI 3.141592
            // #define orbs 10.
            // #define zoom 20
            #define stream _Time.y
            // #define speed 0.1
            // #define orbSize2 0.01
            // #define bump 0.9
            // #define contrast 2
            // #define radius 2
            // #define colourShift 10.
            // #define centre 5.
            // #define sides 8
            // #define shape 20
            // #define mult 15
            // #define div 30.
            uniform float PI2; 
            uniform float orbs2; 
            uniform float zoom2; 
            uniform float bump2;
            uniform float contrast2; 
            uniform float orbSize2; 
            uniform float radius2; 
            uniform float colourShift2; 
            uniform float sides2; 
            uniform float shape2; 
            uniform float kaleid2; 
            uniform float mult2; 
            uniform float div2; 
            uniform float centre2;
            uniform float speed2, speed21, speed22;


           float4 orb(float2 uv, float s, float2 p, float3 color, float c) {
                return pow(vec4(s / length(uv + p) * color, 1.), vec4(c));
            }

            float2x2 rotate(float a) {
                return float2x2(cos(a), -sin(a), sin(a), cos(a));
            }

            float2 kale(float2 uv, float2 offset, float side) {
                float angle = atan2(uv.x, uv.y);
                angle = ((angle / PI2) + 1.0) * 0.5;
                angle = fmod(angle, 1.0 / side) * side;
                angle = -abs(2.0 * angle - 1.0) + 1.0;
                angle = angle;
                float y = length(uv);
                angle = angle * (y);
                return float2(angle, y) - offset;
            }

            float4 hue(float4 color, float shift) {
                const float4 kRGBToYPrime = vec4(0.299, 0.587, 0.114, 0.0);
                const float4 kRGBToI = vec4(0.596, -0.275, -0.321, 0.0);
                const float4 kRGBToQ = vec4(0.212, -0.523, 0.311, 0.0);
                const float4 kYIQToR = vec4(1.0, 0.956, 0.621, 0.0);
                const float4 kYIQToG = vec4(1.0, -0.272, -0.647, 0.0);
                const float4 kYIQToB = vec4(1.0, -1.107, 1.704, 0.0);
                float YPrime = dot(color, kRGBToYPrime);
                float I = dot(color, kRGBToI);
                float Q = dot(color, kRGBToQ);
                float hue = atan2(Q, I);
                float chroma = sqrt(I * I + Q * Q);
                hue += shift;
                Q = chroma * sin(hue);
                I = chroma * cos(hue);
                float4 yIQ = vec4(YPrime, I, Q, 0.0);
                color.r = dot(yIQ, kYIQToR);
                color.g = dot(yIQ, kYIQToG);
                color.b = dot(yIQ, kYIQToB);
                return color;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                // float orbs = orbs2;//20;
                // orbs = orbs2
                
                float4 fragColor;
                fixed2 uv = 2. * i.uv - 1;
                //uv.x *= uv.x/uv.y;
                //uv.y = uv.y/(uv.x/uv.y);
                uv = mul(uv, zoom2);
                float dist = length(uv);
                
                uv = mul(uv, rotate(speed2 / 10));
                if(kaleid2 == 10){
                    uv = kale(uv, dist, sides2);
                    }
                
                for (float j = 0.; j < orbs2; j++) {
                    float t = j / PI2 / orbs2 * shape2;
                    float x = dist*t+radius2 * (j/div2) * tan(dist*t-speed21/2.)*mult2*sin(dist+speed22/15.);
                    float y = 0.;
                    float2 position = vec2(x, y);
                    float3 colour = cos(vec3(-2, 0, -1) * PI2 * 2. / 3. + PI2 * (vec(j) / colourShift2)) * 0.5 + 0.5;
                    
                    fragColor += hue(orb(uv, orbSize2, position, colour, contrast2), 10.);
                }
                // return fragColor;
                //return float4(0,0.5,0.5,1);
                fragColor = pow(fragColor, 2.2);
                return fragColor;//float4(uv, uv);
            }
            ENDCG
        }
    }
}
