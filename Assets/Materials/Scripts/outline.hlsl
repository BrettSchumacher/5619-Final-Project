TEXTURE2D(_CameraColorTexture);
SAMPLER(sampler_CameraColorTexture);
float4 _CameraColorTexture_TexelSize;

TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);

TEXTURE2D(_CameraDepthNormalsTexture);
SAMPLER(sampler_CameraDepthNormalsTexture);
 
float3 DecodeNormal(float4 enc)
{
    float kScale = 1.7777;
    float3 nn = enc.xyz * float3(2 * kScale, 2 * kScale, 0) + float3(-kScale, -kScale, 1);
    float g = 2.0 / dot(nn.xyz, nn.xyz);
    float3 n;
    n.xy = g * nn.xy;
    n.z = g - 1;
    return n;
}

void Outline_float(float2 UV, float OutlineThickness, float DepthSensitivity, float NormalsSensitivity, float ColorSensitivity, float4 OutlineColor, out float4 Out)
{
    float halfScaleFloor = floor(OutlineThickness * 0.5);
    float halfScaleCeil = ceil(OutlineThickness * 0.5);
    float sampleOff = OutlineThickness * 0.5f;
    float2 Texel = (1.00) / float2(_CameraColorTexture_TexelSize.z, _CameraColorTexture_TexelSize.w);

    float2 uvSamples[5];
    float depthSamples[5];
    float3 normalSamples[5], colorSamples[5];

    uvSamples[0] = UV;
    uvSamples[1] = UV + float2(sampleOff * Texel.x, 0);
    uvSamples[2] = UV - float2(sampleOff * Texel.x, 0);
    uvSamples[3] = UV + float2(0, sampleOff * Texel.y);
    uvSamples[4] = UV - float2(0, sampleOff * Texel.y);

    for (int i = 0; i < 5; i++)
    {
        depthSamples[i] = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uvSamples[i]).r;
        normalSamples[i] = DecodeNormal(SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uvSamples[i]));
        colorSamples[i] = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uvSamples[i]);
    }

    // Depth
    float depthFiniteDifference0 = depthSamples[2] + depthSamples[1] - 2.0f * depthSamples[0];
    float depthFiniteDifference1 = depthSamples[3] + depthSamples[4] - 2.0f * depthSamples[0];
    float edgeDepth = max(abs(depthFiniteDifference0), abs(depthFiniteDifference1)) * 1000;
    edgeDepth = edgeDepth > (1 / DepthSensitivity) ? 1 : 0;

    // Normals
    float3 normalFiniteDifference0 = normalSamples[2] + normalSamples[1] - 2.0f * normalSamples[0];
    float3 normalFiniteDifference1 = normalSamples[3] + normalSamples[4] - 2.0f * normalSamples[0];
    float edgeNormal = max(length(normalFiniteDifference0), length(normalFiniteDifference1));
    edgeNormal = edgeNormal > (1 / NormalsSensitivity) ? 1 : 0;

    // Color
    float3 colorFiniteDifference0 = colorSamples[2] + colorSamples[1] - 2.0f * colorSamples[0];
    float3 colorFiniteDifference1 = colorSamples[3] + colorSamples[4] - 2.0f * colorSamples[0];
    float edgeColor = max(length(colorFiniteDifference0), length(colorFiniteDifference1));
    edgeColor = edgeColor > (1 / ColorSensitivity) ? 1 : 0;

    float edge = max(edgeDepth, max(edgeNormal, edgeColor));

    float4 original = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uvSamples[0]);
    Out = ((1 - edge) * original) + (edge * lerp(original, OutlineColor, OutlineColor.a));
}