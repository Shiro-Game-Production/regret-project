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

                // If click count is one, record the time
                if(clickCount == 1) clickTime = Time.time;
                
                // If click count is more than 1 and still on click delay range, return true
                if(clickCount > 1 && Time.time - clickTime < CLICK_DELAY){
                    clickTime = 0;
                    clickCount = 0;

                    return true;
                } 
                // Else if click count is more than 2 or not on click delay range, return false
                else if( clickCount > 2 || Time.time - clickTime > CLICK_DELAY){
                    clickCount = 0;
                    
                    return false;
                }
            }

            return false;
        }
    }
}