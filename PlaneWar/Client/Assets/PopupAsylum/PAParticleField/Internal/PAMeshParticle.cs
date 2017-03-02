using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PAMeshParticle : PAParticleMeshGenerator {

	const int MAX_VERT_COUNT = 65536;

	public Mesh inputMesh;

	public override int GetMaximumParticleCount (){
		if (inputMesh) {
			return (int)(MAX_VERT_COUNT / (float)inputMesh.vertexCount);
		} else {
			return 16250;
		}
	}

    public override float GetParticleBaseSize()
    {
        if (inputMesh)
        {
            return Mathf.Max(inputMesh.bounds.size.x, Mathf.Max(inputMesh.bounds.size.y, inputMesh.bounds.size.z));
        }
        else
        {
            return base.GetParticleBaseSize();
        }
    }

	public override void UpdateMesh (Mesh mesh, PAParticleField settings)
	{
		inputMesh = settings.inputMesh;
		if (inputMesh) {
			base.UpdateMesh (mesh, settings);
		}
	}

	protected override int SetParticleCapacity (int count)
	{
		count = GetClampedParticleCount (count);

		int startAt = count;

		//if we're increasing start at the current count
		if (count * inputMesh.vertexCount > verts.Length) {
			startAt = verts.Length / inputMesh.vertexCount;
		}

		int vertCapactity = count * inputMesh.vertexCount;
		int triCapacity = count * inputMesh.triangles.Length;
		SetArraySizes (vertCapactity, triCapacity);

		return startAt;
	}

	protected override void UpdateDirection (PAParticleField settings, int startAt)
	{
		int count = GetClampedParticleCount (settings.particleCount);

		SkipRandomCalls (6, startAt);

		for (int i = startAt; i < count; i++) {

			float randomDirection = Vector3ToFloat(new Vector3(GetRandomAndIncrement(-1f, 1f), GetRandomAndIncrement(-1f,1f), GetRandomAndIncrement(-1f,1f)));
			float randomRotationAxis = Vector3ToFloat(new Vector3(GetRandomAndIncrement(-1f, 1f), GetRandomAndIncrement(-1f,1f), GetRandomAndIncrement(-1f,1f)).normalized);

			for (int j = 0; j < inputMesh.vertexCount; j++) {
				int vertIndex = i * inputMesh.vertexCount + j;	
				if (vertIndex >= normals.Length){
					Debug.Log("wtf");
				}
				normals[vertIndex].y = randomDirection;
				normals[vertIndex].z = randomRotationAxis;
			}
		}
	}

	protected override void UpdateColor (PAParticleField settings, int startAt)
	{
		int count = GetClampedParticleCount (settings.particleCount);

		SkipRandomCalls (1, startAt);

		for (int i = startAt; i < count; i++) {

			Color randomColor = settings.colorVariation.Evaluate(GetRandomAndIncrement(0f, 1f));

			for (int j = 0; j < inputMesh.vertexCount; j++) {

				int vertIndex = i * inputMesh.vertexCount + j;

				if (inputMesh.colors.Length>0){
					colors[vertIndex] = inputMesh.colors[j] * randomColor;
				}else{
					colors[vertIndex] = randomColor;
				}
			}
		}
	}

	protected override void UpdateSurface (PAParticleField settings, int startAt = 0)
	{
		int count = GetClampedParticleCount (settings.particleCount);//TODO this should be pre-clamped

        float columns = (settings.textureType != PAParticleField.TextureType.Simple ? settings.spriteColumns : 1f);
        float rows = (settings.textureType != PAParticleField.TextureType.Simple ? settings.spriteRows : 1f);
        Vector2 uv0Scale = new Vector2(1f / columns, 1f / rows);

		SkipRandomCalls (3, startAt);

		for (int i = startAt; i < count; i++) {

			float size = GetRandomAndIncrement(settings.minimumSize, 1f);

            Vector2 randomUVOffset = new Vector2((int)GetRandomAndIncrement(0f, columns), (int)GetRandomAndIncrement(0f, rows));

			for (int j = 0; j < inputMesh.vertexCount; j++) {
				int vertIndex = i * inputMesh.vertexCount + j;
				//fill positions
				verts [vertIndex] = inputMesh.vertices [j] * size;

				//encode normal into normal.r
				if (inputMesh.normals.Length > 0){
					normals[vertIndex].x = Vector3ToFloat(inputMesh.normals[j]);
				}

				//fill tangents
				if (inputMesh.tangents.Length > 0){
					tangents[vertIndex] = inputMesh.tangents[j];
				}

				//Fill UV1
				if (inputMesh.uv.Length > 0){
                    uv0[vertIndex] = Vector2.Scale(inputMesh.uv[j] + randomUVOffset, uv0Scale); 
				}
			}
		}
	}

	protected override void UpdateTriangles (PAParticleField settings, int startAt)
	{
		int count = GetClampedParticleCount (settings.particleCount);

		for (int i = startAt; i < count; i++) {
			for (int j = 0; j < inputMesh.triangles.Length; j++) {
				int triIndex = i * inputMesh.triangles.Length + j;
				triangles [triIndex] = inputMesh.triangles [j] + inputMesh.vertexCount * i;
			}
		}
	}

	protected override void UpdateSpeed (PAParticleField settings, int startAt)
	{
		int count = GetClampedParticleCount (settings.particleCount);
	
		SkipRandomCalls (2, startAt);

		for (int i = startAt; i < count; i++) {

			Vector2 speedData = new Vector2 (GetRandomAndIncrement (settings.minimumSpeed, 1f), GetRandomAndIncrement (settings.minSpinSpeed, 1f));

			for (int j = 0; j < inputMesh.vertexCount; j++) {
				int vertIndex = i * inputMesh.vertexCount + j;	

				uv1[vertIndex] = speedData;
			}
		}
	}

	static float Vector3ToFloat( Vector3 c ) {
		c = (c + Vector3.one) * 0.5f;
		return Vector3.Dot(new Vector3(Mathf.Round(c.x * 255),Mathf.Round(c.y * 255),Mathf.Round(c.z * 255)), new Vector3(65536, 256, 1));
	}
}
