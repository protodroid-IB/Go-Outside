Shader "Custom/VignetteImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amount("Amount", Range(0.0, 1)) = 0.0005
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
            float _Amount;
			float4 _MainTex_TexelSize;
  
            fixed4 frag (v2f i) : SV_Target
            {
				float2 center = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);

				float2 distanceToCenter = ( i.uv.x / center.x , i.uv.y / center.y );

				if(distanceToCenter.x > 1)
				{
					distanceToCenter.x = distanceToCenter.x - 1;
				}

				if(distanceToCenter.y > 1)
				{
					distanceToCenter.y = distanceToCenter.x - 1;
				}

				distanceToCenter = normalize(distanceToCenter);
				
				float intensity = length(distanceToCenter);

				float3 color = tex2D(_MainTex, i.uv).rgb * intensity;

                //float colR = tex2D(_MainTex, float2(i.uv.x - _Amount, i.uv.y - _Amount)).r;
                //float colG = tex2D(_MainTex, i.uv).g;
                //float colB = tex2D(_MainTex, float2(i.uv.x + _Amount, i.uv.y + _Amount)).b;
                return fixed4(color.r, color.g, color.b, 1);
            }
            ENDCG
        }
    }
}