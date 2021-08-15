
Shader "Unlit/Acid"
{
    Properties{
        _MainTex ("Texture", 2D) = "black" {}
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
            //#define r _ScreenParams.xy
            #define mod(x, y) (x-y*floor(x/y))
            uniform float kaleid;
            uniform float PI; 
            uniform float orbs; 
            uniform float zoom; 
            uniform float contrast; 
            uniform float orbSize; 
            uniform float radius; 
            uniform float colorShift; 
            uniform float sides; 
            uniform float rotation; 
            uniform float sinMul; 
            uniform float cosMul; 
            uniform float yMul; 
            uniform float xMul; 
            uniform float xSpeed; 
            uniform float ySpeed; 
            uniform float gloop; 
            uniform float yDivide; 
            uniform float xDivide; 
            uniform float speed, speed1, speed2;

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
            
            float4 orb(float2 uv, float s, float2 p, float3 color, float c) {
                return pow(vec4(s / length(uv + p) * color, 1.), vec4(c));
            }

            float2x2 rotate(float a) {
                return float2x2(cos(a), -sin(a), sin(a), cos(a));
            }
            float2 kale(float2 uv, float2 offset, float sides) {
                float angle = atan2(uv.x, uv.y);
                angle = ((angle / PI) + 1.0) * 0.5;
                angle = fmod(angle, 1.0 / sides) * sides;
                angle = -abs(2.0 * angle - 1.0) + 1.0;
                angle = angle;
                float y = length(uv);
                angle = angle * (y);
                return float2(angle, y) - offset;
            }


            // #define PI 3.141592
            // #define orbs 20.
            // #define zoom 0.07
            // #define contrast 0.13//0.53
            // #define orbSize 6.46
            // #define radius 11.
            // #define colorShift 10.32
            // #define sides 1.
            // #define rotation 1.
            // #define sinMul 0.
            // #define cosMul 2.38
            // #define yMul 0.
            // #define xMul 0.28
            // #define xSpeed 0.
            // #define ySpeed 0.
            // #define gloop 0.003;
            // #define yDivide 4.99
            // #define xDivide 6.27
            /*

            Variant 02

            #define zoom 0.27
            #define contrast 0.13
            #define orbSize 4.25
            #define radius 11.
            #define colorShift 10.32
            #define sides 1.
            #define rotation 1.
            #define sinMul 0.
            #define cosMul 2.38
            #define yMul 0.
            #define xMul 0.28
            #define xSpeed 0.
            #define ySpeed 0.
            #define gloop 0.003
            #define yDivide 11.
            #define xDivide 12.4

            */

            /*

            Variant 03

            #define zoom 0.02
            #define contrast 0.13
            #define orbSize 11.
            #define radius 3.21
            #define colorShift 10.32
            #define sides 1.
            #define rotation 1.
            #define sinMul 0.
            #define cosMul 5.
            #define yMul 0.
            #define xMul 0.28
            #define xSpeed 0.
            #define ySpeed 0.
            #define gloop 0.003
            #define yDivide 10.99
            #define xDivide 12.

            */

            
            float4 frag (v2f i) : SV_Target
            {
                // float orbs = orbs2;//20;
                // orbs = orbs2
                
                float reactive = speed;
                float4 fragColor;// = (1,1,1,0.1);

                fixed2 uv = (2. * i.uv - 1) /1;// i.uv.y;
                
                uv = mul(uv, zoom);
                uv /= dot(uv, uv);
                uv = mul(uv, rotate(rotation * reactive / 10.));//mul(uv, rotate(rotation * _Time.y / 10.));
                if(kaleid == 10){uv = kale(uv, 6.97, sides);}
                // uv = mul(uv, rotate(rotation * reactive / 5.));
                for (float j = 0.; j < orbs; j++) {
                    reactive = speed;//  _Time.y;//_speed;// * _Time.y;
                    // float xSpeed = _speed;
                    // float ySpeed = _speed;
                    uv.x += sinMul * sin(uv.y * yMul + reactive * xSpeed) + cos(uv.y / yDivide - reactive);
                    uv.y += cosMul * cos(uv.x * xMul - speed1 * ySpeed) - sin(uv.x / xDivide - speed1);
                    //uv = kale(uv, 6.97, sides);
                    float t = j * PI / orbs * 2.;
                    float x = radius * tan(t);
                    float y = radius * cos(t + speed2 / 10.);
                    float2 position = vec2(x, y);
                    //uv = kale(uv, 6.97, sides);
                    float3 color = cos(.02 * uv.x + .02 * uv.y * vec3(-2, 0, -1) * PI * 2. / 3. + PI * (float(j) / colorShift)) * 0.5 + 0.5;
                    fragColor += .65 - orb(uv, orbSize, position, 1. - color, contrast);
                }
                // return fragColor;
                //return float4(0,0.5,0.5,1);
                return fragColor;//float4(uv, uv);
            }
            ENDCG
        }
    }
}
