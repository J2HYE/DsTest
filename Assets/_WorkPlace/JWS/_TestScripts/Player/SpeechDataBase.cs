using System.Collections.Generic;
using UnityEngine;

namespace JWS
{
    [CreateAssetMenu(fileName = "SpeechDataBase", menuName = "SpeechDataBase", order = 1)]
    public class SpeechDataBase : ScriptableObject
    {
        [System.Serializable]
        public class SpeechData
        {
            public string keyChar;
            public float[] blendShapeWeights = new float[5];
        }

        public List<SpeechData> speechData = new List<SpeechData>();
        private Dictionary<string, float[]> dictionary;

        public bool TryGetBlendShape(string key, out float[] weights)
        {
            if (dictionary == null)
            {
                BuildDictionary();
            }
            return dictionary.TryGetValue(key, out weights);
        }

        private void BuildDictionary()
        {
            dictionary = new Dictionary<string, float[]>();
            foreach (var data in speechData)
            {
                if (!dictionary.ContainsKey(data.keyChar))
                {
                    dictionary.Add(data.keyChar, data.blendShapeWeights);
                }
            }
        }

        [ContextMenu("스피치 데이터 초기화")]
        public void InitializeAlphabetData()
        {
            speechData.Clear();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                float[] weights = new float[5] { 0f, 0f, 0f, 0f, 0f };
                if ("AEIOU".Contains(c))
                {
                    switch (c)
                    {
                        case 'A': weights[0] = 100f; break;
                        case 'I': weights[1] = 100f; break;
                        case 'U': weights[2] = 100f; break;
                        case 'E': weights[3] = 100f; break;
                        case 'O': weights[4] = 100f; break;
                    }
                }
                else
                {
                    weights = c switch
                    {
                        'B' => new float[5] { 100f, 0f, 50f, 0f, 0f },
                        'C' => new float[5] { 50f, 50f, 0f, 100f, 0f },
                        'D' => new float[5] { 0f, 0f, 0f, 50f, 0f },
                        'F' => new float[5] { 100f, 0f, 0f, 0f, 100f },
                        'G' => new float[5] { 0f, 0f, 50f, 50f, 0f },
                        'H' => new float[5] { 0f, 50f, 0f, 50f, 0f },
                        'J' => new float[5] { 0f, 50f, 100f, 0f, 0f },
                        'K' => new float[5] { 0f, 0f, 50f, 100f, 0f },
                        'L' => new float[5] { 0f, 100f, 0f, 50f, 0f },
                        'M' => new float[5] { 100f, 0f, 0f, 0f, 50f },
                        'N' => new float[5] { 0f, 50f, 0f, 0f, 50f },
                        'P' => new float[5] { 100f, 0f, 0f, 0f, 0f },
                        'Q' => new float[5] { 0f, 0f, 50f, 0f, 100f },
                        'R' => new float[5] { 0f, 100f, 0f, 0f, 50f },
                        'S' => new float[5] { 0f, 50f, 0f, 100f, 0f },
                        'T' => new float[5] { 0f, 50f, 0f, 50f, 0f },
                        'V' => new float[5] { 100f, 0f, 0f, 0f, 50f },
                        'W' => new float[5] { 0f, 0f, 50f, 0f, 100f },
                        'X' => new float[5] { 0f, 50f, 0f, 100f, 0f },
                        'Y' => new float[5] { 0f, 100f, 0f, 50f, 0f },
                        'Z' => new float[5] { 0f, 50f, 0f, 100f, 0f },
                        _ => new float[5] { 0f, 0f, 0f, 0f, 0f },
                    };
                }

                speechData.Add(new SpeechData
                {
                    keyChar = c.ToString(),
                    blendShapeWeights = weights
                });
            }


            speechData.Add(new SpeechData
            {
                keyChar = "Space",
                blendShapeWeights = new float[5] { 0f, 0f, 0f, 0f, 0f }
            });

            speechData.Add(new SpeechData
            {
                keyChar = "Hyphen",
                blendShapeWeights = new float[5] { 0f, 0f, 0f, 0f, 0f }
            });

            BuildDictionary();
            Debug.Log("스피치 데이터 초기화 했습니다.");
        }
    }
}