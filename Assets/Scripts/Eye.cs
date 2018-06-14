using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System;

namespace main
{
    public class Eye : MonoBehaviour
    {

        void Start()
        {
            TraceCamera();

            GameObject traceCamera = GameObject.Find("CameraEyeTrace");
            Vector3 dir = Quaternion.AngleAxis(-30, transform.up) * transform.forward;
            Vector3 p = transform.position + dir * 1.5f;
            traceCamera.transform.position = p;
            traceCamera.transform.LookAt(transform.position);

            string s = Some.SerializeObject<Vector3>(transform.position);
        }

        void FixedUpdate()
        {
            TraceCamera();
        }


        void TraceCamera()
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }


    }

    public static class Some {
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}