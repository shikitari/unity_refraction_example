using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace main{
    public class AnimationCoroutine
    {
        public static Func<float, float> easeOut = t => t * (2 - t);//

        protected Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
        protected bool coroutinesBreak = false;
        protected Cup3 owner;

        public AnimationCoroutine(Cup3 owner)
        {
            this.owner = owner;
        }

        public void StopCoroutines()
        {
            coroutinesBreak = true;
            foreach (KeyValuePair<string, Coroutine> coroutine in coroutines)
            {
                if (coroutine.Value != null) owner.StopCoroutine(coroutine.Value);
            }
            coroutines.Clear();
        }

        public IEnumerator Animation(float time, float start, float end, Action<float> routine, Func<float, float> easing = null)
        {
            if (coroutinesBreak) yield break;

            if (Mathf.Abs(end - start) < 0.001f || time < 0.001f)
            {
                if (routine != null) routine(end);
                yield break;
            }
            float interval = Time.fixedDeltaTime;
            float frames = time * (1 / interval);
            float step = 1f / frames;
            for (float t = 0; t < 1; t += step)
            {
                float r = (easing != null) ? easing(t) : t;
                float v = (end - start) * r + start;
                if (routine != null) routine(v);
                yield return new WaitForSeconds(interval);
            }
            if (routine != null) routine(end);
            yield break;
        }
    }
}