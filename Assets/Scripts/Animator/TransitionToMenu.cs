using EasyTransition;
using UnityEngine;

public class TransitionToMenu : StateMachineBehaviour
{
    public TransitionSettings transition;
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       TransitionManager.Instance.Transition(0, transition, 0);
       BackgroundMusic.Instance.SwitchToWorldMusic();
    }
}
