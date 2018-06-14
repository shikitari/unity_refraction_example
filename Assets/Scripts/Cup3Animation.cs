using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace main
{
    public partial class Cup3 : MonoBehaviour
    {
        public class AnimationCoroutine1 : AnimationCoroutine
        {
            public AnimationCoroutine1(Cup3 owner) : base(owner)
            {
            }

            /// <summary>
            /// X-asis line of sight is changed 30 to 70 degrees.
            /// </summary>
            /// <returns>The ani01.</returns>
            /// <param name="callback">Callback.</param>
            public IEnumerator Sight30to70(Action callback = null)
            {
                coroutinesBreak = false;

                owner.cupTop.SetActive(false);

                GameObject g;
                g = Instantiate(MeshManager.arrowPrefab, Vector3.zero, Quaternion.identity);
                g.name = "arrow1";
                Arrow arrow1 = g.GetComponent<Arrow>();

                coroutines["sleep"] = owner.StartCoroutine(Animation(0.5f, 0, 1, t => { }));
                yield return coroutines["sleep"];

                coroutines["change_vision"] = owner.StartCoroutine(Animation(5f, 30, 70, t =>
                {
                    Camera.main.transform.eulerAngles = new Vector3(t, 180, 0);

                    RaycastHit rayCastHit;
                    bool isHit = Physics.Raycast(Camera.main.transform.position, 
                                                 Camera.main.transform.forward, 
                                                 out rayCastHit,
                                                 100, 
                                                 (int)LayerManager.layer.DefautAndCup
                                                 );

                    Vector3 start = Camera.main.transform.position;
                    Vector3 end = (isHit)? rayCastHit.point : Camera.main.transform.position + Camera.main.transform.forward;

                    arrow1.Create(start, end);
                }, easeOut));
                yield return coroutines["change_vision"];

                coroutines["sleep2"] = owner.StartCoroutine(Animation(0.5f, 0, 1, t => { }));
                yield return coroutines["sleep2"];

                Destroy(arrow1.gameObject);

                StopCoroutines();

                if (callback != null) callback();
            }

            /// <summary>
            /// Go up the water level
            /// </summary>
            /// <returns>The up.</returns>
            /// <param name="callback">Callback.</param>
            public IEnumerator UpTheWaterLevel(Action callback = null)
            {
                coroutinesBreak = false;

                GameObject g;
                g = Instantiate(MeshManager.arrowPrefab, Vector3.zero, Quaternion.identity);
                g.name = "arrow1";
                Arrow arrow1 = g.GetComponent<Arrow>();

                g = Instantiate(MeshManager.arrowPrefab, Vector3.zero, Quaternion.identity);
                g.name = "arrow2";
                Arrow arrow2 = g.GetComponent<Arrow>();
                arrow2.GetComponent<MeshRenderer>().material.color = Color.blue;

                //init Camera angle
                Camera.main.transform.LookAt(new Vector3(0, 1, 0.899f));

                Vector3 endPoint;
                coroutines["up_animation"] = owner.StartCoroutine(Animation(2f, Cup3.waterBottomY, Cup3.waterTopY, t =>
                {
                    endPoint = Camera.main.transform.forward + Camera.main.transform.position;
                    RaycastHit raycastHit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, (int)LayerManager.layer.DefautAndCupWater))
                    {
                        endPoint = raycastHit.point;
                    }
                    arrow1.Create(Camera.main.transform.position, endPoint);

                    if (raycastHit.collider.gameObject.layer == 10)
                    {
                        Vector3 dir = (endPoint - Camera.main.transform.position).normalized;
                        Vector3 refracted = Refraction.GetRefractionFormula(dir, Vector3.up, Refraction.air, Refraction.water);

                        RaycastHit raycastHit2;
                        if (Physics.Raycast(new Ray(endPoint, refracted), out raycastHit2, 100, (int)LayerManager.layer.CupInnerAndCupBottom))
                        {
                            arrow2.Create(endPoint, raycastHit2.point);
                        }
                    }

                    owner.cupTop.transform.position = new Vector3(0, t, 0);

                }, easeOut));
                yield return coroutines["up_animation"];

                coroutines["sleep2"] = owner.StartCoroutine(Animation(0.5f, 0, 1, t => { }));
                yield return coroutines["sleep2"];

                Destroy(arrow1.gameObject);
                Destroy(arrow2.gameObject);

                StopCoroutines();
                if (callback != null) callback();
            }
        }
    }
}