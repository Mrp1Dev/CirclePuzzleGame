using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Slider>().value = Player.Instance.CurrentLevelTimer /
                                       (PuzzleCycler.Instance.EndlessMode
                                           ? Player.Instance.EndlessStartTimer
                                           : Player.Instance.LevelTimerMax);
    }
}
