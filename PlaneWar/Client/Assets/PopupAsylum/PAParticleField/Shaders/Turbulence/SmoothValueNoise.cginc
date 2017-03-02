

float3 Rand(float3 co){
	float r = sin(co.x*4.5577 + co.y+16.782 + co.z*3.452);
	return normalize(float3(frac(r), frac(r * 165.536), frac(r * 0.9656)));
}

float3 HalfTrilinear(float3 p00, float3 p01, float3 p02, float3 p10, float3 p11, float3 p12, float f, float3 p){
	//lerp between each corner pair to get a triangle
	float3 t0 = lerp(p00, p10, p.z);
	float3 t1 = lerp(p01, p11, p.z);
	float3 t2 = lerp(p02, p12, p.z);

	//in the triangle get barycentric coordinates of p
	float multiply = f * 2 - 1;
	float a = (-p.x - p.y + 1) * -multiply;
	float b = (f * p.x - f * p.y + f - p.x) * multiply;
	float c = 1-(a+b);

	//return the weighted blended value
    return (a * t0 + b * t1 + c * t2);
}

float3 Bilinear(float3 p00, float3 p01, float3 p10, float3 p11, float3 p){
	//lerp between each corner pair to get a triangle
	float3 t0 = lerp(p00, p10, p.x);
	float3 t1 = lerp(p01, p11, p.x);

	//return the weighted blended value
    return lerp(t0, t1, p.y);
}

float3 HalfSmoothValueNoise(float3 p)
{
	//get the corner of the cell p is in
	float3 corner = floor(p);
	
	//get p relative to the corner
	p -= corner;

	//get the closest corner to p on the xy axis, 0 is the cell corner, 1 is the next cell corner along xy
	float f = floor(p.x+p.y);
	
	//sample 6 pseudo random float3 values at the closest corner and the connected corners
	float3 p00 = Rand(corner + float3(f, f, 0));
	float3 p01 = Rand(corner + float3(1, 0, 0));
	float3 p02 = Rand(corner + float3(0, 1, 0));
	float3 p10 = Rand(corner + float3(f, f, 1));
	float3 p11 = Rand(corner + float3(1, 0, 1));
	float3 p12 = Rand(corner + float3(0, 1, 1));

	return HalfTrilinear(p00, p01, p02, p10, p11, p12, f, p);
}

float3 BilinearNoise(float3 p){
	//get the corner of the cell p is in
	float3 corner = floor(p);
	
	//get p relative to the corner
	p -= corner;

	//sample 6 pseudo random float3 values at the closest corner and the connected corners
	float3 p00 = Rand(corner);
	float3 p01 = Rand(corner + float3(0, 1, 0));
	float3 p10 = Rand(corner + float3(1, 1, 0));
	float3 p11 = Rand(corner + float3(1, 0, 0));

	float3 bi = Bilinear(p00, p01, p10, p11, p);

	return float3(bi.xy, sin(bi.y));
}

float3 DirectionalHalfSmoothValueNoise(float3 p, float3 direction){
	
	direction = normalize(direction) * 2;

	return lerp(HalfSmoothValueNoise(p + direction), HalfSmoothValueNoise(p - direction), 0.5);
	
//	//get the corner of the cell p is in
//	float3 corner = floor(p);
//	
//	//get p relative to the corner
//	p -= corner;
//
//	//get the closest corner to p on the xy axis, 0 is the cell corner, 1 is the next cell corner along xy
//	float f = floor(p.x+p.y);
//	
//	//sample 6 pseudo random float3 values at the closest corner and the connected corners
//	float3 p00 = Rand(corner + float3(f, f, 0));
//	float3 p01 = Rand(corner + float3(1, 0, 0));
//	float3 p02 = Rand(corner + float3(0, 1, 0));
//	float3 p10 = Rand(corner + float3(f, f, 1));
//	float3 p11 = Rand(corner + float3(1, 0, 1));
//	float3 p12 = Rand(corner + float3(0, 1, 1));
//
//	//float3 p0 = clamp(p - direction * 0.5f, 0, 1);
//	//float3 p1 = clamp(p + direction * 0.5f, 0, 1);
//
//	float3 p0 = p - direction * 0.5f;
//	float3 p1 = p + direction * 0.5f;
//
//	float3 halfTrilinear0 = HalfTrilinear(p00, p01, p02, p10, p11, p12, f, p0);
//	float3 halfTrilinear1 = HalfTrilinear(p00, p01, p02, p10, p11, p12, f, p1);
//
//	return (halfTrilinear1 - halfTrilinear0) * 10;
//	//return lerp(halfTrilinear0, halfTrilinear1, 0.5f);
}



float3 Trilinear(float3 v000, float3 v001, float3 v011, float3 v010, float3 v100, float3 v101, float3 v111, float3 v110, float x, float y, float z)
{
    float ix = 1-x;
    float iy = 1-y;
    float iz = 1-z;

    return v000 * ix * iy * iz +
        v100 * x * iy * iz +
        v010 * ix * y * iz +
        v001 * ix * iy * z +
        v101 * x * iy * z +
        v011 * ix * y * z +
        v110 * x * y * iz +
        v111 * x * y * z;
}


float3 SmoothValueNoise(float3 co){

	//Get the cell corners
	float3 f = floor(co);
	float3 c = f + float3(1,1,1);

	//get co relative to the corner
	co -= f;

	//triliear blend between 6 pseudo values at the corners
	return Trilinear(	Rand(float3(f.x, f.y, f.z)),
						Rand(float3(f.x, f.y, c.z)),
						Rand(float3(f.x, c.y, c.z)),
						Rand(float3(f.x, c.y, f.z)),
						Rand(float3(c.x, f.y, f.z)),
						Rand(float3(c.x, f.y, c.z)),
						Rand(float3(c.x, c.y, c.z)),
						Rand(float3(c.x, c.y, f.z)),
						co.x, co.y, co.z);
}

