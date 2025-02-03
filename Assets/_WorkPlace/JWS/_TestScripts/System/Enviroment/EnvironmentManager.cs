using UnityEngine;

namespace JWS
{
    public class EnvironmentManager : MonoBehaviour
    {
        public Light directionalLight;
        
        [Header("데이 나이트 시스템")][Range(0f, 24f)] public float timeOfDay = 12f; // 현재 시간 (0~24)
        public float daySpeed = 0.1f; // 시간 흐름 속도
        private bool sunWaiting = false; // 해가 -35도 이하로 내려갔을 때 대기

        public Gradient sunColorGradient;
        public Gradient fogColorGradient;
        public Gradient cloudColorGradient;
        public Gradient scatteringColorGradient;

        [Header("그라디에이션 플래그")]
        public bool overrideSunColor = true;
        public bool overrideFogColor = true;
        public bool overrideCloudColor = true;

        [Header("바람설정")]
        [Range(0f, 5f)]
        public float baseWindPower = 3f;
        public float baseWindSpeed = 1f;

        [Header("바람속성")]
        [Range(0f, 10f)]
        public float burstsPower = 0.5f;
        public float burstsSpeed = 5f;
        public float burstsScale = 10f;

        [Header("미세바람")]
        [Range(0f, 1f)]
        public float microPower = 0.1f;
        public float microSpeed = 1f;
        public float microFrequency = 3f;

        [Space(10)]
        public float renderDistance = 30f;

        [Space(10)]
        public float Altitude = 1000f;
        public float volumeSize = 500f;
        public int volumeSamples = 25;

        private float volumeOffset;
        private Mesh quadMesh;
        private Matrix4x4[] matrices;

        [Space(10)]
        public Material cloudsMaterial;

        private bool hasIssuedMaterialWarning = false;


        void Awake()
        {
            quadMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            matrices = new Matrix4x4[volumeSamples];
        }

        void Update()
        {
            UpdateTime(); // 🌞 시간 업데이트 추가
            UpdateEnvironment();
            UpdateCloudsVolume();
            UpdateLighting();
        }

        private void UpdateTime()
        {
            if (!sunWaiting)
            {
                timeOfDay += Time.deltaTime * daySpeed;
                if (timeOfDay >= 24f) timeOfDay = 0f;
            }
        }

        private void UpdateEnvironment()
        {
            Shader.SetGlobalFloat("WindPower", baseWindPower);
            Shader.SetGlobalFloat("WindSpeed", baseWindSpeed);
            Shader.SetGlobalFloat("WindBurstsPower", burstsPower);
            Shader.SetGlobalFloat("WindBurstsSpeed", burstsSpeed);
            Shader.SetGlobalFloat("WindBurstsScale", burstsScale);
            Shader.SetGlobalFloat("MicroPower", microPower);
            Shader.SetGlobalFloat("MicroSpeed", microSpeed);
            Shader.SetGlobalFloat("MicroFrequency", microFrequency);
            Shader.SetGlobalFloat("GrassRenderDist", renderDistance);
        }

        private void UpdateCloudsVolume()
        {
            volumeSamples = Mathf.Max(1, volumeSamples);
            volumeSize = Mathf.Max(0, volumeSize);

            if (cloudsMaterial == null)
            {
                return;
            }

            // Dynamically adjust the size of the matrices array to match volumeSamples
            if (matrices.Length != volumeSamples)
            {
                matrices = new Matrix4x4[volumeSamples];
            }

            if (!cloudsMaterial.HasProperty("_ScatteringColor"))
            {
                if (!hasIssuedMaterialWarning)
                {
                    Debug.LogWarning("The assigned material in the Cloud material slot of the EnvironmentManager isn't supported.");
                    hasIssuedMaterialWarning = true;
                }
                return;
            }
            else
            {
                hasIssuedMaterialWarning = false;
            }

            cloudsMaterial.SetFloat("_cloudsPosition", Altitude);
            cloudsMaterial.SetFloat("_cloudsHeight", volumeSize);

            volumeOffset = volumeSize / volumeSamples / 2f;
            Vector3 cloudsStartPosition = new Vector3(0, Altitude, 0) + (Vector3.up * (volumeOffset * volumeSamples / 2f));

            for (int i = 0; i < volumeSamples; i++)
            {
                matrices[i] = Matrix4x4.TRS(cloudsStartPosition - (Vector3.up * volumeOffset * i), Quaternion.Euler(-90, 0, 0), new Vector3(10000, 10000, 10000));
            }

            Graphics.DrawMeshInstanced(quadMesh, 0, cloudsMaterial, matrices, volumeSamples);
        }

        private bool reserveNormal = false;

        private void UpdateLighting()
        {
            if (directionalLight == null) return;

            float normalizedTime = timeOfDay / 24f;

            if (Mathf.Approximately(normalizedTime, 0f)) reserveNormal = !reserveNormal;

            float adjustedTime = !reserveNormal ? normalizedTime : 1f - normalizedTime;

            // 🌞 태양 각도 설정
            float sunAngle = Mathf.Lerp(-90f, 270f, normalizedTime);
            directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

            // 🌞 색상 업데이트
            if (overrideFogColor) RenderSettings.fogColor = fogColorGradient.Evaluate(adjustedTime);

            Color scatteringColor = sunColorGradient.Evaluate(adjustedTime);

            scatteringColor.r = Mathf.Max(scatteringColor.r, 0.01f);
            scatteringColor.g = Mathf.Max(scatteringColor.g, 0.01f);
            scatteringColor.b = Mathf.Max(scatteringColor.b, 0.01f);
            scatteringColor.a = Mathf.Max(scatteringColor.a, 0.05f);

            if (overrideSunColor) directionalLight.color = scatteringColor;

            if (cloudsMaterial != null && cloudsMaterial.HasProperty("_ScatteringColor") && overrideCloudColor)
            {
                cloudsMaterial.SetColor("_ScatteringColor", scatteringColorGradient.Evaluate(adjustedTime));
            }
            else if (cloudsMaterial == null)
            {
                Debug.LogError("cloudsMaterial is null. Please assign a material.");
            }

            Debug.Log($"Gradient Color: {scatteringColor}");
        }



        private void ResumeSunMovement()
        {
            sunWaiting = false;
        }
    }
}