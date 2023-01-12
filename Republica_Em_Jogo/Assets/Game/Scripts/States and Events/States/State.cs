using Game.Tools;
using System;
using Unity.Netcode;

public abstract class State : NetworkSingleton<State>
{
    public event Action Inicio;
    public event Action Fim;

    public abstract void EnterState();
    public abstract void ExitState();

    public void InvokeInicio()
    {
        Inicio?.Invoke();
        Game.Tools.Logger.Instance.LogInfo("Invoke Inicio");
    }

    public void InvokeFim()
    {
        Fim?.Invoke();
        Game.Tools.Logger.Instance.LogInfo("Invoke Fim");

    }




}
