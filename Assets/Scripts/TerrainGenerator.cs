using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject Player;
	public GameObject Creeper;

    public GameObject DirtBlock;
	public GameObject StoneBlock;
	public GameObject GrassBlock;
	public GameObject SnowBlock;
	public GameObject IceBlock;

	public const int Width = 128;
	public const int Length = 128;
	public const int Height = 16;
	public int ChunkSize = 32;

	//public const int Width = 64;
	//public const int Length = 64;
	//public const int Height = 12;
	//public int ChunkSize = 16;

	public float BottomLimit = 0.4f;
	public float MiddleLimit = 0.7f;
	public int MaxAmplitude = 3;
	public int CreeperCount = 5;

	private float amplitude;

	private Chunk[] chunks;

	void Start()
    {
		InstantiateTerrain();
		Instantiate(Player, new Vector3(3, Height * amplitude + 2, 3), Quaternion.identity);

		for (int i = 0; i < CreeperCount; ++i) {
			Instantiate(
				Creeper,
				new Vector3(Random.Range(7, Width), Height * amplitude + 2, Random.Range(7, Length)),
				Quaternion.identity
			);
		}
    }

	private void InitializeChunks()
	{
		chunks = new[] {
			new Chunk(StoneBlock, DirtBlock, GrassBlock),
			new Chunk(StoneBlock, DirtBlock, SnowBlock),
			new Chunk(StoneBlock, IceBlock, SnowBlock),
			new Chunk(StoneBlock, StoneBlock, StoneBlock),
			new Chunk(IceBlock, IceBlock, IceBlock),
			new Chunk(DirtBlock, GrassBlock, GrassBlock),
		};
	}

	private float[,] CreateTerrainMap()
	{
		float[,] map = new float[Width, Length];
		float scale = Random.Range(0.7f, 1.3f);
		float xShift = Random.Range(-20f, 20f);
		float yShift = Random.Range(-20f, 20f);
		amplitude = Random.Range(1f, MaxAmplitude);

		for (int i = 0; i < Width; ++i) {
			for (int j = 0; j < Length; ++j) {
				map[i, j] = Mathf.PerlinNoise((i - xShift) / Width * scale, (j - yShift) / Length * scale) * amplitude;
			}
		}

		return map;
	}

	private Chunk[,] CreateChunkMap()
	{
		Chunk[,] map = new Chunk[Width, Length];

		int x = 0;
		int y = 0;

		while (x < Width) {
			var chunk = chunks[Random.Range(0, chunks.Length - 1)];
			for (int i = 0; i < Width / ChunkSize; ++i) {
				for (int j = 0; j < Length / ChunkSize; ++j) {
					if (x + i < Width && y + j < Length) {
						map[x + i, y + j] = chunk;
					}
				}
			}

			y += Length / ChunkSize;
			if (y >= Length) {
				y = 0;
				x += Width / ChunkSize;
			}
		}

		return map;
	}

	private void InstantiateTerrain()
	{
		float[,] map = CreateTerrainMap();

		InitializeChunks();
		Chunk[,] chunkMap = CreateChunkMap();

		for (int i = 0; i < Width; ++i) {
			for (int j = 0; j < Length; ++j) {
				int limit = Mathf.RoundToInt(map[i, j] * Height);
				var chunk = chunkMap[i, j];
				for (int k = 0; k < limit; ++k) {
					var block =
						k < Height * BottomLimit
							? chunk.Bottom
							: k < Height * MiddleLimit
								? chunk.Middle
								: chunk.Top;
					Instantiate(block, new Vector3(i, k, j), Quaternion.identity);
				}
			}
		}
	}

	public struct Chunk
	{
		public GameObject Bottom;
		public GameObject Middle;
		public GameObject Top;

		public Chunk(GameObject bottom, GameObject middle, GameObject top)
		{
			Bottom = bottom;
			Middle = middle;
			Top = top;
		}
	}
}
