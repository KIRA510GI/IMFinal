using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using System.Reflection; // 리플렉션을 사용하기 위한 네임스페이스

public class XRSimulatorSpeedAdjuster : MonoBehaviour
{
    public XRDeviceSimulator simulator; // XR Device Simulator 참조
    public float newSpeed = 5f;         // 설정할 이동 속도

    private void Start()
    {
        // XRDeviceSimulator 참조 가져오기
        if (simulator == null)
        {
            simulator = GetComponent<XRDeviceSimulator>();
        }

        if (simulator != null)
        {
            // 리플렉션을 통해 translateAnchorSpeed 필드에 접근
            FieldInfo field = typeof(XRDeviceSimulator).GetField("m_TranslateSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                // translateAnchorSpeed에 값을 설정
                field.SetValue(simulator, newSpeed);
                Debug.Log($"Simulator translate speed set to {newSpeed}");
            }
            else
            {
                Debug.LogError("m_TranslateSpeed field not found in XRDeviceSimulator!");
            }
        }
        else
        {
            Debug.LogError("XR Device Simulator not found!");
        }
    }
}