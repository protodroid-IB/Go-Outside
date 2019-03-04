Shader "Custom/Toon_SphereMask_TextureSwap_Roof"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

		_Color2 ("Secondary Color", Color) = (1,1,1,1)
        _SecondTex ("Secondary (RGB)", 2D) = "white" {}

		_ToonRamp ("Toon Ramp", 2D) = "white" {}

		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range(0,1)) = 0.0
		_DissolveScale("Dissolve Scale", Range(0,10)) = 1.0
		_DissolveLineWidth("Line Width", Range(0, 2)) = 0.25
        _DissolveLineColor("Line Tint", Color) = (1,1,1,1)  

		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
        _BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}

		_EmissionAmount("Emission amount", float) = 2.0

		[MaterialToggle] _DissolveRoofBool("Can Dissolve Roof?", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off

        CGPROGRAM
        #pragma surface surf Toon addshadow
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondTex;
		sampler2D _ToonRamp;
		fixed4 _Color;
		fixed4 _Color2;

		half4 LightingToon (SurfaceOutput s, half3 lightDir, half atten)
		{
			half nDotL = dot(s.Normal, lightDir);
			half4 c;
			float ramp = clamp(nDotL, 0.0, 1.0);
			float3 lighting = tex2D(_ToonRamp, float2(ramp, 0.5)).rgb  * (atten);

			c.rgb = s.Albedo * lighting * _Color;
			c.a = s.Alpha;
			return c;
		}

		uniform float3 GLOBALMASK_Position; // from script
		uniform float GLOBALMASK_Radius;

		sampler2D _DissolveTex;
		float _DissolveAmount;
		float _DissolveScale;
		float _DissolveLineWidth;
		sampler2D _BurnRamp;
        fixed4 _BurnColor;
        float _BurnSize;
        float _EmissionAmount;

		float _DissolveRoofBool;
		float _RoofDissolveAmount;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;// built in value to use the world space position
			float3 worldNormal; // built in value for world normal
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 c2 = tex2D (_SecondTex, IN.uv_MainTex) * _Color2;
 
			// triplanar noise
			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));
			half4 nSide1 = tex2D(_DissolveTex, (IN.worldPos.xy + _Time.x) * _DissolveScale);
			half4 nSide2 = tex2D(_DissolveTex, (IN.worldPos.xz + _Time.x) * _DissolveScale);
			half4 nTop = tex2D(_DissolveTex, (IN.worldPos.yz + _Time.x) * _DissolveScale);
 
			float3 noisetexture = nSide1;
			noisetexture = lerp(noisetexture, nTop, blendNormal.x);
			noisetexture = lerp(noisetexture, nSide2, blendNormal.y);
 
			// distance influencer position to world position
			float3 dis = distance(GLOBALMASK_Position, IN.worldPos);
			float3 sphere = 1 - saturate(dis / GLOBALMASK_Radius);
 
			float3 sphereNoise = noisetexture.r * sphere;
 
			float3 DissolveLine = step(sphereNoise - _DissolveLineWidth, _DissolveAmount) * step(_DissolveAmount,sphereNoise) ; // line between two textures
			DissolveLine *= (c2 + 0.2); // color the line
   
			float3 primaryTex = (step(sphereNoise - -_DissolveLineWidth, _DissolveAmount) * c.rgb);
			float3 secondaryTex = (step(_DissolveAmount, sphereNoise) * c2.rgb);
			float3 resultTex = primaryTex + secondaryTex + DissolveLine;

			float3 roofPlayerDistance = distance(GLOBALMASK_Position, IN.worldPos);
			float3 roofDissolveAmount = 1 - (saturate(roofPlayerDistance / 35.0));

			if(_DissolveRoofBool >= 0.5)
			{
				half test = tex2D(_DissolveTex, IN.uv_MainTex).rgb - (10.0 * roofDissolveAmount) * _RoofDissolveAmount;
				clip(test);
			}

            o.Albedo = resultTex;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
