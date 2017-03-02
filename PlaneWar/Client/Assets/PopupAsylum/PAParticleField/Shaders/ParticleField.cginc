// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

#ifndef TURBULENCE_ANY
#if defined(TURBULENCE_SIMPLEX2D) || defined(TURBULENCE_SIMPLEX) || defined(TURBULENCE_SIN)
#define TURBULENCE_ANY
#endif
#endif

#ifndef SHADER_MODEL_2
#if SHADER_TARGET < 30
#define SHADER_MODEL_2
#endif
#endif

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/PopupAsylum/PAParticleField/Shaders/Turbulence/noise2D.cginc"
#include "Assets/PopupAsylum/PAParticleField/Shaders/Turbulence/noise3D.cginc"

fixed _IsUnserialized;

//Floats as Bools
fixed _Editor;
fixed _UserFacing;
fixed _EdgeAlpha;
fixed _EdgeScale;

//Particle params
float2 _ParticleSize;
float3 _FieldSize;
float3 _Speed;
float _SpinSpeed;
float3 _Force;
float _SpeedScale;
float _UOffset;
fixed3 _EdgeThreshold;
float3 _FaceDirection;
float3 _UpDirection;

float _TotalTime;
float3 _DeltaSpeed;
float3 _DeltaForce;
float3 _DeltaPosition;

//Soft particle params
float _NearFadeDistance;
float _NearFadeOffset;

//Exclusion Zones
#ifdef EXCLUSION_ON
float4x4 _ExclusionMatrix0;
float4x4 _ExclusionMatrix1;
float4x4 _ExclusionMatrix2;
fixed3 _ExclusionThreshold0;
fixed3 _ExclusionThreshold1;
fixed3 _ExclusionThreshold2;
#endif

//Turbulence
fixed _TurbulenceInput;
float3 _TurbulenceOffset;
float _TurbulenceFrequency;
float _TurbulenceAmplitude;
float3 _TurbulenceDeltaOffset;



float3 Noise(float3 input){

#ifdef TURBULENCE_ANY
	#if defined(SHADER_MODEL_2) || defined(TURBULENCE_SIN)
		//on shader model 2 fallback to some Sin based pseudo noise
	    input.x*=12;
		input.y*=19;
		input.z*=7;
		return sin((input + input.yzx * 0.2)+float3(1.23,4.56,7.89))*2.5;
	#endif
	#if defined(TURBULENCE_SIMPLEX2D) || defined(SHADER_API_MOBILE)
		return snoised2D(input);
	#else	
		return snoised3D(input);
	#endif
#endif
	return 0;
}

float3 ApplyTurbulence(float3 input, float3 deltaInput, inout float3 position, inout float3 speed)
{
	float3 positionOffset = Noise(input * _TurbulenceFrequency) * _TurbulenceAmplitude;
	position += positionOffset;

#if DIRECTIONAL_ON
	float3 deltaOffset = Noise(deltaInput * _TurbulenceFrequency) * _TurbulenceAmplitude;
	speed += positionOffset- deltaOffset;
#endif

	return positionOffset;
}

float4 AxisAngleToQuaternion(float3 axis, float angle)
{ 
	float4 qr;
	float half_angle = angle;//(angle * 0.5) * 3.14159 / 180.0;
	qr.x = axis.x * sin(half_angle);
	qr.y = axis.y * sin(half_angle);
	qr.z = axis.z * sin(half_angle);
	qr.w = cos(half_angle);
	return qr;
}

float3 Scale(float3 a, float3 b)
{
a.x *= b.x;
a.y *= b.y;
a.z *= b.z;
return a;
}

float3 Divide(float3 a, float3 b)
{
a.x /= b.x;
a.y /= b.y;
a.z /= b.z;
return a;
}

float3 UnpackSpiralVector(float t)
{
	return float3(cos(t) * sin(2048 * t), sin(t) * sin(2048 * t), cos(t));
}

fixed ApplyExclusion(float3 pos){

#ifdef EXCLUSION_ON
	fixed finalEAlpha = 1;

	float4 worldPos = float4(pos, 1);
	
	float3 exclusionPos = abs(mul(_ExclusionMatrix0, worldPos));
	fixed ealpha = 1;
	exclusionPos -= _ExclusionThreshold0;
	ealpha = min(1 - (exclusionPos.x)/(1-_ExclusionThreshold0.x), ealpha);
	ealpha = min(1 - (exclusionPos.y)/(1-_ExclusionThreshold0.y), ealpha);
	ealpha = min(1 - (exclusionPos.z)/(1-_ExclusionThreshold0.z), ealpha);
	
	finalEAlpha = 1 - clamp(ealpha, 0, 1);

	exclusionPos = abs(mul(_ExclusionMatrix1, worldPos));
	ealpha = 1;	
	exclusionPos -= _ExclusionThreshold1;
	ealpha = min(1 - (exclusionPos.x - _ExclusionThreshold1.x)/(1-_ExclusionThreshold1.x), ealpha);
	ealpha = min(1 - (exclusionPos.y - _ExclusionThreshold1.y)/(1-_ExclusionThreshold1.y), ealpha);
	ealpha = min(1 - (exclusionPos.z - _ExclusionThreshold1.z)/(1-_ExclusionThreshold1.z), ealpha);
	
	finalEAlpha = min(1 - clamp(ealpha, 0, 1), finalEAlpha);
	
	exclusionPos = abs(mul(_ExclusionMatrix2, worldPos));
	ealpha = 1;
	ealpha = min(1 - (exclusionPos.x - _ExclusionThreshold2.x)/(1-_ExclusionThreshold2.x), ealpha);
	ealpha = min(1 - (exclusionPos.y - _ExclusionThreshold2.y)/(1-_ExclusionThreshold2.y), ealpha);
	ealpha = min(1 - (exclusionPos.z - _ExclusionThreshold2.z)/(1-_ExclusionThreshold2.z), ealpha);
	
	return min(1 - clamp(ealpha, 0, 1), finalEAlpha);
#else
	return 1;
#endif
}

struct v2fParticleField {
	float4 pos : SV_POSITION;
	float4 worldPos : TEXCOORD0;
	float2 tex : TEXCOORD1;
	fixed4 color : COLOR;
	
	#ifdef SOFTPARTICLES_ON
	float4 projPos : TEXCOORD4;
	#endif	
	
	LIGHTING_COORDS(2,3)
};

/// PAParticleField
/// Method modifys the input data and outputs the position of the pivot of the particle
float3 PAParticleField(inout appdata_full v){
	//in play mode the speed and force are absolute distances
	//in the editor speed and force are deltas
	float time = lerp(1, _Time.y, _Editor);
	float realtime = lerp(_Time.y, 1, _Editor);
	float totalTime = lerp(_TotalTime, _Time.y, _Editor);
	
	//Get the position of the pivot
	float3 objPos = mul (unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
	//Get the position of the vertex
	float3 worldPos = v.vertex.xyz;
	
	//get the speed vector
	float3 speed = normalize(v.vertex.xyz) * v.normal.r;
    float3 realspeed = Scale(speed, _DeltaSpeed) + _DeltaForce;
    speed = Scale(speed, _Speed.xyz) + _Force;
	
	//Move the vertex along its normal by time
	worldPos += speed * time;
	
#ifdef WORLDSPACE_ON
	float3 adjPos = Divide(objPos, _FieldSize.xyz);
	worldPos += adjPos;	
#else
	worldPos += _DeltaPosition;
#endif
		
	//Wrap the vertex position in the bounds
	worldPos.x = (0.5-frac(worldPos.x));
	worldPos.y = (0.5-frac(worldPos.y)); 
	worldPos.z = (0.5-frac(worldPos.z)); 
	
	//Set the vertex alpha based on distance from the center
	fixed3 absPos = abs(worldPos)/0.5;
	fixed alpha = 1;
	
#ifdef SHAPE_SPHERE
	alpha = min(1-(length(absPos) - _EdgeThreshold.x)/(1-_EdgeThreshold.x), alpha);
#elif SHAPE_CYLINDER
	alpha = min(1-(length(absPos.xz) - _EdgeThreshold.x)/(1-_EdgeThreshold.x), alpha);
	alpha = min(1 - (absPos.y - _EdgeThreshold.y)/(1-_EdgeThreshold.y), alpha);
#else
	alpha = min(1 - (absPos.x - _EdgeThreshold.x)/(1-_EdgeThreshold.x), alpha);
	alpha = min(1 - (absPos.y - _EdgeThreshold.y)/(1-_EdgeThreshold.y), alpha);
	alpha = min(1 - (absPos.z - _EdgeThreshold.z)/(1-_EdgeThreshold.z), alpha);	
#endif

	alpha = clamp(alpha, 0, 1);
	
	//Scale the position
	worldPos = Scale(worldPos, _FieldSize.xyz);
	
#ifdef WORLDSPACE_ON
	//Center the field on the objects pivot
	worldPos += objPos;
#endif

	float3 noiseInput = lerp(worldPos, v.vertex.xyz + v.vertex.xyz * totalTime, _TurbulenceInput) - (_TurbulenceOffset * time);
	float3 deltaNoiseInput = lerp(worldPos - realspeed, v.vertex.xyz + v.vertex.xyz * (totalTime + 0.5f), _TurbulenceInput) - (_TurbulenceOffset * time + _TurbulenceDeltaOffset/5);
	ApplyTurbulence(noiseInput, deltaNoiseInput, /*out*/ worldPos, /*out*/ realspeed);

#ifdef EXCLUSION_ON	
	alpha *= ApplyExclusion(worldPos);
#endif
	
#ifdef WORLDSPACE_ON
	float3 facing = normalize(_WorldSpaceCameraPos - worldPos);
#else
	float3 facing = normalize(mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1)) - worldPos);
#endif
	facing = lerp(facing, _FaceDirection, _UserFacing);
	
	float3 worldUp = _UpDirection;
#ifdef DIRECTIONAL_ON
	worldUp = realspeed;
	worldUp = Scale(worldUp, _FieldSize);
	worldUp = normalize(worldUp);
#endif
	
	float3 right = (cross(facing, worldUp));
	
#ifdef SPIN_ON
	float4 q = AxisAngleToQuaternion(facing, time * _SpinSpeed * v.normal.g);
	right = right + 2.0 * cross(q.xyz, cross(q.xyz, right) + q.w * right);
#endif
	
#ifdef DIRECTIONAL_ON
	worldUp *= (length(realspeed)) * _SpeedScale;
	right *= -1;
#else
	worldUp = (cross(facing, right));	
#endif

	//TODO check this code is required in all circumstances
	float minSize = min(_ParticleSize.x, _ParticleSize.y);
	float rightSize = max(length(right), minSize/_ParticleSize.x);
	float upSize = max(length(worldUp), minSize/_ParticleSize.y); 
	
	worldUp = normalize(worldUp) * upSize;
	right = normalize(right) * rightSize;
	
	//Scale by the alpha if _EdgeScale is enabled, scale particles to zero that are out of bounds
	fixed scaleByAlpha = lerp(1, alpha, _EdgeScale) * ceil(alpha);

	right *= scaleByAlpha;
	worldUp *= scaleByAlpha;
	
	//Finalize values
	v.vertex.xyz = worldPos + (right * _ParticleSize.x * (v.texcoord1.x - 0.5)) + (worldUp * _ParticleSize.y * (-v.texcoord1.y + 0.5));
	v.normal.xyz = facing;
	v.tangent.xyz = worldUp;
	v.tangent.w = 1;
	v.texcoord.x = v.texcoord.x + _UOffset;
	
	//Apply alpha if _EdgeAlpha is 1
	v.color.a *= lerp(1, alpha, _EdgeAlpha);

	return worldPos;
}

float4 PAPositionVertex(float3 position){
#ifdef WORLDSPACE_ON
	//Position the vertex in view
	return mul(UNITY_MATRIX_VP, float4(position, 1));
#else
	//Position the vertex in view
	return mul(UNITY_MATRIX_MVP, float4(position, 1));
#endif
}

float4 PAPositionVertexSurf(float3 position){
#ifdef WORLDSPACE_ON
	return mul(unity_WorldToObject, float4(position, 1));
#else
	return float4(position, 1);
#endif
}

v2fParticleField vertParticleFieldCube(appdata_full v) 
{
	v2fParticleField o;	
	UNITY_INITIALIZE_OUTPUT(v2fParticleField, o);
	
	PAParticleField(v);
	o.color = v.color;
	o.worldPos = float4(v.vertex.xyz, 1);
	o.tex = v.texcoord;

#ifndef WORLDSPACE_ON
	TRANSFER_VERTEX_TO_FRAGMENT(o);
#endif
	v.vertex = mul(unity_WorldToObject, o.worldPos);
	
#ifdef WORLDSPACE_ON
	TRANSFER_VERTEX_TO_FRAGMENT(o);
#endif
	o.pos = PAPositionVertex(o.worldPos);
	
#ifdef SOFTPARTICLES_ON
	o.projPos = ComputeScreenPos (o.pos);
#endif	

	//Clip safe
	o.color.a *= clamp(o.pos.z * _NearFadeDistance -_NearFadeOffset, 0, 1);

	return o;
}