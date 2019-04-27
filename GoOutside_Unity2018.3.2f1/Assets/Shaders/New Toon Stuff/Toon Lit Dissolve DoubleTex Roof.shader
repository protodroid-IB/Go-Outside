Shader "Toon/Lit Dissolve DoubleTex Roof" {
    Properties {
        _Color ("Primary Color", Color) = (0.6901961,0.6901961,0.6901961,1)
        _MainTex ("Primary (RGB)", 2D) = "white" {}
        _Color2 ("Secondary Color", Color) = (0.6901961,0.6901961,0.6901961,1)
        _SecondTex ("Secondary (RGB)", 2D) = "white" {}
        _Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
        _NoiseTex("Dissolve Noise", 2D) = "white"{}
        _NScale ("Noise Scale", Range(0, 10)) = 0.05
        _DisAmount("Noise Texture Opacity", Range(0.01, 1)) = 0.01
        _DisLineWidth("Line Width", Range(0, 2)) = 0.005
        _DisLineColor("Line Tint", Color) = (1,1,1,1)  
		_DissolveRoofBool("DissolveBool", Range(0.0, 1.0)) = 1.0
		_RoofDissolveAmount("RoofDissolveAmount", Range(0.0, 1.0)) = 0.0
    }
 
        SubShader{
            Tags { "RenderType" = "Transparent" }
            LOD 200            
       
CGPROGRAM
 
#pragma surface surf ToonRamp
sampler2D _Ramp;
 
// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
    #ifndef USING_DIRECTIONAL_LIGHT
    lightDir = normalize(lightDir);
    #endif
   
    half d = dot (s.Normal, lightDir)*0.5 + 0.5;
    half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
    half4 c;
    c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
    c.a = 0;
    return c;
}
 
 
 float3 GLOBALMASK_Position; // from script
 float GLOBALMASK_Radius; // from script
 
sampler2D _MainTex, _SecondTex;
float4 _Color, _Color2;
sampler2D _NoiseTex;
float _DisAmount, _NScale;
float _DisLineWidth;
float4 _DisLineColor;

float _DissolveRoofBool;
float _RoofDissolveAmount;

 
 
struct Input {
    float2 uv_MainTex : TEXCOORD0;
    float3 worldPos;// built in value to use the world space position
    float3 worldNormal; // built in value for world normal
   
};
 
void surf (Input IN, inout SurfaceOutput o) {
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
 
    float3 DissolveLine = step(sphere - _DisLineWidth, _DisAmount) * step(_DisAmount,sphere) ; // line between two textures
    DissolveLine *= _DisLineColor; // color the line
   
    float3 primaryTex = (step(sphere - _DisLineWidth,_DisAmount) * c.rgb);
    float3 secondaryTex = (step(_DisAmount, sphere) * c2.rgb);
    float3 resultTex = primaryTex + secondaryTex + DissolveLine;
    o.Albedo = resultTex;

	float3 roofPlayerDistance = distance(GLOBALMASK_Position, IN.worldPos);
	float3 roofDissolveAmount = 1 - (saturate(roofPlayerDistance / 35.0));

	half test = (tex2D(_NoiseTex, IN.uv_MainTex).rgb - (10.0 * roofDissolveAmount) * _RoofDissolveAmount) * (int)_DissolveRoofBool;
	clip(test);
 
    o.Emission = DissolveLine;
    o.Alpha = c.a;
   
}
ENDCG
 
    }
 
    Fallback "Diffuse"
}