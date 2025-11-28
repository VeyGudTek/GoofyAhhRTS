using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.GamePlay.Services.Unit;
using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Static.Classes;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class PixelData
    {
        public int x;
        public int y;
        public float a;
    }

    public class FogService : MonoBehaviour
    {
        [SerializeField]
        private Renderer Renderer;
        [SerializeField]
        private Transform Corner1;
        [SerializeField]
        private Transform Corner2;
        [InitializationRequired]
        private UnitManagerService UnitManagerService { get; set; }

        private const float FogQualityMultiplier = 1f;
        private float Top => Mathf.Max(Corner1.position.z, Corner2.position.z) * FogQualityMultiplier;
        private float Bottom => Mathf.Min(Corner1.position.z, Corner2.position.z) * FogQualityMultiplier;
        private float Left => Mathf.Min(Corner1.position.x, Corner2.position.x) * FogQualityMultiplier;
        private float Right => Mathf.Max(Corner1.position.x, Corner2.position.x) * FogQualityMultiplier;
        private float Length => Right - Left;
        private float Height => Top - Bottom;
        private bool ReadyToUpdate { get; set; } = true;
        private Texture2D FogAlphaMap { get; set; }
        private const int IterationsPerFrame = 2000;
        
        public void InjectDependencies(UnitManagerService unitManagerService)
        {
            UnitManagerService = unitManagerService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();

            FogAlphaMap = InitializeTexture();
        }

        private void Update()
        {
            if (ReadyToUpdate)
            {
                UpdateTexture();
                ReadyToUpdate = false;
            }
        }

        public void UpdateTexture()
        {
            InitializeColor();
            List<PixelData> pixels = new();
            StartCoroutine(UpdatePixels(pixels));
        }

        private Texture2D InitializeTexture()
        {
            Texture2D fogAlphaMap = Texture2D.blackTexture;
            fogAlphaMap.Reinitialize((int)Length, (int)Height);
            fogAlphaMap.wrapMode = TextureWrapMode.Clamp;

            return fogAlphaMap;
        }

        private void InitializeColor()
        {
            Color[] pixels = new Color[(int)Length * (int)Height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(255, 255, 255, 1);
            }
            FogAlphaMap.SetPixels(pixels);
        }

        private IEnumerator UpdatePixels(List<PixelData> pixels)
        {
            yield return null;
            int currentIteration = 0;
            foreach (UnitService unit in UnitManagerService.AllyUnits.ToList())
            {
                Vector3 worldPosition = unit.transform.position;
                Vector2 mapPosition = MapPositionToPixel(worldPosition);
                float radius = unit.UnitVisionService.VisionRange * FogQualityMultiplier;

                for (float i = (int)-radius; i <= radius; i++)
                {
                    for (float j = (int)-radius; j <= radius; j++)
                    {
                        Vector2 pixelPosition = new Vector2(
                            (int)Mathf.Clamp(i + mapPosition.x, 0, FogAlphaMap.width),
                            (int)Mathf.Clamp(j + mapPosition.y, 0, FogAlphaMap.height)
                        );
                        float alpha = GetAlpha(radius, pixelPosition, mapPosition, worldPosition);

                        PixelData existingPixel = pixels.Where(p => p.x == pixelPosition.x && p.y == pixelPosition.y).FirstOrDefault();
                        if (existingPixel != null)
                        {
                            existingPixel.a = Mathf.Min(existingPixel.a, alpha);
                        }
                        else
                        {
                            pixels.Add(new PixelData() { x = (int)pixelPosition.x, y = (int)pixelPosition.y, a = alpha });
                        }
                        currentIteration += 1;
                        if (currentIteration >= IterationsPerFrame)
                        {
                            yield return null;
                        }
                    }
                }
            }

            SetPixels(pixels);
        }

        private Vector2 MapPositionToPixel(Vector3 worldPosition)
        {
            float worldX = Mathf.Clamp(worldPosition.x * FogQualityMultiplier, Left, Right);
            float worldZ = Mathf.Clamp(worldPosition.z * FogQualityMultiplier, Bottom, Top);

            float textureX = (worldX - Left);
            float textureY = (worldZ - Bottom);

            return new Vector2(textureX, textureY);
        }

        private float GetAlpha(float radius, Vector2 pixelPosition, Vector2 mapPosition, Vector3 unitWorldPosition)
        {
            float distanceFromCenter = Vector2.Distance(pixelPosition, mapPosition);

            int layerMask = LayerMask.GetMask(LayerNames.Obstacle);
            if (Physics.Raycast(unitWorldPosition, GetPixelDirection(pixelPosition, mapPosition), distanceFromCenter / FogQualityMultiplier, layerMask))
            {
                return 1f;
            }

            float fullAlphaRadiusPercent = .6f;
            float dropOffDistance = (1f - fullAlphaRadiusPercent) * radius;

            if (distanceFromCenter <= radius * fullAlphaRadiusPercent)
            {
                return 0f;
            }

            return Mathf.Max(0, 1f - (radius - distanceFromCenter) / dropOffDistance);
        }

        private Vector3 GetPixelDirection(Vector2 pixelPosition, Vector2 unitMapPosition)
        {
            return new Vector3(
                pixelPosition.x - unitMapPosition.x,
                0f,
                pixelPosition.y - unitMapPosition.y
            );
        }

        private void SetPixels(List<PixelData> pixels)
        {
            foreach (PixelData pixel in pixels)
            {
                FogAlphaMap.SetPixel(FogAlphaMap.width - pixel.x, FogAlphaMap.height - pixel.y, new Color(255, 255, 255, pixel.a));
            }
            FogAlphaMap.Apply();

            Renderer.material.mainTexture = FogAlphaMap;
            ReadyToUpdate = true;
        }
    }
}

