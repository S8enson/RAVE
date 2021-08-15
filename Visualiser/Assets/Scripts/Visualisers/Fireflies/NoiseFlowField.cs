using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFlowField : MonoBehaviour
{
    FastNoiseLite fastNoise;
    public Vector3Int gridSize;
    public float cellSize;
    public float increment;
    public Vector3 offset, offsetSpeed;
    public Vector3[,,] flowFieldDirection;
    
    // Particles
    public GameObject particlePrefab;
    public int numOfParticles;
    [HideInInspector]
    public List<FlowFieldParticle> particles;
    public List<MeshRenderer> particleMeshRenderer;

    public float spawnRadius;
    public float particleScale, particleMoveSpeed, particleRotateSpeed;

    bool particleSpawnValidation(Vector3 position){
        bool valid = true;
        foreach (FlowFieldParticle particle in particles)
        {
            if(Vector3.Distance(position, particle.transform.position) < spawnRadius){
                valid = false;
                break;
            }
        }
        if(valid){
            return true;
        }
        else{
            return false;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        flowFieldDirection = new Vector3[gridSize.x, gridSize.y, gridSize.z];
        fastNoise = new FastNoiseLite();
        particles = new List<FlowFieldParticle>();
        particleMeshRenderer = new List<MeshRenderer>();
        for (int i = 0; i < numOfParticles; i++)
        {
            int attempt = 0;

            while(attempt < 100){
            Vector3 randomPos = new Vector3(
                Random.Range(this.transform.position.x, this.transform.position.x + gridSize.x * cellSize),
                Random.Range(this.transform.position.y, this.transform.position.y + gridSize.y * cellSize),
                Random.Range(this.transform.position.z, this.transform.position.z + gridSize.z * cellSize));

            bool isValid = particleSpawnValidation(randomPos);

            if(isValid){
                GameObject particleInstance = (GameObject)Instantiate(particlePrefab);
                particleInstance.transform.position = randomPos;
                particleInstance.transform.parent = this.transform;
                particleInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
                particles.Add(particleInstance.GetComponent<FlowFieldParticle>());
                particleMeshRenderer.Add(particleInstance.GetComponent<MeshRenderer>());
                break;
            }
            if(!isValid){
                attempt++;
            }

            
        }
        }
        Debug.Log(particles.Count);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFFDir();
        ParticleBehaviour();
    }

    void CalculateFFDir(){
        
        
        float xOff = 0f;
        for (int x = 0; x < gridSize.x; x++)
        {
            offset = new Vector3(offset.x + (offsetSpeed.x * Time.deltaTime), offset.y + (offsetSpeed.y * Time.deltaTime), offset.z + (offsetSpeed.z * Time.deltaTime));
            float yOff = 0f;
            for (int y = 0; y < gridSize.y; y++)
            {
                float zOff = 0f;
                for (int z = 0; z < gridSize.z; z++)
                {
                    float noise = fastNoise.GetNoise(xOff + offset.x, yOff + offset.y, zOff + offset.z) + 1;
                    Vector3 noiseDirection = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));
                    flowFieldDirection[x,y,z] = Vector3.Normalize(noiseDirection);
                    zOff += increment;
                }
                yOff += increment;
            }
            xOff += increment;
        }
    }
    
    void ParticleBehaviour(){
        foreach (FlowFieldParticle p in particles)
        {
            // x
            if(p.transform.position.x > this.transform.position.x + (gridSize.x * cellSize)){
                p.transform.position = new Vector3(this.transform.position.x, p.transform.position.y, p.transform.position.z);
            }
            if(p.transform.position.x < this.transform.position.x){
                p.transform.position = new Vector3(this.transform.position.x + (gridSize.x * cellSize), p.transform.position.y, p.transform.position.z);
            }
            // y
            if(p.transform.position.y > this.transform.position.y + (gridSize.y * cellSize)){
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y, p.transform.position.z);
            }
            if(p.transform.position.y < this.transform.position.y){
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y + (gridSize.y * cellSize), p.transform.position.z);
            }
            // z
            if(p.transform.position.z > this.transform.position.z + (gridSize.z * cellSize)){
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z);
            }
            if(p.transform.position.z < this.transform.position.z){
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z + (gridSize.z * cellSize));
            }
            Vector3Int particlePos = new Vector3Int(
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.x - this.transform.position.x) / cellSize, 0, gridSize.x -1)),
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.y - this.transform.position.y) / cellSize, 0, gridSize.y -1)),
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.z - this.transform.position.z) / cellSize, 0, gridSize.z -1))
            );
            p.ApplyRotation(flowFieldDirection[particlePos.x, particlePos.y, particlePos.z], particleRotateSpeed);
            p.moveSpeed = particleMoveSpeed;
            //p.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
        }
    }


    private void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position + new Vector3((gridSize.x * cellSize) * 0.5f, (gridSize.y * cellSize) * 0.5f, (gridSize.z * cellSize) * 0.5f), 
        new Vector3(gridSize.x * cellSize, gridSize.y * cellSize, gridSize.z * cellSize));
    }

}
