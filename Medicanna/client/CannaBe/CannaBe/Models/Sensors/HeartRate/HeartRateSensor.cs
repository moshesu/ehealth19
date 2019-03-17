using Microsoft.Band.Sensors;
using System;
using System.Runtime.Serialization;

namespace CannaBe
{
    [DataContract]
    public class HeartRateSensorReading : ViewModel
    {
        DateTimeOffset _timestamp;

        [DataMember]
        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
            set
            {
                SetValue(ref _timestamp, value, "Timestamp");
            }
        }

        int _heartRate;

        [DataMember]
        public int HeartRate
        {
            get { return _heartRate; }
            set
            {
                SetValue(ref _heartRate, value, "HeartRate");
            }
        }

        HeartRateQuality _quality;

        [DataMember]
        public HeartRateQuality Quality
        {
            get { return _quality; }
            set
            {
                SetValue(ref _quality, value, "Quality");
            }
        }

        public int Value
        {
            get
            {
                return HeartRate;
            }
        }

        public double Accuracy
        {
            get
            {
                if (Quality == HeartRateQuality.Locked)
                {
                    return 1;
                }
                else
                {
                    return 0.5;
                }

            }
        }


    }
}
