using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface W_IState
{
    void Enter();
    void Execute();
    void Exit();
}
