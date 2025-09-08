#if MANUAL_DI
using ManualDi.Sync;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CounterView : MonoInstaller
{
    [SerializeField] private Button incrementButton;
    [SerializeField] private TMP_Text counterLabel;

    public Button Button => incrementButton;
    public TMP_Text Label => counterLabel;
    
    public override void Install(DiContainerBindings b)
    {
        b.Bind<CounterView>().FromInstance(this);
    }
}
#endif