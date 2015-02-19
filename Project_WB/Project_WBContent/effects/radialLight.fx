texture lightMask;
sampler mainSampler : register(s0);
sampler lightSampler = sampler_state {
	Texture = lightMask;

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;

	AddressU = Clamp;
	AddressV = Clamp;
};

struct PsIn {
	float4 TexCoord : TEXCOORD0;
};

float4 PsRadLight(PsIn input) : COLOR0 {
	float2 coord = input.TexCoord;

	float4 mainColor = tex2D(mainSampler, coord);
	float4 lightColor = tex2D(lightSampler, coord);

	return mainColor * lightColor;
}

technique Technique1 {
	pass Pass1 {
		PixelShader = compile ps_2_0 PsRadLight();
	}
}
