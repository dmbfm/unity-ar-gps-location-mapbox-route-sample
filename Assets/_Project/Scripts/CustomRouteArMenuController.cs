using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARLocation.MapboxRoutes.SampleProject
{
    public class CustomRouteArMenuController : MonoBehaviour
    {
        public enum StateType
        {
            Closed,
            Open,
            OpenTransition,
            CloseTransition
        }

        [System.Serializable]
        public class StateData
        {
            public StateType CurrentState = StateType.Closed;
            public TweenRectTransformGroup tweenGroup;
        }

        [System.Serializable]
        public class ElementsData
        {
            public Button BtnToggle;
            public Button BtnNext;
            public Button BtnPrev;
            public Button BtnRestart;
            public Button BtnExit;
            public Button BtnLineRender;

            public Text LabelNext;
            public Text LabelPrev;
            public Text LabelRestart;
            public Text LabelSearch;
            public Text LabelTargetRender;

            public RectTransform TargetNext;
            public RectTransform TargetPrev;
            public RectTransform TargetRestart;
            public RectTransform TargetExit;
            public RectTransform TargetLineRender;
        }

        [System.Serializable]
        public class SettingsData
        {
            public MapboxRoute MapboxRoute;
            public CustomRouteMenuController MenuController;
            public float TransitionSpeed = 2.0f;
        }

        public SettingsData Settings;
        public ElementsData Elements;
        private StateData s = new StateData();

        public void Awake()
        {
            s = new StateData();

            showOnlyToggleButton();
        }

        void showOnlyToggleButton()
        {
            Elements.BtnExit.gameObject.SetActive(false);
            Elements.BtnLineRender.gameObject.SetActive(false);
            Elements.BtnNext.gameObject.SetActive(false);
            Elements.BtnPrev.gameObject.SetActive(false);
            Elements.BtnRestart.gameObject.SetActive(false);
            Elements.BtnToggle.gameObject.SetActive(true);
        }

        void showAllButtons()
        {
            Elements.BtnExit.gameObject.SetActive(true);
            Elements.BtnLineRender.gameObject.SetActive(true);
            Elements.BtnNext.gameObject.SetActive(true);
            Elements.BtnPrev.gameObject.SetActive(true);
            Elements.BtnRestart.gameObject.SetActive(true);
            Elements.BtnToggle.gameObject.SetActive(true);
        }

        public void OnEnable()
        {
            Elements.BtnToggle.onClick.AddListener(OnTogglePress);
            Elements.BtnNext.onClick.AddListener(OnNextPress);
            Elements.BtnPrev.onClick.AddListener(OnPrevPress);
            Elements.BtnRestart.onClick.AddListener(OnRestartPress);
            Elements.BtnExit.onClick.AddListener(OnSearchPress);
            Elements.BtnLineRender.onClick.AddListener(OnLineRenderPress);

            updateLineRenderButtonLabel();
        }

        public void OnDisable()
        {
            Elements.BtnToggle.onClick.RemoveListener(OnTogglePress);
            Elements.BtnNext.onClick.RemoveListener(OnNextPress);
            Elements.BtnPrev.onClick.RemoveListener(OnPrevPress);
            Elements.BtnRestart.onClick.RemoveListener(OnRestartPress);
            Elements.BtnExit.onClick.RemoveListener(OnSearchPress);
            Elements.BtnLineRender.onClick.RemoveListener(OnSearchPress);
        }

        private void updateLineRenderButtonLabel()
        {
            var mc = Settings.MenuController;

            if (mc.PathRendererType == CustomRouteMenuController.LineType.Route)
            {
                Elements.LabelTargetRender.text = "Route Path";
            }
            else
            {
                Elements.LabelTargetRender.text = "Line To Target";
            }

        }

        private void OnLineRenderPress()
        {
            var mc = Settings.MenuController;

            if (mc.PathRendererType == CustomRouteMenuController.LineType.Route)
            {
                mc.PathRendererType = CustomRouteMenuController.LineType.NextTarget;
                Elements.LabelTargetRender.text = "Line To Target";
            }
            else
            {
                mc.PathRendererType = CustomRouteMenuController.LineType.Route;
                Elements.LabelTargetRender.text = "Route Path";
            }
        }


        private void OnSearchPress()
        {
            // Settings.MenuController.EndRoute();
        }

        private void OnRestartPress()
        {
            Settings.MapboxRoute.ClosestTarget();
        }

        private void OnPrevPress()
        {
            Settings.MapboxRoute.PrevTarget();
        }

        private void OnNextPress()
        {
            Settings.MapboxRoute.NextTarget();
        }

        private void OnTogglePress()
        {
            toggleMenu();
        }

        void toggleMenu()
        {
            if (s.CurrentState == StateType.Closed)
            {
                openMenu();
            }
            else if (s.CurrentState == StateType.Open)
            {
                closeMenu();
            }
        }

        void openMenu()
        {
            switch (s.CurrentState)
            {
                case StateType.Closed:

                    showAllButtons();

                    s.tweenGroup = new TweenRectTransformGroup(Settings.TransitionSpeed, TweenRectTransformGroup.EaseFunc.EaseInCubic);

                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnNext.GetComponent<RectTransform>(), Elements.TargetNext.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnPrev.GetComponent<RectTransform>(), Elements.TargetPrev.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnRestart.GetComponent<RectTransform>(), Elements.TargetRestart.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnExit.GetComponent<RectTransform>(), Elements.TargetExit.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnLineRender.GetComponent<RectTransform>(), Elements.TargetLineRender.position, Quaternion.identity));

                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnToggle.GetComponent<RectTransform>(), Elements.BtnToggle.GetComponent<RectTransform>().position, Quaternion.Euler(0, 0, 180)));

                    //s.BtnNextTween = new Tween(Elements.BtnNext.GetComponent<RectTransform>().position, Elements.TargetNext.position, Settings.TransitionSpeed);
                    //s.BtnPrevTween = new Tween(Elements.BtnPrev.GetComponent<RectTransform>().position, Elements.TargetPrev.position, Settings.TransitionSpeed);
                    //s.BtnRestartTween = new Tween(Elements.BtnRestart.GetComponent<RectTransform>().position, Elements.TargetRestart.position, Settings.TransitionSpeed);
                    //s.BtnExitTween = new Tween(Elements.BtnExit.GetComponent<RectTransform>().position, Elements.TargetExit.position, Settings.TransitionSpeed);

                    Elements.BtnToggle.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);

                    s.CurrentState = StateType.OpenTransition;

                    break;
            }
        }

        void closeMenu()
        {
            switch (s.CurrentState)
            {
                case StateType.Open:

                    s.tweenGroup = new TweenRectTransformGroup(Settings.TransitionSpeed, TweenRectTransformGroup.EaseFunc.EaseInCubic);

                    var togglerRt = Elements.BtnToggle.GetComponent<RectTransform>();

                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnNext.GetComponent<RectTransform>(), togglerRt.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnPrev.GetComponent<RectTransform>(), togglerRt.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnRestart.GetComponent<RectTransform>(), togglerRt.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnExit.GetComponent<RectTransform>(), togglerRt.position, Quaternion.identity));
                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnLineRender.GetComponent<RectTransform>(), togglerRt.position, Quaternion.identity));

                    s.tweenGroup.Elements.Add(new TweenRectTransform(Elements.BtnToggle.GetComponent<RectTransform>(), Elements.BtnToggle.GetComponent<RectTransform>().position, Quaternion.Euler(0, 0, 0)));

                    //Elements.BtnToggle.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);

                    s.CurrentState = StateType.CloseTransition;

                    //showOnlyToggleButton();
                    //var pos = Elements.BtnToggle.GetComponent<RectTransform>().position;
                    //Elements.BtnNext.GetComponent<RectTransform>().position = pos;
                    //Elements.BtnPrev.GetComponent<RectTransform>().position = pos;
                    //Elements.BtnRestart.GetComponent<RectTransform>().position = pos;
                    //Elements.BtnExit.GetComponent<RectTransform>().position = pos;
                    //Elements.BtnToggle.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    //s.CurrentState = StateType.Closed;
                    break;
            }
        }

        void Start()
        {

        }

        void showButtonLabels()
        {
            Elements.LabelNext.gameObject.SetActive(true);
            Elements.LabelPrev.gameObject.SetActive(true);
            Elements.LabelRestart.gameObject.SetActive(true);
            Elements.LabelSearch.gameObject.SetActive(true);
            Elements.LabelTargetRender.gameObject.SetActive(true);
        }

        void hideButtonLabels()
        {
            Elements.LabelNext.gameObject.SetActive(false);
            Elements.LabelPrev.gameObject.SetActive(false);
            Elements.LabelRestart.gameObject.SetActive(false);
            Elements.LabelTargetRender.gameObject.SetActive(false);
        }

        void Update()
        {
            switch (s.CurrentState)
            {
                case StateType.OpenTransition:
                    if (s.tweenGroup.Update())
                    {
                        showButtonLabels();
                        s.CurrentState = StateType.Open;
                    }

                    break;

                case StateType.CloseTransition:
                    if (s.tweenGroup.Update())
                    {
                        hideButtonLabels();
                        showOnlyToggleButton();
                        s.CurrentState = StateType.Closed;
                    }
                    break;
            }
        }
    }
}
