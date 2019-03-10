Shader "Custom/Unlit Sphere Mask Dissolve Field of Vision" {
    Properties 
	{
        _Color ("Primary Color", Color) = (1,1,1,1)
        _MainTex ("Primary (RGB)", 2D) = "white" {}
        _NoiseTex("Dissolve Noise", 2D) = "white"{}
        _NScale ("Noise Scale", Range(0, 10)) = 1
        _DisAmount("Noise Texture Opacity", Range(0.0001, 1)) =0.01

        _DisLineWidth("Line Width", Range(0, 2)) = 0
        _DisLineColor("Line Tint", Color) = (1,1,1,1)
		_FlashIntensity("Flash Intensity", Range(0, 1)) = 0
    }
 
    SubShader
	{
		Lighting Off
        Tags {"RenderType" = "Opaque"}
        LOD 200
		Cull Off
       
		CGPROGRAM
 
		#pragma surface surf Unlit
		#include "UnityCG.cginc"

		half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
           half4 c;
           c.rgb = s.Albedo;
           c.a = s.Alpha;
           return c;
         }


		uniform float3 GLOBALMASK_Position; // from script
		uniform float GLOBALMASK_Radius;
 
		sampler2D _MainTex;
		float4 _Color;
		sampler2D _NoiseTex;
		float _DisAmount, _NScale;
		float _DisLineWidth;
		float4 _DisLineColor;

		float _FlashIntensity;
		
 
 
		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			//float2 uv_SecondTex : TEXCOORD1;
			float3 worldPos;// built in value to use the world space position
			float3 worldNormal; // built in value for world normal
   
		};
 
		void surf (Input IN, inout SurfaceOutput o) {
			//half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			
			if(_FlashIntensity > 0)
			{
				_Color.g = 1 - _FlashIntensity;
				_Color.b = 1 - _FlashIntensity;
			}

			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			
			
 
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
			DissolveLine *= c.rgb; // color the line
   
			//float3 primaryTex = (step(sphereNoise - _DisLineWidth,_DisAmount) * c.rgb);
			float3 mainTex = (step(_DisAmount, sphereNoise) * c.rgb);
			float3 resultTex = mainTex + DissolveLine;

			half test = tex2D(_NoiseTex, IN.uv_MainTex).rgb - (10.0 * sphere);
			clip(-test);

			o.Albedo = resultTex;
 
			o.Emission = DissolveLine;
			o.Alpha = c.a;
   
		}
		ENDCG
	}
 
	Fallback "Diffuse"
}