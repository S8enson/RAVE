Shader "Unlit/S1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define t _Time.y
            #define r _ScreenParams.xy
            #define mod(x, y) (x-y*floor(x/y))
            uniform float _speed;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenCoord : TEXCOORD1;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenCoord.xy = ComputeScreenPos(o.vertex);
                return o;
            }




            fixed4 frag (v2f i) : SV_Target
            {

                //adjust t based on amplitude/band
                fixed3 c;
                fixed l, z=_speed;

                for(int j=0;j<3;j++) {
                    fixed2 uv, p=i.uv.xy;
                    uv=p;
                    p-=.5;
                    p.x*=r.x/r.y;
                    z+=.07;
                    l=length(p);
                    uv+=p/l*(sin(z)+1.)*abs(sin(l*9.-z*2.));
                    l=l;
                    if(j==0){
                        c.x=.001/length(abs(mod(uv,1.)-.5));
                    }
                    else if(j==1){
                        c.y=.001/length(abs(mod(uv,1.)-.5));
                    }
                    else{
                        c.z=.001/length(abs(mod(uv,1.)-.5));
                    }
                    

                }
                return float4(c/l,t);
            }
            ENDCG
        }
    }
}
