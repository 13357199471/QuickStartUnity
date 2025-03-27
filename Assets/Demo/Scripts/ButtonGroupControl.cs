using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ButtonSelectedChangedHandler(int index);
public class ButtonGroupControl : MonoBehaviour {

    // Use this for initialization
    public event ButtonSelectedChangedHandler ButtonSelectedChangeEvent;
    private Button[] buttons;
    Dictionary<int, Transform> _pagesDic;
    bool bInit = false;
    public void SelectedButton(int index) {
        if (bInit == false) {
            Init();
        }
        ButtonSelectedChange(index);
    }
	void Start () {
        if (bInit == false){
            Init();
        }
        ButtonSelectedChange(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Init() {
        bInit = true;
        buttons = this.transform.GetComponentsInChildren<Button>();
        if (buttons == null){
            return;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int j = i;
            buttons[i].onClick.AddListener(() =>
            {
                ButtonSelectedChange(j);
            });
        }

        _pagesDic = new Dictionary<int, Transform>();
        var parent = transform.parent;
        if (parent == null || 
            parent.childCount < 2) {
            return;
        }
      
        var siblingTransform = parent.GetChild(1);
        int nCount = siblingTransform.childCount;
        for (int i = 0; i < nCount; i++) {
            _pagesDic.Add(i, siblingTransform.GetChild(i));
        }
    }
    void ButtonSelectedChange(int i) {
        if (i >= buttons.Length) {
            Debug.Log("button index is illegal");
            return;
        }

        for (int index = 0; index < buttons.Length; index++) {
            if (index == i){
                buttons[index].interactable = false;
            }
            else {
                buttons[index].interactable = true;
            }
        }
        if (ButtonSelectedChangeEvent != null){
            ButtonSelectedChangeEvent(i);
        }else {
            if (!_pagesDic.ContainsKey(i))
            {
                return;
            }
            _pagesDic[i].SetAsLastSibling();
        }
    }
}
