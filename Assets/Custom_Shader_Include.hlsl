//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float hash21(float2 x){return frac(sin(dot(x, float2(12.9898, 4.1414))) * 43758.5453);}
float noise21(float2 uv){
	float2 i = floor(uv);
    float2 f = frac(uv); f = f*f*(3.0-2.0*f);
	return lerp(lerp(
		hash21(i),
		hash21(i + float2(1,0)), f.x), lerp(
		hash21(i + float2(0,1)),
		hash21(i + float2(1,1)), f.x), f.y);
}
float fbm(float2 uv, int octaves){
    float2x2 rotation = float2x2(0.9,-0.4,1.1,0.8);
    float amplitude=0.5; float total = 0.;
	for(int i = 0; i < octaves; ++i){
		uv = mul(uv, rotation);
		total += amplitude * abs(noise21(uv * 3.)*2.0-1.0);
		amplitude *= 0.5;
		uv *= 2.0;
	}
	return total;
}

void LavaTextureSample_float(
    in float2 uv,
    in float time,
    out float height
){
	float basis = fbm(uv*.7+time, 3);
	height = fbm(uv - (basis-.5)*.2, 3);
}

#endif