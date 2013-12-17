Shader "underwaterObjectWithCaustics2"
{
    Properties 
    {
_color("_color", Color) = (0.7462686,1,0.9787078,1)
_texture("_texture", 2D) = "black" {}
_speed("_speed", Float) = 10
_alpha("_alpha", 2D) = "black" {}
_addColor("_addColor", Color) = (1,1,1,1)
_speedColor("_speedColor", Float) = 0
_speed2("_speed2", Float) = 0
_strength("_strength", Float) = 0
_UVsize("_UVsize", Float) = 0
_meshColor("_meshColor", 2D) = "black" {}
_meshAlpha("_meshAlpha", 2D) = "black" {}

    }

    SubShader 
    {
       Tags
       {
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="TransparentCutout"

       }


Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


       CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
#pragma target 3.0


float4 _color;
sampler2D _texture;
float _speed;
sampler2D _alpha;
float4 _addColor;
float _speedColor;
float _speed2;
float _strength;
float _UVsize;
sampler2D _meshColor;
sampler2D _meshAlpha;

         struct EditorSurfaceOutput {
          half3 Albedo;
          half3 Normal;
          half3 Emission;
          half3 Gloss;
          half Specular;
          half Alpha;
          half4 Custom;
         };

         inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
         {
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

         }

         inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
         {
          half3 h = normalize (lightDir + viewDir);

          half diff = max (0, dot ( lightDir, s.Normal ));

          float nh = max (0, dot (s.Normal, h));
          float spec = pow (nh, s.Specular*128.0);

          half4 res;
          res.rgb = _LightColor0.rgb * diff;
          res.w = spec * Luminance (_LightColor0.rgb);
          res *= atten * 2.0;

          return LightingBlinnPhongEditor_PrePass( s, res );
         }

         struct Input {
          float2 uv_meshColor;
float3 worldPos;
float2 uv_meshAlpha;

         };

         void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


         }


         void surf (Input IN, inout EditorSurfaceOutput o) {
          o.Normal = float3(0.0,0.0,1.0);
          o.Alpha = 1.0;
          o.Albedo = 0.0;
          o.Emission = 0.0;
          o.Gloss = 0.0;
          o.Specular = 0.0;
          o.Custom = 0.0;

float4 Sampled2D0=tex2D(_meshColor,IN.uv_meshColor.xy);
float4 Split0=float4( IN.worldPos.x, IN.worldPos.y,IN.worldPos.z,1.0 );
float4 Assemble0_2_NoInput = float4(0,0,0,0);
float4 Assemble0_3_NoInput = float4(0,0,0,0);
float4 Assemble0=float4(float4( Split0.x, Split0.x, Split0.x, Split0.x).x, float4( Split0.z, Split0.z, Split0.z, Split0.z).y, Assemble0_2_NoInput.z, Assemble0_3_NoInput.w);
float4 Multiply6=_UVsize.xxxx * Assemble0;
float4 Multiply1=_Time * _speed.xxxx;
float4 UV_Pan2=float4(Multiply6.x + Multiply1.y,Multiply6.y,Multiply6.z,Multiply6.w);
float4 Tex2D1=tex2D(_alpha,UV_Pan2.xy);
float4 Multiply3=_Time * _speed2.xxxx;
float4 UV_Pan1=float4(Multiply6.x + Multiply3.y,Multiply6.y,Multiply6.z,Multiply6.w);
float4 Tex2D2=tex2D(_alpha,UV_Pan1.xy);
float4 Multiply4=Tex2D1 * Tex2D2;
float4 Multiply5=Multiply4 * _strength.xxxx;
float4 Sampled2D1=tex2D(_meshAlpha,IN.uv_meshAlpha.xy);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
clip( Sampled2D1.aaaa );
o.Albedo = Sampled2D0;
o.Emission = Multiply5;
o.Custom = Sampled2D1.aaaa;
o.Alpha = Sampled2D1.aaaa;

          o.Normal = normalize(o.Normal);
         }
       ENDCG
    }
    Fallback "Transparent/Cutout/Diffuse"
}