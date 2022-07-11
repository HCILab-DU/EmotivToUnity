using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLInput : MonoBehaviour
{
    public string StreamType = "EEG";
    public float scaleInput = 0.1f;

    StreamInfo[] streamInfos;
    StreamInlet streamInlet;

    float[] sample;
    private int channelCount = 0;

    void Update()
    {
        // 인렛이 없으면 만들어주기 위한 block
        if (streamInlet == null)
        {
            streamInfos = LSL.LSL.resolve_stream("type", StreamType, 1, 0.0);   // type, EEG, 1(받아올 최대 채널 수), 0(타임아웃 기준)

            // 스트림 정보가 뭐라도 있으면
            if (streamInfos.Length > 0) 
            {
                streamInlet = new StreamInlet(streamInfos[0]);
                channelCount = streamInlet.info().channel_count();  // 인렛이 받은 정보서 채널 개수를 읽어보고
                streamInlet.open_stream();
            }
        }

        // 인렛이 만들어 졌다면
        if (streamInlet != null)
        {
            sample = new float[channelCount];   // 채널 개수 길이의 float 배열, sample 

            double lastTimeStamp = streamInlet.pull_sample(sample, 0.0f);
            if (lastTimeStamp != 0.0)
            {
                Process(sample, lastTimeStamp);
                while ((lastTimeStamp = streamInlet.pull_sample(sample, 0.0f)) != 0)
                {
                    
                    
                    //info로 부터 sample에 데이터 집어넣기 
                    for (int i = 0; i < streamInfos.Length; i++)
                    {
                        //Debug.Log(streamInfos[i]);
                    }

                    //info로 부터 sample에 데이터 집어넣기 完

                    Process(sample, lastTimeStamp);
                }
            }
        }
    }
    void Process(float[] newSample, double timeStamp)
    {


        Debug.Log("sample[3]: " + newSample[3] + "   sample[4] " + newSample[4] + "   sample[5] " + newSample[5]);

        var inputVelocity = new Vector3(scaleInput * (newSample[0] - 0.5f), scaleInput * (newSample[1] - 0.5f), scaleInput * (newSample[2] - 0.5f));
        gameObject.transform.position = gameObject.transform.position + inputVelocity;
    }
}