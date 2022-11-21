Shader "Unlit/Learning Shader"{
    Properties{
        //_MainTex ("Texture", 2D) = "white" {}
        _Value = ("Value", float ) = 1.0
    }
    SubShader{
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData{ // per-vertex mesh data
                float4 vertex : POSITION; //vertex position
                /*float3 normals : NORMAL;
                float4 tangent : TANGENT;
                float4 color : color;*/
                float2 uv : TEXCOORD0; // uv coordinates
                //float2 uv1 : TEXCOORD1; // uv1 coordinates
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (MeshData v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
