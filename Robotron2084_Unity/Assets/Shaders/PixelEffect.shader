Shader "Hidden/PixelEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;

                float pixelSizeX = 1.0 / _ScreenParams.x;
                float pixelSizeY = 1.0 / _ScreenParams.y;

                float abberAmount = .01;

                float cellSizeX = _ScreenParams.x / 150.0 * pixelSizeX;
                float cellSizeY = _ScreenParams.y / 150.0 * pixelSizeY;

                float x = cellSizeX * floor(i.uv.x / cellSizeX);
                float y = cellSizeY * floor(i.uv.y / cellSizeY);
                col.r = tex2D(_MainTex, fixed2(x - abberAmount, y)).r;
                col.g = tex2D(_MainTex, fixed2(x, y)).g;
                col.b = tex2D(_MainTex, fixed2(x + abberAmount, y)).b;
                col.a = tex2D(_MainTex, fixed2(x, y)).a;
                return col;
            }
            ENDCG
        }
    }
}
