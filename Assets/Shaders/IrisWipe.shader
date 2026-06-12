Shader "Custom/IrisWipe"
{
    Properties
    {
        _Radius      ("Radio del iris",   Range(0, 2))    = 0.15
        _Softness    ("Suavidad",         Range(0, 0.05)) = 0.01
        _Center      ("Centro",           Vector)         = (0.5,0.5,0,0)
        _WhiteAlpha  ("Alpha blanco",     Range(0, 1))    = 1
        _FadeAlpha   ("Alpha negro ext.", Range(0, 1))    = 1
        _AspectRatio ("Aspect Ratio",     Float)          = 1.777
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };
            struct v2f {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float  _Radius;
            float  _Softness;
            float4 _Center;
            float  _WhiteAlpha;
            float  _FadeAlpha;
            float  _AspectRatio;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 centered = uv - float2(0.5, 0.5);
                centered.x *= _AspectRatio;
                float dist = length(centered);

                // inside = 1 dentro del circulo, 0 fuera
                float inside = 1.0 - smoothstep(
                    _Radius - _Softness,
                    _Radius + _Softness,
                    dist
                );

                // Fuera del circulo: negro con _FadeAlpha
                // Dentro del circulo: blanco con _WhiteAlpha (0 = transparente = ventana)
                float alpha = (1.0 - inside) * _FadeAlpha + inside * _WhiteAlpha;
                float rgb   = inside * _WhiteAlpha; // negro fuera, blanco dentro solo si _WhiteAlpha > 0

                return float4(rgb, rgb, rgb, alpha);
            }
            ENDCG
        }
    }
}