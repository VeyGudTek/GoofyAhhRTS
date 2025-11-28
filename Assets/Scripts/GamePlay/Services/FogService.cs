using System.Collections.Generic;
using System.Linq;
using Source.GamePlay.Services.Unit;
using Source.GamePlay.Services.Unit.Instance;
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

        private const float FogQualityMultiplier = 2f;
        private float Top => Mathf.Max(Corner1.position.z, Corner2.position.z) * FogQualityMultiplier;
        private float Bottom => Mathf.Min(Corner1.position.z, Corner2.position.z) * FogQualityMultiplier;
        private float Left => Mathf.Min(Corner1.position.x, Corner2.position.x) * FogQualityMultiplier;
        private float Right => Mathf.Max(Corner1.position.x, Corner2.position.x) * FogQualityMultiplier;
        private float Length => Right - Left;
        private float Height => Top - Bottom;
        
        public void InjectDependencies(UnitManagerService unitManagerService)
        {
            UnitManagerService = unitManagerService;
        }

        public void Start()
        {
            this.CheckInitializeRequired();

            InvokeRepeating(nameof(UpdateTexture), 0f, .25f);
        }

        public void UpdateTexture()
        {
            Texture2D fogAlphaMap = InitializeTexture();
            List<PixelData> pixels = GetPixels(fogAlphaMap);

            foreach (PixelData pixel in pixels)
            {
                fogAlphaMap.SetPixel(fogAlphaMap.width - pixel.x, fogAlphaMap.height - pixel.y, new Color(255, 255, 255, pixel.a));
            }
            fogAlphaMap.Apply();

            Renderer.material.mainTexture = fogAlphaMap;
        }

        private Texture2D InitializeTexture()
        {
            Texture2D fogAlphaMap = Texture2D.blackTexture;
            fogAlphaMap.Reinitialize((int)Length, (int)Height);
            fogAlphaMap.wrapMode = TextureWrapMode.Clamp;

            Color[] pixels = new Color[(int)Length * (int)Height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(255, 255, 255, 1);
            }
            fogAlphaMap.SetPixels(pixels);

            return fogAlphaMap;
        }

        private List<PixelData> GetPixels(Texture2D fogAlphaMap)
        {
            List<PixelData> pixels = new();

            foreach (UnitService unit in UnitManagerService.AllyUnits)
            {
                Vector2 worldPosition = new Vector2(unit.transform.position.x * FogQualityMultiplier, unit.transform.position.z * FogQualityMultiplier);
                Vector2 mapPosition = MapPositionToPixel(worldPosition, fogAlphaMap);
                float radius = unit.UnitVisionService.VisionRange * FogQualityMultiplier;

                for (float i = (int)-radius; i <= radius; i++)
                {
                    for (float j = (int)-radius; j <= radius; j++)
                    {
                        Vector2 pixelPosition = new Vector2(
                            (int)Mathf.Clamp(i + mapPosition.x, 0, fogAlphaMap.width),
                            (int)Mathf.Clamp(j + mapPosition.y, 0, fogAlphaMap.height)
                        );
                        float alpha = GetAlpha(radius, Vector2.Distance(pixelPosition, mapPosition));

                        PixelData existingPixel = pixels.Where(p => p.x == pixelPosition.x && p.y == pixelPosition.y).FirstOrDefault();
                        if (existingPixel != null)
                        {
                            existingPixel.a = Mathf.Min(existingPixel.a, alpha);
                        }
                        else
                        {
                            pixels.Add(new PixelData() { x = (int)pixelPosition.x, y = (int)pixelPosition.y, a = alpha });
                        }
                    }
                }
            }

            return pixels;
        }

        private Vector2 MapPositionToPixel(Vector2 worldPosition, Texture2D alphaMap)
        {
            float worldX = Mathf.Clamp(worldPosition.x, Left, Right);
            float worldZ = Mathf.Clamp(worldPosition.y, Bottom, Top);

            float textureX = (worldX - Left);
            float textureY = (worldZ - Bottom + 1);

            return new Vector2(textureX, textureY);
        }

        private float GetAlpha(float radius, float distanceFromCenter)
        {
            float fullAlphaRadiusPercent = .6f;
            float dropOffDistance = (1f - fullAlphaRadiusPercent) * radius;

            if (distanceFromCenter <= radius * fullAlphaRadiusPercent)
            {
                return 0f;
            }

            return Mathf.Max(0, 1f - (radius - distanceFromCenter) / dropOffDistance);
        }
    }
}

