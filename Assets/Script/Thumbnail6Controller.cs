using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Thumbnail6Controller : MonoBehaviour
{
    public GameObject IMG_questionImagePanel;
    public GameObject G_optionsParent;
    public Transform T_startPoint, T_endPoint;
    public Sprite[] SPR_questionSprite;
    public AudioClip[] AC_answerClip;
    public AudioClip[] AC_optionClip;
    public GameObject G_questionImagePrefab;
    public GameObject G_activityCompleted;
    public AudioSource AS_emptyAudioSource;
    public AudioClip AC_wrongAudioClip;
    public TextMeshProUGUI TMP_timeCounter;
    public TextMeshProUGUI TMP_counter;
    public float F_waitTime;
    GameObject _currentQuestion = null;
    GameObject _prevQuestion = null;
    GameObject _currentSelectedOption = null;
    int _currentIndex = 0;
    bool B_canInteract = false;
    bool B_counter = true;
    float counter = 1f;
    int displayCounter = 10;

    void Start()
    {
        OpenOptions();

        Invoke(nameof(CloseOptions), F_waitTime);
        Invoke(nameof(SpawnQuestion), F_waitTime + 1.5f);

        EnableTimeCounter();
    }

    string RemoveTag(string text) { return Regex.Replace(text, "<.*?>", string.Empty); }

    void OpenOptions()
    {
        int childCount = G_optionsParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform _obj = G_optionsParent.transform.GetChild(i);
            OpenOption(_obj);
        }
    }

    void UpdateAndResetCounter()
    {
        Utilities.Instance.ANIM_ShowBounceNormal(TMP_timeCounter.transform.parent);
        counter = 1f;
        TMP_timeCounter.text = $"{--displayCounter}";
        TMP_timeCounter.transform.parent.GetComponent<Image>().fillAmount = counter;
    }

    private void Update() {
        if(!B_counter) return;

        if(displayCounter <= 0) { B_counter = false; DisableTimeCounter(); EnableQuestionCounter(); return;}

        if(counter < 0f) { UpdateAndResetCounter(); }

        counter -= Time.deltaTime;
        TMP_timeCounter.transform.parent.GetComponent<Image>().fillAmount = counter;
    }

    void EnableTimeCounter() {
        TMP_timeCounter.text = $"{displayCounter}";
        TMP_timeCounter.transform.parent.gameObject.SetActive(true);
    }

    void DisableTimeCounter() { TMP_timeCounter.transform.parent.gameObject.SetActive(false); }

    void EnableQuestionCounter() { 
        UpdateQuestionCounter();
        TMP_counter.gameObject.SetActive(true);
    }

    void UpdateQuestionCounter() {
        TMP_counter.text = $"{_currentIndex + 1}/{SPR_questionSprite.Length}";
    }

    void CloseOptions()
    {
        int childCount = G_optionsParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform _obj = G_optionsParent.transform.GetChild(i);
            CloseOption(_obj);
        }
    }

    void OpenOption(Transform _obj)
    {
        Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
            () => {
                    _obj.transform.GetChild(1).gameObject.SetActive(false);
                    _obj.transform.GetChild(0).gameObject.SetActive(true);
            }
        );
    }

    void CloseOption(Transform _obj)
    {
        Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
            () => {
                _obj.transform.GetChild(0).gameObject.SetActive(false);
                _obj.transform.GetChild(1).gameObject.SetActive(true);
            }
        );
    }

    public static void EnableOptionText(GameObject optionObj)
    {
        optionObj.transform.GetChild(1).gameObject.SetActive(false);
        optionObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    public static void EnableOptionBG(GameObject optionObj)
    {
        optionObj.transform.GetChild(0).gameObject.SetActive(false);
        optionObj.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnBGPanelClicked()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        if(!B_canInteract || selectedObj.CompareTag("answer")) return;

        DisableClicking();
        if(_currentSelectedOption != null)
            if(_currentSelectedOption.name != selectedObj.name)
                CloseOption(_currentSelectedOption.transform);
            else if(_currentSelectedOption.name == selectedObj.name)
            {
                EnableClicking();
                return;
            }

        OpenOption(selectedObj.transform);
        _currentSelectedOption = selectedObj;
        var selectedOptText = RemoveTag(selectedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

        if(EvaluateAnswer(selectedOptText))
        {
            selectedObj.tag = "answer";
            _currentSelectedOption = null;
            _currentIndex++;
            PlayOptionVO(selectedOptText);
            Invoke(nameof(SpawnQuestion), 3.5f);
        }else{
            PlayAudio(AC_wrongAudioClip);
            EnableClicking();
        }
    }

    public void OnQuestionIMGClicked()
    {
        AudioManager.PlayAudio(AC_answerClip[_currentIndex]);
    }

    void PlayOptionVO(string selectedObjSTR)
    {
        int i = 0;
        for (; i < AC_optionClip.Length; i++)
        {
            if(AC_optionClip[i].name.Contains(selectedObjSTR))
            {
                PlayAudio(AC_optionClip[i]);
                break;
            }
        }
        Invoke(nameof(PlayQuestionVO), AC_optionClip[i].length);
    }

    void PlayQuestionVO()
    {
        PlayAudio(AC_answerClip[_currentIndex - 1]);
    }

    void PlayAudio(AudioClip _audioClip)
    {
        AS_emptyAudioSource.PlayOneShot(_audioClip);
    }

    bool EvaluateAnswer(string clickedOptString)
    {
        return SPR_questionSprite[_currentIndex].name.Contains(clickedOptString);
    }

    void SpawnQuestion()
    {
        if(_currentIndex == SPR_questionSprite.Length){
            G_activityCompleted.SetActive(true);
            return;
        }

        _prevQuestion = _currentQuestion;
        MovePrevQuestion();
        var spawnedQuestion = Instantiate(G_questionImagePrefab, IMG_questionImagePanel.transform);
        spawnedQuestion.GetComponent<Image>().sprite = SPR_questionSprite[_currentIndex];
        spawnedQuestion.GetComponent<Image>().preserveAspect = true;
        spawnedQuestion.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        spawnedQuestion.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        spawnedQuestion.GetComponent<Button>().onClick.AddListener(OnQuestionIMGClicked);

        _currentQuestion = spawnedQuestion;
        Utilities.Instance.ANIM_Move(spawnedQuestion.transform, T_startPoint.position, 0f, MoveQuestion);
        // DisableClicking();
    }

    void EnableClicking() {B_canInteract = true;}
    void DisableClicking() {B_canInteract = false;}

    void MoveQuestion()
    {
        Utilities.Instance.ANIM_MoveWithScaleUp(_currentQuestion.transform, IMG_questionImagePanel.transform.position, onCompleteCallBack: () => { UpdateQuestionCounter(); EnableClicking(); AudioManager.PlayAudio(AC_answerClip[_currentIndex]); });
    }

    void MovePrevQuestion()
    {
        if(_prevQuestion == null) return;

        Utilities.Instance.ANIM_Move(_prevQuestion.transform, T_endPoint.position);
    }
}
