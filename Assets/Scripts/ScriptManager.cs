using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace main
{
    static public class ScriptManager
    {
        static public Dictionary<string, MonoBehaviour> scriptList = new Dictionary<string, MonoBehaviour>();

        static public T Get<T>(String name = null, MonoBehaviour findTarget = null, bool forceFind = false) where T : MonoBehaviour
        {
            string fullName = typeof(T).FullName;

            if (scriptList.ContainsKey(fullName) && scriptList[fullName] != null && forceFind == false)
            {
                return (T)scriptList[fullName];
            }

            Type t = Type.GetType(fullName);
            string gameObjectName;
            if (name == null) {
                gameObjectName = typeof(T).Name;
            } else {
                gameObjectName = name;
            }

            T script;
            if (findTarget == null)
            {
                script = (T)GameObject.Find(gameObjectName)?.GetComponent(t);
            } 
            else
            {
                script = (T)findTarget.transform.Find(gameObjectName)?.GetComponent(t);
            }

            scriptList[fullName] = script;
            return script;
        }
    }
}