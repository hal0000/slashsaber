using UnityEngine;
using UnityEngine.UI;

public class PredictCollide : MonoBehaviour
{
    public GameObject TipRay;
    public GameObject BaseRay;
    public Text PredictResult;

    [SerializeField]
    int _predictRayAmount;

    public bool Predict = false;

    void FixedUpdate()
    {
        if (!Predict) return;
        var tipPos = TipRay.transform.position;
        var tipDelta = TipRay.transform.position - BaseRay.transform.position;
        var delta = tipDelta / _predictRayAmount;
        for (int i = 0; i < _predictRayAmount; i++)
        {
            if (Physics.Raycast(tipPos, Vector3.back, 10))
            {
                PredictResult.text = "HIT";
                PredictResult.color = Color.green;
                Predict = false;
                break;
            }
            else
            {
                PredictResult.text = "MISS";
                PredictResult.color = Color.red;
                Debug.DrawRay(tipPos, Vector3.back, Color.blue, 1f);
            }
            tipPos -= delta;
        }
        Predict = false;
    }
}