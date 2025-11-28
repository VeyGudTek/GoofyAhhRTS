using Source.GamePlay.Services.Unit;
using Source.GamePlay.Services.Unit.Instance;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
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

        private float Top => Mathf.Max(Corner1.position.z, Corner2.position.z);
        private float Bottom => Mathf.Min(Corner1.position.z, Corner2.position.z);
        private float Left => Mathf.Min(Corner1.position.x, Corner2.position.x);
        private float Right => Mathf.Max(Corner1.position.x, Corner2.position.x);
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

            foreach (UnitService unit in UnitManagerService.AllyUnits)
            {
                Vector2 worldPosition = new Vector2(unit.transform.position.x, unit.transform.position.z);
                Vector2 mapPosition = MapPositionToPixel(worldPosition, fogAlphaMap);
                float radius = unit.UnitVisionService.VisionRange;

                for (int i = (int)-radius; i <= radius; i++)
                {
                    for (int j = (int)-radius; j <= radius; j++)
                    {
                        Vector2 pixelPosition = new Vector2(
                            Mathf.Clamp(i + mapPosition.x, 0, fogAlphaMap.width), 
                            Mathf.Clamp(j + mapPosition.y, 0, fogAlphaMap.height)
                        );
                        float alpha = GetAlpha(radius, Vector2.Distance(pixelPosition, mapPosition));
                        fogAlphaMap.SetPixel(fogAlphaMap.width - (int)pixelPosition.x, fogAlphaMap.height - (int)pixelPosition.y, new Color(255, 255, 255, alpha));
                    }
                }
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

        private Vector2 MapPositionToPixel(Vector2 worldPosition, Texture2D alphaMap)
        {
            float worldX = Mathf.Clamp(worldPosition.x, Left, Right);
            float worldZ = Mathf.Clamp(worldPosition.y, Bottom, Top);

            float textureX = worldX - Left + 1;
            float textureY = worldZ - Bottom + 1;

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

