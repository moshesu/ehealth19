using Microsoft.Band.Sensors;
using System;
using System.Runtime.Serialization;

namespace CannaBe
{
    [DataContract]
    public class ContactSensorReading : ViewModel
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

        BandContactState _contact;

        [DataMember]
        public BandContactState Contact
        {
            get { return _contact; }
            set
            {
                SetValue(ref _contact, value, "Contact");
            }
        }

        public BandContactState Value
        {
            get
            {
                return Contact;
            }
        }
    }
}
