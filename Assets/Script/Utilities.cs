using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class Utilities : MonoGenericSingleton<Utilities>
{

    public void ANIM_ShowNormal(Transform obj) => obj.DOScale(Vector3.one, 0.5f);

    public Tween ScaleObject(Transform obj, float scaleSize=1.5f, float duration=0f)
    {
        return obj.DOScale(Vector3.one * scaleSize, duration);
    }

    public void ANIM_ShowBounceNormal(Transform obj)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.25f));
        sequence.Append(obj.DOScale(Vector3.one, 0.5f));
        sequence.Play();
    }

    public void ANIM_ScaleUpDelayScaleDown(Transform obj, float delayTime=0f)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.DOScale(Vector3.one * 1.25f, 0.5f));
        seq.AppendInterval(delayTime);
        seq.Append(obj.DOScale(Vector3.one, 0.5f));
        seq.Play();
    }


    public void ANIM_HideNormal(Transform obj) => obj.DOScale(Vector3.zero, 0.5f);


    public void ANIM_HideBounce(Transform obj)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.25f));
        sequence.Join(obj.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(0.25f));
        sequence.Play();
    }

    public void ANIM_Explode(Transform obj)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOScale(Vector3.one, 1f));
        sequence.Join(obj.GetComponent<Image>().DOFade(0, 1f));
        sequence.Play();
    }

    public void ANIM_SpeakerReset(Transform obj)
    {
        // obj.DOScale(Vector3.zero, 0);
        // obj.GetComponent<Image>().DOFade(1, 0);
    }

    public void ANIM_Move(Transform obj, Vector3 endPos, float movementSpeed = 0.5f, TweenCallback callBack = null)
    {
        var _tween = obj.DOMove(endPos, movementSpeed);
        _tween.onComplete += callBack;
        _tween.Play();
    }

    public void ANIM_MoveWithScaleUp(Transform obj, Vector3 endPos, TweenCallback onCompleteCallBack=null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOMove(endPos, 0.5f));
        sequence.Join(obj.DOScale(Vector3.one * 1, 0.5f));
        sequence.onComplete += onCompleteCallBack;
        sequence.Play();
    }

    public void ANIM_MoveWithScaleDown(Transform obj, Vector3 endPos, TweenCallback onCompleteCallBack=null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOMove(endPos, 0.5f));
        sequence.Join(obj.DOScale(Vector3.one, 0.5f));
        sequence.onComplete += onCompleteCallBack;
        sequence.Play();
    }

    public void ANIM_CorrectScaleEffect(Transform obj) => StartCoroutine(IENUM_Hearbeat(obj));

    IEnumerator IENUM_Hearbeat(Transform obj)
    {
        for (int i = 0; i < 3; i++)
        {
            obj.DOScale(new Vector3(1.25f, 1.25f, 1), 0.5f);
            yield return new WaitForSeconds(0.25f);
            obj.DOScale(new Vector3(1, 1, 1), 0.5f);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void ANIM_WrongShakeEffect(Transform obj) => StartCoroutine(IENUM_HeadShake(obj));

    IEnumerator IENUM_HeadShake(Transform obj)
    {
        obj.GetComponent<Button>().interactable = false;
        for (int i = 0; i < 4; i++)
        {
            obj.DOMove(obj.position + new Vector3(0.25f, 0f, 0f), 0.1f);
            yield return new WaitForSeconds(0.1f);
            obj.DOMove(obj.position - new Vector3(0.25f, 0f, 0f), 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        obj.GetComponent<Button>().interactable = true;
    }

    public void ANIM_RotateHide(Transform obj, TweenCallback callback = null)
    {
        var tween = obj.DORotate(new Vector3(0, 90, 0), 0.35f);
        tween.onComplete += callback;
    }

    public void ANIM_RotateShow(Transform obj, TweenCallback callback = null)
    {
        var _tween = obj.DORotate(new Vector3(0, 0, 0), 0.35f);
        _tween.onComplete += callback;
    }

    public void ANIM_RotateObjWithCallback(Transform obj, TweenCallback callback = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.DORotate(new Vector3(0, 90, 0), 0.35f));
        seq.AppendCallback(callback);
        seq.Append(obj.DORotate(new Vector3(0, 0, 0), 0.35f));
        seq.Play();
    }

    public void ANIM_ShrinkObject(Transform obj, float actionTime=0.5f)
    {
        obj.DOScale(Vector3.zero, actionTime);
    }

    public void ANIM_ShakeObj(Transform obj)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(obj.DORotate(new Vector3(0, 0, 45), 0.35f));
        seq.Append(obj.DORotate(new Vector3(0, 0, -45), 0.25f));
        seq.Append(obj.DORotate(new Vector3(0, 0, 25), 0.15f));
        seq.Append(obj.DORotate(new Vector3(0, 0, -25), 0.05f));
        seq.Append(obj.DORotate(new Vector3(0, 0, 0), 0.05f));
    }

    public void ApplyScaleEffectsToChildObjects(GameObject[] objs, TweenCallback<GameObject> callback=null)
    {
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < objs.Length; i++)
        {
            var _child = objs[i].transform;
            seq.Append(_child.DOScale(Vector3.one * 1.25f, 0.15f));
            seq.Append(_child.DOScale(Vector3.one, 0.15f));
        }
    }

    public void ANIM_MoveUpDown(Transform obj, Vector3 endPosition, TweenCallback callback=null)
    {
        Vector3 startPosition = obj.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(obj.DOMove(endPosition, 1f));
        // foreach (var func in callback)
        // {
        sequence.AppendCallback(callback);
        // }
        sequence.Append(obj.DOMove(startPosition, 1f));
        sequence.Play();
    }

    public void ANIM_FlyIn(Transform obj) => obj.DOMoveY(-1.6f, 2f).SetEase(Ease.OutCirc);
    // public void ANIM_FlyIn(Transform obj) => obj.DOMove(new Vector3(obj.transform.position.x, -1.6f, 0), 2f).SetEase(Ease.OutCirc);


}