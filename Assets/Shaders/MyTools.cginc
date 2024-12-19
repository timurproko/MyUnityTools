/// CONST
#define TAU 6.28318530718
#define PI  3.14159265358

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
float InverseLerp(float a, float b, float v)
{
    return (v - a) / (b - a);
}

float Clamp01(float value)
{
    return max(0, min(1, value));
}

float Clamp(float value, float minVal, float maxVal)
{
    return max(minVal, min(maxVal, value));
}
