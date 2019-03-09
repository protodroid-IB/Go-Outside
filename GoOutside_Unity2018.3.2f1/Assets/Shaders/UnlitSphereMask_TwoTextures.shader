Shader "Custom/Unlit Sphere Mask Two Textures" 
{
    Properties 
	{
        _Color ("Primary Color", Color) = (1,1,1,1)
        _MainTex ("Primary (RGB)", 2D) = "white" {}
        _Color2 ("Secondary Color", Color) = (1,1,1,1)
        _SecondTex ("Secondary (RGB)", 2D) = "white" {}
        _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
        _NoiseTex("Dissolve Noise", 2D) = "white"{}
        _NScale ("Noise Scale", Range(0, 10)) = 1
        _DisAmount("Noise Texture Opacity", Range(0.01, 1)) =0.01

        _DisLineWidth("Line Width", Range(0, 2)) = 0
        _DisLineColor("Line Tint", Color) = (1,1,1,1)  
    }
 
        SubShader
		{
			Lighting Off
            Tags {"RenderType" = "Opaque"}
            LOD 200            
       
			CGPROGRAM
 
			#pragma surface surf Unlit
			#include "UnityCG.cginc"

			half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) 
			{
			   half4 c;
			   c.rgb = s.Albedo;
			   c.a = s.Alpha;
			   return c;
			 }
 
 
			uniform float3 GLOBALMASK_Position; // from script
			uniform float GLOBALMASK_Radius;
 
			sampler2D _MainTex, _SecondTex;
			float4 _Color, _Color2;
			sampler2D _NoiseTex;
			float _DisAmount, _NScale;
			float _DisLineWidth;
			float4 _DisLineColor;
			 
			struct Input {
				float2 uv_MainTex : TEXCOORD0;
				float3 worldPos;// built in value to use the world space position
				float3 worldNormal; // built in value for world normal
   
			};
 
			void surf (Input IN, inout SurfaceOutput o) 
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				half4 c2 = tex2D(_SecondTex, IN.uv_MainTex) * _Color2;
 
				// triplanar noise
				 float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
				half4 nSide1 = tex2D(_NoiseTex, (IN.worldPos.xy + _Time.x) * _NScale);
				half4 nSide2 = tex2D(_NoiseTex, (IN.worldPos.xz + _Time.x) * _NScale);
				half4 nTop = tex2D(_NoiseTex, (IN.worldPos.yz + _Time.x) * _NScale);
 
				float3 noisetexture = nSide1;
				noisetexture = lerp(noisetexture, nTop, blendNormal.x);
				noisetexture = lerp(noisetexture, nSide2, blendNormal.y);
 
				// distance influencer position to world position
				float3 dis = distance(GLOBALMASK_Position, IN.worldPos);
				float3 sphere = 1 - saturate(dis / GLOBALMASK_Radius);
 
				float3 sphereNoise = noisetexture.r * sphere;
 
				float3 DissolveLine = step(sphereNoise - _DisLineWidth, _DisAmount) * step(_DisAmount,sphereNoise) ; // line between two textures
				DissolveLine *= _DisLineColor; // color the line
   
				float3 primaryTex = (step(sphereNoise - _DisLineWidth,_DisAmount) * c.rgb);
				float3 secondaryTex = (step(_DisAmount, sphereNoise) * c2.rgb);
				float3 resultTex = primaryTex + secondaryTex + DissolveLine;

				o.Albedo = resultTex;
 
				o.Emission = DissolveLine;
				o.Alpha = c.a;
   
		}
		ENDCG
 
    }
 
    Fallback "Diffuse"
}