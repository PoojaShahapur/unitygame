// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

float3 UnpackVector( float v ) {		
     return frac(v / float3(16777216, 65536, 256)) * 2 - 1;
}

void OrthoNormalize(inout float3 forward, inout float3 up, inout float3 right){
	forward = normalize(forward);
	right = normalize(cross(forward,up));
	up = cross(right,forward);
}

float3x3 LookMatrix(float3 forward, float3 up){	
	float3 right = float3(1,0,0);
	OrthoNormalize(forward, up, right);
	
	float3x3 rotMat = float3x3(right.x, right.y, right.z,
		up.x, up.y, up.z,
		-forward.x, -forward.y, -forward.z);

	return rotMat;
}

float3 PAGetMeshNormal(appdata_full v){
	return UnpackVector(v.normal.x);
}

float3 PAParticleMeshField(inout appdata_full v){
	//in play mode the speed and force are absolute distances
	//in the editor speed and force are deltas
	float time = lerp(1, _Time.y, _Editor);
	float realtime = lerp(_Time.y, 1, _Editor);
	float totalTime = lerp(_TotalTime, _Time.y, _Editor);
	
	//Get the position of the pivot
	float3 objPos = mul (unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
	//Get the position of the vertex
	float3 worldPos = UnpackVector(v.normal.y);
	
	//get the speed vector
	float3 speed = normalize(worldPos) * v.texcoord1.x;
    float3 realspeed = Scale(speed, _DeltaSpeed) + _DeltaForce; // + rand for sin based noise
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
	
//	worldPos += rand;
	
#ifdef WORLDSPACE_ON
	//Center the field on the objects pivot
	worldPos += objPos;
#endif

	float3 noiseInput = worldPos - (_TurbulenceOffset * time);
	float3 deltaNoiseInput = worldPos - realspeed - (_TurbulenceOffset * time + _TurbulenceDeltaOffset/5);
	ApplyTurbulence(noiseInput, deltaNoiseInput, /*out*/ worldPos, /*out*/ realspeed);

#ifdef EXCLUSION_ON	
	float finalEAlpha = ApplyExclusion(worldPos);	
	alpha *= finalEAlpha;
#endif
	
	float3 vertPos = v.vertex.xyz;
	float3 vertNorm = UnpackVector(v.normal.x);
	
#ifdef DIRECTIONAL_ON
	float3x3 rotMat = LookMatrix(realspeed, _UpDirection);	
	vertPos = mul(vertPos, rotMat);
	vertNorm = mul(vertNorm, rotMat);

#elif SPIN_ON
	float4 q = AxisAngleToQuaternion(UnpackVector(v.normal.z), v.texcoord1.y + time * _SpinSpeed * v.texcoord1.y);//UnpackVector(v.normal.z)
	
	vertPos = vertPos + 2.0 * cross(q.xyz, cross(q.xyz, vertPos) + q.w * vertPos);
	vertNorm = vertNorm + 2.0 * cross(q.xyz, cross(q.xyz, vertNorm) + q.w * vertNorm);
#else
	float3x3 rotMat = LookMatrix(-_FaceDirection, _UpDirection);	
	vertPos = mul(vertPos, rotMat);
	vertNorm = mul(vertNorm, rotMat);
#endif

	fixed scaleByAlpha = lerp(1, alpha, _EdgeScale) * ceil(alpha);

	vertPos *= scaleByAlpha;
	
	//Finalize values
	v.vertex.xyz = worldPos + vertPos * _ParticleSize.x;
#ifdef WORLDSPACE_ON
	v.normal.xyz = mul(unity_WorldToObject, float4(vertNorm, 0));
#else
	v.normal.xyz = vertNorm;
#endif
	
	v.color.a *= lerp(1, alpha, _EdgeAlpha);

	v.texcoord.x = v.texcoord.x + _UOffset;

	return worldPos;
}