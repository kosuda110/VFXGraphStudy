using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosu.UnityLibrary;
using UnityOSC;

public class DataManager : BaseDataManager<DataManager>
{

    protected override void Setup()
    {
        _senders = new ISender[0];
        _recievers = new IReciever[1];
        _recievers[0] = new UDPOSCReciever(10000)
        {
            IsQueueing = true,
            //onLatestDataRecieved = (msg) =>
            //{
            //    if (msg.IsBundle())
            //    {
            //        //Debug.LogError("Not implemented");
            //        var bundle = msg as OSCBundle;
            //        GetProjectionMatrix(bundle);
            //    }
            //    else
            //    {
            //        Debug.Log($"UDP OSCMessage (Thread) : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
            //    }
            //},
            onDataRecieved = (msg) =>
            {
                if (msg.IsBundle())
                {
                    var bundle = msg as OSCBundle;
                    GetProjectionMatrix(bundle);
                }
                else
                {
                    Debug.Log($"UDP OSCMessage : Address = {msg.Address} Data = {msg.Data[0].ToString()}");
                }
            }
        };
    }

    protected override void BeforeClose()
    {
    }

    private void GetProjectionMatrix(OSCBundle bundle)
    {
        var m00 = ((float) (bundle.Data[1] as OSCMessage).Data[0]);
        var m10 = ((float) (bundle.Data[2] as OSCMessage).Data[0]);
        var m20 = ((float) (bundle.Data[3] as OSCMessage).Data[0]);
        var m30 = ((float) (bundle.Data[4] as OSCMessage).Data[0]);
        var m01 = ((float) (bundle.Data[5] as OSCMessage).Data[0]);
        var m11 = ((float) (bundle.Data[6] as OSCMessage).Data[0]);
        var m21 = ((float) (bundle.Data[7] as OSCMessage).Data[0]);
        var m31 = ((float) (bundle.Data[8] as OSCMessage).Data[0]);
        var m02 = ((float) (bundle.Data[9] as OSCMessage).Data[0]);
        var m12 = ((float) (bundle.Data[10] as OSCMessage).Data[0]);
        var m22 = ((float) (bundle.Data[11] as OSCMessage).Data[0]);
        var m32 = ((float) (bundle.Data[12] as OSCMessage).Data[0]);
        var m03 = ((float) (bundle.Data[13] as OSCMessage).Data[0]);
        var m13 = ((float) (bundle.Data[14] as OSCMessage).Data[0]);
        var m23 = ((float) (bundle.Data[15] as OSCMessage).Data[0]);
        var m33 = ((float) (bundle.Data[16] as OSCMessage).Data[0]);
        var p00 = ((float) (bundle.Data[17] as OSCMessage).Data[0]);
        var p10 = ((float) (bundle.Data[18] as OSCMessage).Data[0]);
        var p20 = ((float) (bundle.Data[19] as OSCMessage).Data[0]);
        var p30 = ((float) (bundle.Data[20] as OSCMessage).Data[0]);
        var p01 = ((float) (bundle.Data[21] as OSCMessage).Data[0]);
        var p11 = ((float) (bundle.Data[22] as OSCMessage).Data[0]);
        var p21 = ((float) (bundle.Data[23] as OSCMessage).Data[0]);
        var p31 = ((float) (bundle.Data[24] as OSCMessage).Data[0]);
        var p02 = ((float) (bundle.Data[25] as OSCMessage).Data[0]);
        var p12 = ((float) (bundle.Data[26] as OSCMessage).Data[0]);
        var p22 = ((float) (bundle.Data[27] as OSCMessage).Data[0]);
        var p32 = ((float) (bundle.Data[28] as OSCMessage).Data[0]);
        var p03 = ((float) (bundle.Data[29] as OSCMessage).Data[0]);
        var p13 = ((float) (bundle.Data[30] as OSCMessage).Data[0]);
        var p23 = ((float) (bundle.Data[31] as OSCMessage).Data[0]);
        var p33 = ((float) (bundle.Data[32] as OSCMessage).Data[0]);
        Matrix4x4 m = new Matrix4x4()
        {
            m00 = m00, m01 = m01, m02 = m02, m03 = m03,
            m10 = m10, m11 = m11, m12 = m12, m13 = m13,
            m20 = m20, m21 = m21, m22 = m22, m23 = m23,
            m30 = m30, m31 = m31, m32 = m32, m33 = m33
        };
        Matrix4x4 p = new Matrix4x4()
        {
            m00 = p00, m01 = p01, m02 = p02, m03 = p03,
            m10 = p10, m11 = p11, m12 = p12, m13 = p13,
            m20 = p20, m21 = p21, m22 = p22, m23 = p23,
            m30 = p30, m31 = p31, m32 = p32, m33 = p33
        };
        //Debug.Log(m);
        //Camera.main.projectionMatrix = p;
        Camera.main.transform.parent.localScale = m.ExtractScale();
        Camera.main.transform.parent.position = m.ExtractPosition();
        Camera.main.transform.parent.rotation = m.ExtractRotation();
        float a = p[0];
        float b = Mathf.Abs( p[5]);
        float c = p[10];
        float d = p[14];

        float aspect_ratio = b / a;

        float k = (c - 1.0f) / (c + 1.0f);
        float clip_min = (d * (1.0f - k)) / (2.0f * k);
        float clip_max = k * clip_min;

        float RAD2DEG = 180.0f / 3.14159265358979323846f;
        float fov = RAD2DEG * (2.0f * (float) System.Math.Atan(1.0f / b));
        Camera.main.aspect = aspect_ratio;
        //Camera.main.nearClipPlane = clip_min;
        //Camera.main.farClipPlane = clip_max;
        Camera.main.fieldOfView = fov;
        Camera.main.projectionMatrix = p;
    }

}

public static class MatrixExtensions
{

    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }

    public static Vector3 ExtractScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
}
