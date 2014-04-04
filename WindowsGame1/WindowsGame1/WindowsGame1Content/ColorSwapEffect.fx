float4x4 World;
float4x4 View;
float4x4 Projection;




sampler sprite : register(s0);

/*
 * Convert from BGRX to RGBA
 */
float4 BGR2RGB(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(sprite, texCoord);
    return float4(tex.b, tex.g, tex.r, 1.0);
}

technique KinectVideo
{
    pass KinectVideo
    {
        PixelShader = compile ps_2_0 BGR2RGB();
    }
}






// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.

    return float4(1, 0, 0, 1);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
