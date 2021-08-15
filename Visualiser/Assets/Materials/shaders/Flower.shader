
Shader "Unlit/Flower"
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
            #define r _ScreenParams.xy
            #define mod(x, y) (x-y*floor(x/y))
            uniform float PIf; 
            uniform bool kaleidf;
            uniform float orbsf; 
            uniform float zoomf; 
            uniform float contrastf; 
            uniform float orbSizef; 
            uniform float radiusf; 
            uniform float colorShiftf; 
            uniform float sidesf; 
            uniform float rotationf; 
            uniform float sinMulf; 
            uniform float cosMulf; 
            uniform float yMulf; 
            uniform float xMulf; 
            uniform float xSpeedf; 
            uniform float ySpeedf; 
            uniform float gloopf; 
            uniform float yDividef; 
            uniform float xDividef; 
            uniform float speedf, speed1f, speed2f;

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
                angle = ((angle / PIf) + 1.0) * 0.5;
                angle = fmod(angle, 1.0 / sides) * sides;
                angle = -abs(2.0 * angle - 1.0) + 1.0;
                angle = angle;
                float y = length(uv);
                angle = angle * (y);
                return float2(angle, y) - offset;
            }


            

            
            float4 frag (v2f i) : SV_Target
            {
                float4 fragColor;
                fixed2 uv = 23.09 * (2. * i.uv - 1) / 1;
                float dist = length(uv);
                uv = mul(uv,rotate((speedf + speed2f) / 20.));
                if(kaleidf == 0){
                    uv = kale(uv, vec2(6.97), sidesf);
                }
                uv = mul(uv, rotate((speedf + speed2f) / 5.));
                uv = mul(uv, zoomf);
                for (float i = 0.; i < orbsf; i++) {
                    uv.x += 0.57 * sin(0.3 * uv.y + (speedf + speed2f));
                    uv.y -= 0.63 * cos(0.53 * uv.x + (speedf + speed2f));
                    float t = i * PIf / orbsf * 2.;
                    float x = 4.02 * tan(t + (speedf + speed2f) / 10.);
                    float y = 4.02 * cos(t - (speedf + speed2f) / 30.);
                    fixed2 position = vec2(x, y);
                    fixed3 color = cos(vec3(-2, 0, -1) * PIf * 2. / 3. + PIf * (float(i) / 5.37)) * 0.5 + 0.5;
                    fragColor += orb(uv, 1.39, position, color, 1.37);
                }
                fragColor = pow(fragColor, 2.2); // reverses gamma conversion = less washed out
                return fragColor;//float4(uv, uv);
            }
            ENDCG
        }
    }
}
