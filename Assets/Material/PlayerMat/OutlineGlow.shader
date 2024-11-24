Shader "Custom/OutlineGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineSize ("Outline Size", Range(0, 10)) = 1.5  // Increased default
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2.5  // Increased range and default
        _SharpnessFactor ("Sharpness", Range(1, 10)) = 3  // New property for sharpness
    }
    
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;
            float _GlowIntensity;
            float _SharpnessFactor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 texelSize = _MainTex_TexelSize.xy * _OutlineSize;
                
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed alpha = col.a;
                fixed4 outline = 0;
                
                // Sample more points for sharper outline
                for(int j = 1; j <= _SharpnessFactor; j++)
                {
                    float offset = j * (1.0 / _SharpnessFactor);
                    outline += tex2D(_MainTex, i.uv + float2(texelSize.x * offset, 0));
                    outline += tex2D(_MainTex, i.uv - float2(texelSize.x * offset, 0));
                    outline += tex2D(_MainTex, i.uv + float2(0, texelSize.y * offset));
                    outline += tex2D(_MainTex, i.uv - float2(0, texelSize.y * offset));
                    
                    // Diagonals
                    outline += tex2D(_MainTex, i.uv + texelSize * offset);
                    outline += tex2D(_MainTex, i.uv - texelSize * offset);
                    outline += tex2D(_MainTex, i.uv + float2(texelSize.x * offset, -texelSize.y * offset));
                    outline += tex2D(_MainTex, i.uv + float2(-texelSize.x * offset, texelSize.y * offset));
                }
                
                outline /= (_SharpnessFactor * 8);
                outline.rgb = _OutlineColor.rgb * _GlowIntensity;
                outline.a *= _OutlineColor.a;
                
                return lerp(outline, col, alpha);
            }
            ENDCG
        }
    }
}
