float4x4 World;
float4x4 View;
float4x4 Projection;
sampler s0;
texture lightMask;
sampler lightSampler = sampler_state{Texture = lightMask;};
float2 pos;
float2 pos2;
float intensity;

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

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0  
{  
    float4 color = tex2D(s0, coords);  
    float4 lightColor = tex2D(lightSampler, coords);
	float modifier;
	if(pos.x != 0)
	{
		modifier = intensity / (abs(sqrt(((coords.x - pos.x)*(coords.x - pos.x))+((coords.y - pos.y)*(coords.y - pos.y)))));
		//if(abs(sqrt(((coords.x - pos.x)*(coords.x - pos.x))+((coords.y - pos.y)*(coords.y - pos.y))))<0.2)
		lightColor += modifier;
	}
	float modifier2;
	
	modifier2 = 0.0075 / (abs(sqrt(((coords.x - pos2.x)*(coords.x - pos2.x))+((coords.y - pos2.y)*(coords.y - pos2.y)))));
	lightColor += modifier2;

	color =  color * lightColor;
    return color;
} 

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        //VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
