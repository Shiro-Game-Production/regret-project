using UnityEngine;

namespace Utils{
    public static class MouseClick{
        [Header("Double click")]
        private const float CLICK_DELAY = 1f;
        private static float clickTime = 0f;
        private static int clickCount;

        public static bool IsDoubleClick(){
            if(Input.GetMouseButtonDown(0)){
                clickCount++;

                if(clickCount == 1) clickTime = Time.time;

                if(clickCount > 1 && Time.time - clickTime < CLICK_DELAY){
                    clickTime = 0;
                    clickCount = 0;
                    Debug.Log("Double click");
                    return true;
                } else if( clickCount > 2 || Time.time - clickTime > CLICK_DELAY){
                    clickCount = 0;
                    Debug.Log("Cancel double click");
                    return false;
                }
            }

            return false;
        }
    }
}