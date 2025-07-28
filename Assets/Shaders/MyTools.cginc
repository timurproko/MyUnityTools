/// CONST
#define PI  3.14159265358979323846
#define TAU 6.28318530717958647692

/// PROPERTIES
// [Header(Category name)][Space(10)]

// _Float ("Float", Float) = 0
// _Integer ("Integer", Integer) = 0
// _Range ("Range", Range(0, 1)) = 0
// _Color ("Color", Color) = (1, 1, 1, 1)
// _Vector ("Vector", Vector) = (0, 0, 0, 0)

// _Texture2D ("Texture 2D", 2D) = "white" {} 
// _Texture3D ("Texture 3D", 3D) = "white" {}
// _CubeMap ("CubeMap Texture", 3D) = "black" {}

// [Toggle] _Boolean ("Boolean", Float) = 0
// [IntRange] _IntRange ("Range", Range(0, 255)) = 0
// [KeywordEnum(Option1, Option2, Option3)] _Enum ("Enum", Integer) = 0
// [PowerSlider(0.5)] _Slider("Slider", Range (0, 1)) = 0
// [MainTexture] _MainTex ("Main Texture", 2D) = "white" {}
// [NormalMap] _NormalMap ("Normal Map", 2D) = "bump" {}

/// MESH ATTRIBUTES
// float4 positionOS : POSITION;       // Vertex position in object space
// float3 normal : NORMAL;             // Vertex normal
// float4 tangent : TANGENT;           // Vertex tangent
// float4 color : COLOR;               // Vertex color (if present)
// float2 uv : TEXCOORD0;              // First set of UV coordinates
// float2 uv1 : TEXCOORD1;             // Second set of UV coordinates

/// SUB-SHADER TAGS
// "RenderPipeline" = "UniversalPipeline"

/// PASS TAGS
/// LIGHTING
// "LightMode" = "UniversalForward"

/// CULLING
// Cull Off
// Cull Back
// Cull Front

/// DEPTH BUFFER
// ZWrite On
// ZWrite Off

// ZTest LEqual
// ZTest Always
// ZTest GEqual

/// MODES
// Transparency                 Blend SrcAlpha OneMinusSrcAlpha
// Transparency Pre-Multiplied  Blend One OneMinusSrcAlpha

// Darken                       Blend OneMinusDstColor Zero
// Multiply                     Blend DstColor Zero
// Multiply 2X                  Blend DstColor SrcColor
// Color Burn                   Blend DstColor OneMinusSrcColor
// Linear Burn                  Blend DstColor OneMinusSrcAlpha

// Lighten                      Blend One OneMinusSrcColor
// Screen                       Blend OneMinusDstColor One
// Color Dodge                  Blend One DstColor
// Add                          Blend One One
// Lighten Color                Blend SrcColor DstColor

// Overlay                      Blend SrcAlpha DstAlpha
// Soft Light                   Blend OneMinusSrcColor One
// Hard Light                   Blend One SrcAlpha
// Vivid Light                  Blend SrcAlpha OneMinusDstAlpha
// Linear Light                 Blend SrcAlpha One
// Pin Light                    Blend DstAlpha OneMinusSrcAlpha

// Difference                   Blend OneMinusDstColor OneMinusSrcColor
// Exclusion                    Blend OneMinusDstColor SrcColor

// Subtract                     Blend Zero OneMinusSrcColor
// Divide                       Blend SrcColor OneMinusDstColor

// Hue                          Blend SrcColor One
// Saturation                   Blend SrcAlpha DstColor
// Color                        Blend SrcColor DstAlpha
// Luminosity                   Blend DstColor SrcAlpha

// Reflect                      Blend DstColor DstColor
// Contrast Negate              Blend OneMinusDstColor SrcAlpha

// Erase                        Blend Zero Zero

/// CUSTOM FUNCTIONS
// INVERSE LERP
float InverseLerp(float a, float b, float v)
{
    return (v - a) / (b - a);
}

float2 InverseLerp(float2 a, float2 b, float2 v)
{
    return (v - a) / (b - a);
}

float3 InverseLerp(float3 a, float3 b, float3 v)
{
    return (v - a) / (b - a);
}

float4 InverseLerp(float4 a, float4 b, float4 v)
{
    return (v - a) / (b - a);
}

// CLAMP01
float Clamp01(float value)
{
    return max(0, min(1, value));
}

float2 Clamp01(float2 value)
{
    return max(0, min(1, value));
}

float3 Clamp01(float3 value)
{
    return max(0, min(1, value));
}

float4 Clamp01(float4 value)
{
    return max(0, min(1, value));
}

// CLAMP
float Clamp(float value, float minVal, float maxVal)
{
    return max(minVal, min(maxVal, value));
}

float2 Clamp(float2 value, float2 minVal, float2 maxVal)
{
    return max(minVal, min(maxVal, value));
}

float3 Clamp(float3 value, float3 minVal, float3 maxVal)
{
    return max(minVal, min(maxVal, value));
}

float4 Clamp(float4 value, float4 minVal, float4 maxVal)
{
    return max(minVal, min(maxVal, value));
}

// FIT
float Fit(float value, float newMin, float newMax, float oldMin = 0, float oldMax = 1)
{
    return newMin + (value - oldMin) / (oldMax - oldMin) * (newMax - newMin);
}

float2 Fit(float2 value, float2 newMin, float2 newMax, float2 oldMin = 0, float2 oldMax = 1)
{
    return newMin + (value - oldMin) / (oldMax - oldMin) * (newMax - newMin);
}

float3 Fit(float3 value, float3 newMin, float3 newMax, float3 oldMin = 0, float3 oldMax = 1)
{
    return newMin + (value - oldMin) / (oldMax - oldMin) * (newMax - newMin);
}

float4 Fit(float4 value, float4 newMin, float4 newMax, float4 oldMin = 0, float4 oldMax = 1)
{
    return newMin + (value - oldMin) / (oldMax - oldMin) * (newMax - newMin);
}

// GENERATORS
float GetWave(float2 uv, float speed = 0.1, float freq = 1, float amp = 1, float offset = 0)
{
    float2 uvCenter = Fit(uv, -1, 1);
    float radialDist = length(uvCenter);
    float wave = cos((radialDist - _Time.y * speed) * TAU * freq) * amp + offset;
    wave *= 1 - radialDist;
    return wave;
}
