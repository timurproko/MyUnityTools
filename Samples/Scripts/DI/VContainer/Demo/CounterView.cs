#if VCONTAINER
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public sealed class CounterView : MonoInstaller
{
    [SerializeField] private Button incrementButton;
    [SerializeField] private TMP_Text counterLabel;

    public Button Button => incrementButton;
    public TMP_Text Label => counterLabel;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterComponent(this);
    }
}
#endif